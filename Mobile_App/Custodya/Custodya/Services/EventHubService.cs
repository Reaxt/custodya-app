using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Services
{
    public class EventHubService : IObservable<string>
    {
        private const int EVENTS_TO_UPDATE_CHECKPOINT = 15;
        private EventProcessorClient _eventProcessor;
        private ConcurrentDictionary<string, int> _partitionEventCount;
        private List<IObserver<string>> _observers;
        private List<string> _telemeteryEvents;
        private Task _eventProcessorTask; //to stop garbage collection
        private IConnectivity _connectivity;
        private SemaphoreSlim _telemetrySemaphore;
        private CancellationTokenSource _eventProcessorNetworkCancel;
        public EventHubService() 
        {
            _eventProcessorNetworkCancel= new CancellationTokenSource();
            _connectivity = Connectivity.Current;
            _telemetrySemaphore = new SemaphoreSlim(1, 1);
            var blobClient = new BlobContainerClient(App.Settings.StorageConnectionString, App.Settings.BlobContainerName);
            _eventProcessor = new EventProcessorClient(blobClient, App.Settings.ConsumerGroup, App.Settings.EndpointConnectionString);
            _partitionEventCount = new ConcurrentDictionary<string, int>();
            _telemeteryEvents = new List<string>();
            _observers = new List<IObserver<string>>();
            _eventProcessor.ProcessEventAsync += ProcessEventHandler;
            _eventProcessor.ProcessErrorAsync += _eventProcessor_ProcessErrorAsync;
            _connectivity.ConnectivityChanged += _connectivity_ConnectivityChanged;
            _eventProcessor.StartProcessing(_eventProcessorNetworkCancel.Token);
        }
        private async void _connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess != NetworkAccess.Internet)
            {
                if (!_eventProcessor.IsRunning)
                {
                    _eventProcessorNetworkCancel.Dispose();
                    _eventProcessorNetworkCancel = new CancellationTokenSource();
                    await _eventProcessor.StartProcessingAsync(_eventProcessorNetworkCancel.Token);
                }
            } else
            {
                _eventProcessorNetworkCancel.Cancel();
            }
        }

        private async Task _eventProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
#if ANDROID
            //Log.Error("EventHubService", $"Error: {arg.Exception.Message}");
#endif
        }

        async Task ProcessEventHandler(ProcessEventArgs args)
        {
            
            if(args.CancellationToken.IsCancellationRequested) return;

            await _telemetrySemaphore.WaitAsync();
            string partition = args.Partition.PartitionId;
            byte[] eventBody = args.Data.EventBody.ToArray();
            string body = args.Data.EventBody.ToString();
            _telemeteryEvents.Add(body);
            foreach(var observer in _observers)
            {
                observer.OnNext(body);
            }

            int eventsSinceLastCheckpoint = _partitionEventCount.AddOrUpdate(
                key:partition,
                addValue:1,
                updateValueFactory: (_, currentCount) => currentCount+1);
            if(eventsSinceLastCheckpoint > EVENTS_TO_UPDATE_CHECKPOINT)
            {
                await args.UpdateCheckpointAsync();
                _partitionEventCount[partition] = 0;
            }
            _telemetrySemaphore.Release();
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if(!_observers.Contains(observer)) {
                _observers.Add(observer);
                GiveObserverEvents(observer);
            }
            return new Unsubscriber<string>(_observers, observer);
        }
        private async void GiveObserverEvents(IObserver<string> observer)
        {
            await _telemetrySemaphore.WaitAsync();
            try
            {
                foreach (string str in _telemeteryEvents)
                {
                    observer.OnNext(str);
                }
            }
            catch (Exception e)
            {
                MauiProgram.Services.GetService<ErrorAlertProviderService>().RaiseError($"err in GiveObserverEvents {e.Message}");
                throw;
            }
            _telemetrySemaphore.Release();
        }
    }
}

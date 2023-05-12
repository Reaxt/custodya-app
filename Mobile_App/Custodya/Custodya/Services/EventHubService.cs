using Android.Util;
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
        public EventHubService() 
        {
            var blobClient = new BlobContainerClient(App.Settings.StorageConnectionString, App.Settings.BlobContainerName);
            _eventProcessor = new EventProcessorClient(blobClient, App.Settings.ConsumerGroup, App.Settings.EventHubConnectionString);
            _partitionEventCount = new ConcurrentDictionary<string, int>();
            _telemeteryEvents = new List<string>();
            _observers = new List<IObserver<string>>();
            _eventProcessor.ProcessEventAsync += ProcessEventHandler;
            _eventProcessor.ProcessErrorAsync += _eventProcessor_ProcessErrorAsync;
            Task.Run(() => _eventProcessor.StartProcessingAsync());
        }

        private async Task _eventProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
#if ANDROID
            Log.Error("EventHubService", $"Error: {arg.Exception.Message}");
#endif
        }

        async Task ProcessEventHandler(ProcessEventArgs args)
        {
            //we want to cancel for some reason !!
            if(args.CancellationToken.IsCancellationRequested) return;

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
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if(!_observers.Contains(observer)) {
                _observers.Add(observer);
                //give the observer any events it may have missed
                foreach (string str in _telemeteryEvents)
                {
                    observer.OnNext(str);
                }
            }
            return new Unsubscriber<string>(_observers, observer);
        }
    }
}

using Custodya.Attributes;
using Custodya.Interfaces;
using Firebase.Database;
using Firebase.Database.Offline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
///Credit: Heavily taken from the example class given during the "MauiFitness" assignment, from Youma Badawy and Aref Mourtada
namespace Custodya.Services
{
    /// <summary>
    /// A generic implementation of realtimedatabase using firebase
    /// </summary>
    /// <remarks>
    /// This has a probability of getting VERY EXPENSIVE with lots of data!
    /// </remarks>
    /// <typeparam name="T">The item for the database</typeparam>
    public class FirebaseRealtimeDatabaseService<T> : IGenericRealtimeDatabase<T> where T : class, IHasUKey, ISubsystemState
    {
        private readonly RealtimeDatabase<T> _realtimeDb;
        private string _dbKey;
        private ObservableCollection<T> _items;
        private FirebaseClient _client;
        private TelemetryDatabaseUpdaterService<T> _updaterService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<T> Items
        {
            //not ideal!!! maybe we could remove this..?
            get
            {
                if(_items == null)
                {
                    Task.Run(() => LoadItems()).Wait();
                }
                return _items;
            }
        }

        public T LatestItem {get; private set;}

        private async Task LoadItems()
        {
            _items = new ObservableCollection<T>(await GetAllItemsAsync());
            LatestItem = _items.Count == 0 ? null : _items.OrderBy(x => x.Timestamp).Last();
        }
        public FirebaseRealtimeDatabaseService()
        {
            FirebaseOptions options = new FirebaseOptions()
            {
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
                AuthTokenAsyncFactory = async () => await AuthService.Client.User.GetIdTokenAsync()
            };
            var client = new FirebaseClient(App.Settings.FireBase_DB_BaseUrl, options);
            string dbKey = nameof(T);
            var attr = typeof(T).GetCustomAttribute<ModelJsonName>();
            if (attr != null)
            {
                //Default to the class name..
                dbKey = attr.Name;
            }
            this._client = client;
            this._dbKey = dbKey;
            _realtimeDb =
                client.Child(dbKey)
                .AsRealtimeDatabase<T>(dbKey, "", StreamingOptions.LatestOnly, InitialPullStrategy.MissingOnly, true);
            _updaterService = new TelemetryDatabaseUpdaterService<T>(this);
            _items = Items;
        }
        public async Task<bool> AddItemAsync(T item)
        {
            try
            {
                item.Key = _realtimeDb.Post(item);
                _realtimeDb.Put(item.Key, item);
                Items.Add(item);
                LatestItem = item;
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(T item)
        {
            try
            {
                _realtimeDb.Delete(item.Key);
                Items.Remove(item);
                LatestItem = await GetLastItemAsync();
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<T>> GetAllItemsAsync(bool forceRefresh = false)
        {
            if (_realtimeDb.Database?.Count == 0 || forceRefresh)
            {
                try
                {
                    await _realtimeDb.PullAsync();

                }
                catch (Exception)
                {
                    return null;
                }
            }
            var result = _realtimeDb.Once().Select(x => x.Object);
            return await Task.FromResult(result);
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            try
            {
                _realtimeDb.Put(item.Key, item);
                T collectionItem = Items.Where(x => x.Key == item.Key).First();
                Items[Items.IndexOf(collectionItem)] = item;
            }
            catch (Exception)
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        public async Task<T> GetLastItemAsync()
        {
            return Items.OrderBy(x => x.Timestamp).First();
        }

        public async Task<bool> CheckExists(T item)
        {
            return Items.Any((x) => x.Timestamp.Equals(item.Timestamp));
        }
    }
}

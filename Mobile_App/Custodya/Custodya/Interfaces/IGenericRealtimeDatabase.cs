

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Custodya.Interfaces
{
    /// <summary>
    /// A generic repository to be implemented elsewhere
    /// </summary>
    /// <typeparam name="T">The item for the repository</typeparam>
    public interface IGenericRealtimeDatabase<T> : IGenericDatabase<T>, INotifyPropertyChanged
    {
        public ObservableCollection<T> Items { get; } //Item to bind to the collection
        /// <summary>
        /// Get all the items in the database 
        /// </summary>
        /// <remarks>
        /// CAN BE EXPENSIVE ON A BIG DATABASE
        /// </remarks>
        /// <param name="forceRefresh">Force us to repull every bit of data</param>
        /// <returns>An IENumerable with the items from the database.</returns>
        Task<IEnumerable<T>> GetAllItemsAsync(bool forceRefresh = false); //Get all items from database
        /// <summary>
        /// Get a range of items from the database
        /// </summary>
        /// <param name="count">The amount of items to grab</param>
        /// <param name="startIndex">The starting index of our query</param>
        /// <returns>An IENumerable with the items from the requested range.</returns>
        //Task<IEnumerable<T>> GetRangeAsync(int count = 15, int startIndex = 0);
        public T LatestItem { get; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custodya.Interfaces
{
    public interface IGenericDatabase<T>
    {
        /// <summary>
        /// Add an item to the database asynchronously
        /// </summary>
        /// <param name="item">The item to add to the database</param>
        /// <returns>A task of bool, true if success, false otherwise.</returns>
        Task<bool> AddItemAsync(T item); //Create operation
        /// <summary>
        /// Update an item in database asynchronously
        /// </summary>
        /// <param name="item">The item with its changes applied</param>
        /// <returns>A task of bool, true if success, false otherwise.</returns>
        Task<bool> UpdateItemAsync(T item); //Update operation
        /// <summary>
        /// Delete an item from the database asynchronously
        /// </summary>
        /// <param name="item">The item to remove from the database</param>
        /// <returns>A task of bool, true if success, false otherwise.</returns>
        Task<bool> DeleteItemAsync(T item); //Delete operation
        /// <summary>
        /// Get the latest item in the database
        /// </summary>
        /// <returns>The newest in the database</returns>
        Task<T> GetLastItemAsync();
        /// <summary>
        /// Check if an item exists.
        /// </summary>
        /// <remarks>
        /// Should be implemented to work even if the item we're checking against does not have a key!
        /// </remarks>
        /// <param name="item">The key we are checking against</param>
        /// <returns>True if the item exists</returns>
        Task<bool> CheckExists(T item);

    }
}

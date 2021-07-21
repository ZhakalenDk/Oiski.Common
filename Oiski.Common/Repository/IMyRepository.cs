using System;
using System.Collections.Generic;
using System.Text;

namespace Oiski.Common.Repository
{
    /// <summary>
    /// Defines a repository <see langword="object"/> from which data can be pushed to or pulled from a <strong>data storage</strong>
    /// </summary>
    /// <typeparam name="EntityType">The type of the <see cref="IMyRepositoryEntity{IDType, SaveType}"/> the repository will handle</typeparam>
    /// <typeparam name="SaveType">The type the <see cref="IMyRepositoryEntity{IDType, SaveType}"/> will save its state as or load a previous state from</typeparam>
    public interface IMyRepository<EntityType, SaveType>
    {
        /// <summary>
        /// Gets the enumerable <see langword="object"/>
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> <see langword="object"/> that can be iteated over</returns>
        IEnumerable<EntityType> GetEnumerable ();
        /// <summary>
        /// Fetch a <typeparamref name="EntityType"/> entry from <strong>data storage</strong>
        /// </summary>
        /// <typeparam name="IDType">The type of the ID of the entry</typeparam>
        /// <param name="_entity">The ID to search for</param>
        /// <returns>The first occurece of type <typeparamref name="EntityType"/> that matches the <paramref name="_entity"/></returns>
        EntityType GetDataByIdentifier<IDType> (IDType _id);
        /// <summary>
        /// Insert a new <typeparamref name="EntityType"/> entry into the <strong>data storage</strong>
        /// </summary>
        /// <param name="_data"></param>
        /// <returns><see langword="true"/> if the entry could be addedd; Othewise <see langword="false"/></returns>
        bool InsertData<IDType> (IMyRepositoryEntity<IDType, SaveType> _data);
        /// <summary>
        /// Delete a <typeparamref name="EntityType"/> entry from the <strong>data storage</strong>
        /// </summary>
        /// <typeparam name="IDType">The type of the ID that identifies the <typeparamref name="EntityType"/> entry</typeparam>
        /// <param name="_entity">The ID of the entry to delete</param>
        /// <returns><see langword="true"/> if the entry could be deleted; Otherwise <see langword="false"/></returns>
        bool DeleteData<IDType> (IMyRepositoryEntity<IDType, SaveType> _entity);
        /// <summary>
        /// Update an <typeparamref name="EntityType"/> entry in <strong>data storage</strong>
        /// </summary>
        /// <param name="_data"></param>
        /// <returns><see langword="true"/> if the <typeparamref name="EntityType"/> entry could be updated; Otherwise <see langword="false"/></returns>
        bool UpdateData<IDType> (IMyRepositoryEntity<IDType, SaveType> _data);
    }
}
//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.Collections.Generic;

namespace Ease.Util.Collections.Generic
{
    /// <summary>
    /// A mapping of keys to a collection of values. Takes care of ensuring that an empty collection is present,
    /// avoiding need for null checks and collection initialization per key prior to use. Eg:
    ///
    /// <code>
    /// var families = new MapOfCollections&lt;string, HashSet&lt;string&gt;, string&gt;();
    /// families["Doe"].Add("Jane");
    /// families["Doe"].Add("John");
    /// var theDoes = string.Join(", ", families["Doe"]);
    /// </code>
    ///
    /// See <seealso cref="MapOfHashSets{TKey, TValue}"/> and <seealso cref="MapOfLists{TKey, TValue}"/> for convenient simplification for
    /// common some cases.
    /// </summary>
    /// <typeparam name="TKey">The lookup key type.</typeparam>
    /// <typeparam name="TCollection">The collection type to store the values in per key.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class MapOfCollections<TKey, TCollection, TValue>
    {
        private readonly Dictionary<TKey, TCollection> _inner = new Dictionary<TKey, TCollection>();
        private readonly Func<TCollection> _allocateNewCollection;
        private readonly Action<TCollection, TValue> _addItemToCollection;
        private readonly Action<TCollection, TValue> _removeItemFromCollection;

        /// <summary>
        /// Allocates a new instance.
        /// 
        /// NOTE: It is recommended that you either inherit and override the <see cref="NewCollection"/>, <see cref="AddValueTo(TCollection, TValue)"/>,
        /// and <see cref="Remove(TKey, TValue)"/> methods _or_ provide the implementations via the optional constructor parameters, but
        /// avoid mixing the approaches.
        /// </summary>
        /// <param name="allocateNewCollection">[optional] Method for allocating a new instance of the collection being mapped to.</param>
        /// <param name="addValueToCollection">[optional] Method for adding an item to the mapped-to collections.</param>
        /// <param name="removeValueFromCollection">[optional] Method for removing an item from the mapped-to collections.</param>
        public MapOfCollections(
            Func<TCollection> allocateNewCollection = null,
            Action<TCollection, TValue> addValueToCollection = null,
            Action<TCollection, TValue> removeValueFromCollection = null)
        {
            _allocateNewCollection = allocateNewCollection;
            _addItemToCollection = addValueToCollection;
            _removeItemFromCollection = removeValueFromCollection;
        }

        /// <summary>
        /// Override to provide implementation for allocating a new instance of <typeparamref name="TCollection"/>, otherwise 
        /// the constructor-provided `allocateNewCollection` will be used.
        /// </summary>
        /// <returns>A new instance of <typeparamref name="TCollection"/></returns>
        protected virtual TCollection NewCollection()
        {
            if (null == _allocateNewCollection)
            {
                throw new InvalidOperationException($"Either provide the ctor param value for [allocateNewCollection] or override [{nameof(NewCollection)}]");
            }
            return _allocateNewCollection();
        }

        /// <summary>
        /// Override to provide implementation for adding a value to an instance of <typeparamref name="TCollection"/>, otherwise 
        /// the constructor-provided `addItemToCollection` will be used.
        /// </summary>
        /// <param name="coll">The collection to which the value should be added.</param>
        /// <param name="value">The value to add.</param>
        protected virtual void AddValueTo(TCollection coll, TValue value)
        {
            if (null == _addItemToCollection)
            {
                throw new InvalidOperationException($"Either provide the ctor param value for [addValueToCollection] or override [{nameof(AddValueTo)}]");
            }
            _addItemToCollection(coll, value);
        }

        /// <summary>
        /// Override to provide implementation for removing a value from an instance of <typeparamref name="TCollection"/>, otherwise 
        /// the constructor-provided `removeItemFromCollection` will be used.
        /// </summary>
        /// <param name="coll">The collection to which the value should be added.</param>
        /// <param name="value">The value to remove.</param>
        protected virtual void RemoveValueFrom(TCollection coll, TValue value)
        {
            if (null == _removeItemFromCollection)
            {
                throw new InvalidOperationException($"Either provide the ctor param value for [removeValueFromCollection] or override [{nameof(RemoveValueFrom)}]");
            }
            _removeItemFromCollection(coll, value);
        }

        private TCollection CollectionAt(TKey key)
        {
            if (!_inner.TryGetValue(key, out var coll))
            {
                coll = NewCollection();
                _inner[key] = coll;
            }
            return coll;
        }

        /// <summary>
        /// Add a value to the collection at <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key for selecting the appropriate collection to which the <paramref name="value"/> will be added.</param>
        /// <param name="value">The value to add to the collection.</param>
        public void Add(TKey key, TValue value)
        {
            AddValueTo(CollectionAt(key), value);
        }

        /// <summary>
        /// Remove the value from the collection at <paramref name="key"/>. If the item does not exist in the collection, nothing happens.
        /// </summary>
        /// <param name="key">The key for selecting the appropriate collection from which the <paramref name="value"/> will be removed.</param>
        /// <param name="value">The value to remove from the collection.</param>
        public void Remove(TKey key, TValue value)
        {
            if (_inner.TryGetValue(key, out var coll))
            {
                RemoveValueFrom(coll, value);
            }
        }

        /// <summary>
        /// Access the collection at the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key for selecting the desired collection.</param>
        /// <returns></returns>
        public TCollection this[TKey key]
        {
            get => CollectionAt(key);
        }

        /// <summary>
        /// Clears the map.
        /// </summary>
        public void Clear()
        {
            _inner.Clear();
        }
    }

    /// <summary>
    /// Convenience implementation of <see cref="MapOfCollections{TKey, TCollection, TValue}"/> for concrete collections implementing
    /// <seealso cref="ICollection{TValue}"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MapOfConcreteCollections<TKey, TCollection, TValue> : MapOfCollections<TKey, TCollection, TValue>
        where TCollection : ICollection<TValue>, new()
    {
        protected override TCollection NewCollection() { return new TCollection(); }
        protected override void AddValueTo(TCollection coll, TValue value) { coll.Add(value); }
        protected override void RemoveValueFrom(TCollection coll, TValue value) { coll.Remove(value); }
    }

    /// <summary>
    /// Convenience implementation of <see cref="MapOfConcreteCollections{TKey, TCollection, TValue}"/> for <seealso cref="HashSet{TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MapOfHashSets<TKey, TValue> : MapOfConcreteCollections<TKey, HashSet<TValue>, TValue> { }

    /// <summary>
    /// Convenience implementation of <see cref="MapOfConcreteCollections{TKey, TCollection, TValue}"/> for <seealso cref="List{TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MapOfLists<TKey, TValue> : MapOfConcreteCollections<TKey, List<TValue>, TValue> { }
}

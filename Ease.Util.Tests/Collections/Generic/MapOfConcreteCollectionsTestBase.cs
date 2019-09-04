//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.Collections.Generic;
using System;
using Ease.Util.Collections.Generic;

namespace Ease.Util.Tests.Collections.Generic
{
    public abstract class MapOfConcreteCollectionsTestBase<TMap, TKey, TCollection, TValue> : MapOfCollectionsTestBase<TMap, TKey, TCollection, TValue>
        where TMap : MapOfConcreteCollections<TKey, TCollection, TValue>, new()
        where TCollection : ICollection<TValue>, new()
    {
        public class WeirdCollectionLikeThing
        {
            public Guid Id { get; set; }
            public HashSet<string> Stuff { get; private set; } = new HashSet<string>();
        }

        protected override bool CollectionContains(TCollection col, TValue value)
        {
            return col.Contains(value);
        }

        protected override long CollectionCount(TCollection col)
        {
            return col.Count;
        }

        protected override TMap NewSut()
        {
            return new TMap();
        }
    }
}

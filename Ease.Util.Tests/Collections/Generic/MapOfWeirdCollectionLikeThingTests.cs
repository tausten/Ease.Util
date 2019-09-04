//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.Collections.Generic;
using System;
using static Ease.Util.Tests.Collections.Generic.MapOfWeirdCollectionLikeThingTests;

namespace Ease.Util.Tests.Collections.Generic
{
    public class MapOfWeirdCollectionLikeThingTests : DirectMapOfCollectionsTestBase<Guid, WeirdCollectionLikeThing, string>
    {
        public class WeirdCollectionLikeThing
        {
            public Guid Id { get; set; }
            public HashSet<string> Stuff { get; private set; } = new HashSet<string>();
        }

        private readonly Guid _mainKey = Guid.NewGuid();
        protected override Guid MainCollectionKey => _mainKey;

        protected override string MainCollectionValue => "Jiggy Wiggy";

        private readonly Guid _secondaryKey = Guid.NewGuid();
        protected override Guid SecondaryCollectionKey => _secondaryKey;

        protected override string SecondaryCollectionValue => "Wagga Wagga";

        protected override bool CollectionContains(WeirdCollectionLikeThing col, string value)
        {
            return col.Stuff.Contains(value);
        }

        protected override long CollectionCount(WeirdCollectionLikeThing col)
        {
            return col.Stuff.Count;
        }

        protected override Action<WeirdCollectionLikeThing, string> GetAddItemToCollectionAction()
        {
            return (col, value) => col.Stuff.Add(value);
        }

        protected override Func<WeirdCollectionLikeThing> GetCtorAllocatorFunc()
        {
            return () => new WeirdCollectionLikeThing();
        }
        protected override Action<WeirdCollectionLikeThing, string> GetRemoveItemFromCollectionAction()
        {
            return (col, value) => col.Stuff.Remove(value);
        }
    }

}

//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.Collections.Generic;
using Ease.Util.Collections.Generic;

namespace Ease.Util.Tests.Collections.Generic
{
    public class MapOfHashSetsTests : MapOfConcreteCollectionsTestBase<MapOfHashSets<int, string>, int, HashSet<string>, string>
    {
        protected override int MainCollectionKey => 123;
        protected override string MainCollectionValue => "Jiggy Wiggy";
        protected override int SecondaryCollectionKey => 456;
        protected override string SecondaryCollectionValue => "Wagga Wagga";
    }
}

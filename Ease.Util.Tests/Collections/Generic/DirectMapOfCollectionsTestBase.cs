//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using FluentAssertions;
using NUnit.Framework;
using Ease.Util.Collections.Generic;
using System;

namespace Ease.Util.Tests.Collections.Generic
{
    public abstract class DirectMapOfCollectionsTestBase<TKey, TCollection, TValue>
        : MapOfCollectionsTestBase<MapOfCollections<TKey, TCollection, TValue>, TKey, TCollection, TValue>
    {
        protected abstract Func<TCollection> GetCtorAllocatorFunc();

        protected abstract Action<TCollection, TValue> GetAddItemToCollectionAction();

        protected abstract Action<TCollection, TValue> GetRemoveItemFromCollectionAction();

        [Flags]
        private enum NonNullCtorParam
        {
            None,

            allocateNewCollection = 0x01,
            addValueToCollection = 0x02,
            removeValueFromCollection = 0x04,
        }

        private MapOfCollections<TKey, TCollection, TValue> NewSutWithCtorParams(NonNullCtorParam nonNullCtorParams = NonNullCtorParam.None)
        {
            return new MapOfCollections<TKey, TCollection, TValue>(
               (nonNullCtorParams & NonNullCtorParam.allocateNewCollection) == NonNullCtorParam.allocateNewCollection ? GetCtorAllocatorFunc() : null,
               (nonNullCtorParams & NonNullCtorParam.addValueToCollection) == NonNullCtorParam.addValueToCollection ? GetAddItemToCollectionAction() : null,
               (nonNullCtorParams & NonNullCtorParam.removeValueFromCollection) == NonNullCtorParam.removeValueFromCollection ? GetRemoveItemFromCollectionAction() : null);
        }

        protected override MapOfCollections<TKey, TCollection, TValue> NewSut()
        {
            return NewSutWithCtorParams(NonNullCtorParam.allocateNewCollection | NonNullCtorParam.addValueToCollection | NonNullCtorParam.removeValueFromCollection);
        }

        [Test]
        public void NullCtorParams_Throw_InvalidOperationException_On_Indexer()
        {
            // Arrange
            var sut = NewSutWithCtorParams();

            // Act
            // Assert
            sut.Invoking(s => s[MainCollectionKey])
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void NullCtorParams_Throw_InvalidOperationException_On_Add()
        {
            // Arrange
            var sut = NewSutWithCtorParams(NonNullCtorParam.allocateNewCollection | NonNullCtorParam.removeValueFromCollection);

            // Act
            // Assert
            sut.Invoking(s => s.Add(MainCollectionKey, MainCollectionValue))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void NullCtorParams_Throw_InvalidOperationException_On_Remove()
        {
            // Arrange
            var sut = NewSutWithCtorParams(NonNullCtorParam.allocateNewCollection | NonNullCtorParam.addValueToCollection);
            sut.Add(MainCollectionKey, MainCollectionValue);

            // Act
            // Assert
            sut.Invoking(s => s.Remove(MainCollectionKey, MainCollectionValue))
                .Should().Throw<InvalidOperationException>();
        }
    }
}

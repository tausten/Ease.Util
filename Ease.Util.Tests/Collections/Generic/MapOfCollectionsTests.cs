//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Extensions;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using Ease.Util.Collections.Generic;
using System;

namespace Ease.Util.Tests.Collections.Generic
{
    public abstract class MapOfCollectionsTestBase<TKey, TCollection, TValue>
    {
        protected abstract Func<TCollection> GetCtorAllocatorFunc();

        protected abstract Action<TCollection, TValue> GetAddItemToCollectionAction();

        protected abstract Action<TCollection, TValue> GetRemoveItemFromCollectionAction();

        protected abstract TKey MainCollectionKey { get; }
        protected abstract TValue MainCollectionValue { get; }

        protected abstract TKey SecondaryCollectionKey { get; }
        protected abstract TValue SecondaryCollectionValue { get; }

        [Flags]
        private enum NonNullCtorParam
        {
            None,
            allocateNewCollection = 0x01,
            addValueToCollection = 0x02,
            removeValueFromCollection = 0x04
        }

        private MapOfCollections<TKey, TCollection, TValue> NewSut(NonNullCtorParam nonNullCtorParams = NonNullCtorParam.None)
        {
            return new MapOfCollections<TKey, TCollection, TValue>(
               (nonNullCtorParams & NonNullCtorParam.allocateNewCollection) == NonNullCtorParam.allocateNewCollection ? GetCtorAllocatorFunc() : null,
               (nonNullCtorParams & NonNullCtorParam.addValueToCollection) == NonNullCtorParam.addValueToCollection ? GetAddItemToCollectionAction() : null,
               (nonNullCtorParams & NonNullCtorParam.removeValueFromCollection) == NonNullCtorParam.removeValueFromCollection ? GetRemoveItemFromCollectionAction() : null);
        }

        private MapOfCollections<int, HashSet<string>, string> NewNonNullCtorParamsSut()
        {
            return new MapOfCollections<int, HashSet<string>, string>(
               () => new HashSet<string>(),
               (col, value) => col.Add(value),
               (col, value) => col.Remove(value)
               );
        }

        [Test]
        public void NullCtorParams_Throw_InvalidOperationException_On_Indexer()
        {
            // Arrange
            var sut = NewSut();

            // Act
            // Assert
            sut.Invoking(s => s[MainCollectionKey])
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void NullCtorParams_Throw_InvalidOperationException_On_Add()
        {
            // Arrange
            var sut = NewSut(NonNullCtorParam.allocateNewCollection | NonNullCtorParam.removeValueFromCollection);

            // Act
            // Assert
            sut.Invoking(s => s.Add(MainCollectionKey, MainCollectionValue))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void NullCtorParams_Throw_InvalidOperationException_On_Remove()
        {
            // Arrange
            var sut = NewSut(NonNullCtorParam.allocateNewCollection | NonNullCtorParam.addValueToCollection);
            sut.Add(MainCollectionKey, MainCollectionValue);

            // Act
            // Assert
            sut.Invoking(s => s.Remove(MainCollectionKey, MainCollectionValue))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Indexer_Returns_Empty_Collection()
        {
            // Arrange
            var sut = NewNonNullCtorParamsSut();

            // Act
            var result = sut[123];

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<HashSet<string>>();
            result.Should().BeEmpty();
        }

        [Test]
        public void Add_Adds_To_Proper_Collection()
        {
            // Arrange
            var sut = NewNonNullCtorParamsSut();
            const int otherCollectionKey = 456;
            sut.Add(otherCollectionKey, "other initial value");

            const int properCollectionKey = 123;
            const string expectedValue = "expected value";

            // Act
            sut.Add(properCollectionKey, expectedValue);

            // Assert
            var properCollection = sut[properCollectionKey];
            properCollection.Count.Should().Be(1);
            properCollection.Should().Contain(expectedValue);
        }
    }

    public class MapOfCollectionsTests : MapOfCollectionsTestBase<int, HashSet<string>, string>
    {
        protected override int MainCollectionKey => 123;

        protected override string MainCollectionValue => "Jiggy Wiggy";

        protected override int SecondaryCollectionKey => 456;

        protected override string SecondaryCollectionValue => "Wagga Wagga";

        protected override Action<HashSet<string>, string> GetAddItemToCollectionAction()
        {
            return (col, value) => col.Add(value);
        }

        protected override Func<HashSet<string>> GetCtorAllocatorFunc()
        {
            return () => new HashSet<string>();
        }
        protected override Action<HashSet<string>, string> GetRemoveItemFromCollectionAction()
        {
            return (col, value) => col.Remove(value);
        }
    }

}

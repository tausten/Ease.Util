//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using FluentAssertions;
using NUnit.Framework;
using Ease.Util.Collections.Generic;

namespace Ease.Util.Tests.Collections.Generic
{
    public abstract class MapOfCollectionsTestBase<TMap, TKey, TCollection, TValue>
        where TMap : MapOfCollections<TKey, TCollection, TValue>
    {
        protected abstract TMap NewSut();

        protected abstract long CollectionCount(TCollection col);

        protected abstract bool CollectionContains(TCollection col, TValue value);

        protected abstract TKey MainCollectionKey { get; }
        protected abstract TValue MainCollectionValue { get; }

        protected abstract TKey SecondaryCollectionKey { get; }
        protected abstract TValue SecondaryCollectionValue { get; }

        [Test]
        public void Indexer_Returns_Empty_Collection()
        {
            // Arrange
            var sut = NewSut();

            // Act
            var result = sut[MainCollectionKey];

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TCollection>();
            CollectionCount(result).Should().Be(0);
        }

        [Test]
        public void Add_Value()
        {
            // Arrange
            var sut = NewSut();
            sut.Add(SecondaryCollectionKey, SecondaryCollectionValue);

            // Act
            sut.Add(MainCollectionKey, MainCollectionValue);

            // Assert
            var properCollection = sut[MainCollectionKey];
            CollectionCount(properCollection).Should().Be(1);
            CollectionContains(properCollection, MainCollectionValue).Should().BeTrue();
        }

        [Test]
        public void Add_Multiple_Values()
        {
            // Arrange
            var sut = NewSut();

            // Act
            sut.Add(MainCollectionKey, MainCollectionValue);
            sut.Add(MainCollectionKey, SecondaryCollectionValue);

            // Assert
            var properCollection = sut[MainCollectionKey];
            CollectionCount(properCollection).Should().Be(2);
            CollectionContains(properCollection, MainCollectionValue).Should().BeTrue();
            CollectionContains(properCollection, SecondaryCollectionValue).Should().BeTrue();
        }

        [Test]
        public void Remove_Value()
        {
            // Arrange
            var sut = NewSut();
            sut.Add(MainCollectionKey, MainCollectionValue);
            var properCollection = sut[MainCollectionKey];

            // Act
            CollectionCount(properCollection).Should().Be(1);
            sut.Remove(MainCollectionKey, MainCollectionValue);

            // Assert
            CollectionContains(properCollection, MainCollectionValue).Should().BeFalse();
            CollectionCount(properCollection).Should().Be(0);
        }
    }
}

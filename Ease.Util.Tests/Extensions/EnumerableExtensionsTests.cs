//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Extensions;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ease.Util.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        public interface ISomeStringBatchHandler
        {
            void Handle<TElement>(IEnumerable<TElement> batch);
        }

        private ISomeStringBatchHandler _mockHandler;

        [SetUp]
        public void SetUp()
        {
            _mockHandler = A.Fake<ISomeStringBatchHandler>();
        }

        #region ImmediateBatch variants
        [Test]
        public void ImmediateBatch_Gracefully_Handles_Null()
        {
            // Arrange
            IEnumerable<string> someEnumerable = null;

            // Act
            var result = someEnumerable.ImmediateBatch(5, _mockHandler.Handle);

            // Assert
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>._)).MustNotHaveHappened();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ImmediateBatch_Gracefully_Handles_Null_No_Action()
        {
            // Arrange
            IEnumerable<object> someEnumerable = null;

            // Act
            var result = someEnumerable.ImmediateBatch(5);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ImmediateBatch_Gracefully_Handles_Empty()
        {
            // Arrange
            IEnumerable<string> someEnumerable = new string[0];

            // Act
            var result = someEnumerable.ImmediateBatch(5, _mockHandler.Handle);

            // Assert
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>._)).MustNotHaveHappened();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ImmediateBatch_Gracefully_Handles_Empty_No_Action()
        {
            // Arrange
            IEnumerable<object> someEnumerable = new object[0];

            // Act
            var result = someEnumerable.ImmediateBatch(5);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void ImmediateBatch_Executes_Action_For_Each_Batch()
        {
            // Arrange
            const int batchSize = 2;
            var someEnumerable = new[] { "one", "two", "three", "four", "five" };
            var expectedNumBatches = (int)Math.Ceiling((double)someEnumerable.Length / batchSize);

            // Act
            var result = someEnumerable.ImmediateBatch(batchSize, _mockHandler.Handle);

            // Assert
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>._)).MustHaveHappened(expectedNumBatches, Times.Exactly);
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>.That.IsSameSequenceAs("one", "two"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>.That.IsSameSequenceAs("three", "four"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>.That.IsSameSequenceAs(new[] { "five" }))).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ImmediateBatch_Returns_Expected_Batches()
        {
            // Arrange
            const int batchSize = 2;
            var someEnumerable = new[] { "one", "two", "three", "four", "five" };
            var expectedNumBatches = (int)Math.Ceiling((double)someEnumerable.Length / batchSize);

            // Act
            var result = someEnumerable.ImmediateBatch(batchSize, _mockHandler.Handle).ToArray();

            // Assert
            result[0].Should().BeEquivalentTo(new[] { "one", "two" });
            result[1].Should().BeEquivalentTo(new[] { "three", "four" });
            result[2].Should().BeEquivalentTo(new[] { "five" });
        }
        #endregion ImmediateBatch

        #region YieldedBatch variants
        [Test]
        public void YieldedBatch_Gracefully_Handles_Null()
        {
            // Arrange
            IEnumerable<object> someEnumerable = null;

            // Act
            var result = someEnumerable.YieldedBatch(5, _mockHandler.Handle);

            // Assert
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<object>>._)).MustNotHaveHappened();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void YieldedBatch_Gracefully_Handles_Null_No_Action()
        {
            // Arrange
            IEnumerable<object> someEnumerable = null;

            // Act
            var result = someEnumerable.YieldedBatch(5);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void YieldedBatch_Gracefully_Handles_Empty()
        {
            // Arrange
            IEnumerable<object> someEnumerable = new object[0];

            // Act
            var result = someEnumerable.YieldedBatch(5, _mockHandler.Handle);

            // Assert
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<object>>._)).MustNotHaveHappened();
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void YieldedBatch_Gracefully_Handles_Empty_No_Action()
        {
            // Arrange
            IEnumerable<object> someEnumerable = new object[0];

            // Act
            var result = someEnumerable.YieldedBatch(5);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void YieldedBatch_Executes_Action_For_Each_Batch_As_Batches_Are_Enumerated()
        {
            // Arrange
            const int batchSize = 2;
            var someEnumerable = new[] { "one", "two", "three", "four", "five" };
            var expectedNumBatches = (int)Math.Ceiling((double)someEnumerable.Length / batchSize);

            // Act
            var result = someEnumerable.YieldedBatch(batchSize, _mockHandler.Handle);

            // Assert
            var currentBatchIndex = 0;
            foreach(var batch in result)
            {
                ++currentBatchIndex;
                A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>._)).MustHaveHappened(currentBatchIndex, Times.Exactly);
            }
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>.That.IsSameSequenceAs("one", "two"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>.That.IsSameSequenceAs("three", "four"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mockHandler.Handle(A<IEnumerable<string>>.That.IsSameSequenceAs(new[] { "five" }))).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void YieldedBatch_Returns_Expected_Batches()
        {
            // Arrange
            const int batchSize = 2;
            var someEnumerable = new[] { "one", "two", "three", "four", "five" };
            var expectedNumBatches = (int)Math.Ceiling((double)someEnumerable.Length / batchSize);

            // Act
            var result = someEnumerable.YieldedBatch(batchSize, _mockHandler.Handle).ToArray();

            // Assert
            result[0].Should().BeEquivalentTo(new[] { "one", "two" });
            result[1].Should().BeEquivalentTo(new[] { "three", "four" });
            result[2].Should().BeEquivalentTo(new[] { "five" });
        }
        #endregion YieldedBatch
    }
}

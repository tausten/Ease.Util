//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ease.Util.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Break the <paramref name="elements"/> up into batches of size <paramref name="batchSize"/>, optionally executing the passed 
        /// <paramref name="batchOperation"/> on each batch before `yield return`ing it.
        /// 
        /// NOTE: If the caller doesn't iterate over the returned IEnumerable of batches, no <paramref name="batchOperation"/> will be executed
        /// because this is lazily iterated. If you want the operations to be executed immediately on all batches, then use 
        /// <see cref="ImmediateBatch{TElement}(IEnumerable{TElement}, int, Action{IEnumerable{TElement}})"/> or be sure to iterate
        /// over the returned `IEnumerable` immediately.
        /// </summary>
        /// <typeparam name="TElement">The type of the individual items in <paramref name="elements"/></typeparam>
        /// <param name="elements">The input sequence to split into batches.</param>
        /// <param name="batchSize">The size of the batches to be returned. NOTE: The final batch may be smaller than this if the input sequence length is not an even multiple of <paramref name="batchSize"/></param>
        /// <param name="batchOperation">[optional] An action to perform on each batch prior to `yield return`ing the batch.</param>
        /// <returns>A lazily computed `IEnumerable` of batches (each batch is itself an `IEnumerable{TElement}`)</returns>
        public static IEnumerable<IEnumerable<TElement>> YieldedBatch<TElement>(this IEnumerable<TElement> elements, int batchSize, Action<IEnumerable<TElement>> batchOperation = null)
        {
            if (null != elements)
            {
                var batch = new List<TElement>();
                foreach (var element in elements)
                {
                    batch.Add(element);
                    if (batch.Count == batchSize)
                    {
                        batchOperation?.Invoke(batch);
                        yield return batch;
                        batch = new List<TElement>();
                    }
                }
                if (batch.Any())
                {
                    batchOperation?.Invoke(batch);
                    yield return batch;
                }
            }
        }

        /// <summary>
        /// Break the <paramref name="elements"/> up into batches of size <paramref name="batchSize"/>, optionally executing the passed 
        /// <paramref name="batchOperation"/> on each batch before `return`ing it. All batches are assembled, and <paramref name="batchOperation"/>
        /// executions performed immediately.
        /// </summary>
        /// <typeparam name="TElement">The type of the individual items in <paramref name="elements"/></typeparam>
        /// <param name="elements">The input sequence to split into batches.</param>
        /// <param name="batchSize">The size of the batches to be returned. NOTE: The final batch may be smaller than this if the input sequence length is not an even multiple of <paramref name="batchSize"/></param>
        /// <param name="batchOperation">[optional] An action to perform on each batch prior to `return`ing the batch.</param>
        /// <returns>A fully evaluated `IEnumerable` of batches (each batch is itself an `IEnumerable{TElement}`)</returns>
        public static IEnumerable<IEnumerable<TElement>> ImmediateBatch<TElement>(this IEnumerable<TElement> elements, int batchSize, Action<IEnumerable<TElement>> batchOperation = null)
        {
            return elements.YieldedBatch(batchSize, batchOperation).ToList();
        }
    }
}

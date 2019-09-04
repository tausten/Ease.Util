//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ease.Util.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the set of strongly-typed values for the specified Enum type <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The Enum type</typeparam>
        /// <returns>The strongly-typed values for the specified Enum type</returns>
        public static IEnumerable<TEnum> GetAllEnumValuesFor<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Extension for getting all valid strongly-typed values of the Enum type of the <paramref name="reference"/>.
        /// </summary>
        /// <typeparam name="TEnum">The Enum type</typeparam>
        /// <param name="reference">The reference from which to obtain the Enum Type</param>
        /// <returns>The strongly-typed values for the specified Enum type</returns>
        public static IEnumerable<TEnum> GetAllEnumValues<TEnum>(this TEnum reference)
        {
            return GetAllEnumValuesFor<TEnum>();
        }
    }
}

//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using Ease.Util.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ease.Util.Tests.Extensions
{
    class StringExtensionsTests
    {
        public enum SomeEnumType
        {
            Undefined = 0,
            First,
            Second,
            Jump = 123,
            AfterJump
        }

        [Flags]
        public enum SomeFlagEnumType
        {
            Undefined = 0x0,
            HasCarbon = 0x01,
            HasHydrogen = 0x02,
            HasOxygen = 0x04,

            IsOrganic = HasCarbon | HasHydrogen | HasOxygen,

            IsLiving = 0x10 | IsOrganic,
            IsPlant = 0x20 | IsLiving,
            IsAnimal = 0x40 | IsLiving

            // Yes.. this makes for some interesting possibilities around Plant-Animals..  ;-)
        }

        public enum InputMode
        {
            Null,
            Empty,
            WhiteSpace,
            Unparsable
        }

        [TestCase(true, InputMode.Null)]
        [TestCase(true, InputMode.Empty)]
        [TestCase(true, InputMode.WhiteSpace)]
        [TestCase(true, InputMode.Unparsable)]
        [TestCase((ushort)1234, InputMode.Null)]
        [TestCase((ushort)1234, InputMode.Empty)]
        [TestCase((ushort)1234, InputMode.WhiteSpace)]
        [TestCase((ushort)1234, InputMode.Unparsable)]
        [TestCase((short)1234, InputMode.Null)]
        [TestCase((short)1234, InputMode.Empty)]
        [TestCase((short)1234, InputMode.WhiteSpace)]
        [TestCase((short)1234, InputMode.Unparsable)]
        [TestCase((int)1234, InputMode.Null)]
        [TestCase((int)1234, InputMode.Empty)]
        [TestCase((int)1234, InputMode.WhiteSpace)]
        [TestCase((int)1234, InputMode.Unparsable)]
        [TestCase((uint)1234, InputMode.Null)]
        [TestCase((uint)1234, InputMode.Empty)]
        [TestCase((uint)1234, InputMode.WhiteSpace)]
        [TestCase((uint)1234, InputMode.Unparsable)]
        [TestCase((long)1234, InputMode.Null)]
        [TestCase((long)1234, InputMode.Empty)]
        [TestCase((long)1234, InputMode.WhiteSpace)]
        [TestCase((long)1234, InputMode.Unparsable)]
        [TestCase((ulong)1234, InputMode.Null)]
        [TestCase((ulong)1234, InputMode.Empty)]
        [TestCase((ulong)1234, InputMode.WhiteSpace)]
        [TestCase((ulong)1234, InputMode.Unparsable)]
        [TestCase((float)12.34, InputMode.Null)]
        [TestCase((float)12.34, InputMode.Empty)]
        [TestCase((float)12.34, InputMode.WhiteSpace)]
        [TestCase((float)12.34, InputMode.Unparsable)]
        [TestCase((double)12.345, InputMode.Null)]
        [TestCase((double)12.345, InputMode.Empty)]
        [TestCase((double)12.345, InputMode.WhiteSpace)]
        [TestCase((double)12.345, InputMode.Unparsable)]
        [TestCase(SomeEnumType.Jump, InputMode.Null)]
        [TestCase(SomeEnumType.Undefined, InputMode.Empty)]
        [TestCase(SomeEnumType.AfterJump, InputMode.WhiteSpace)]
        [TestCase(SomeEnumType.First, InputMode.Unparsable)]
        public void ToValueOr_Returns_Default<T>(T defaultValue, InputMode inputMode)
        {
            // Arrange
            string input;
            switch(inputMode)
            {
                case InputMode.Null:
                    input = null;
                    break;
                case InputMode.Empty:
                    input = string.Empty;
                    break;
                case InputMode.WhiteSpace:
                    input = "  /t /r/n ";
                    break;
                default:
                    input = "Some wickedly invalid string that won't convert";
                    break;
            }

            // Act
            var result = input.ToValueOr(defaultValue);

            // Assert
            result.Should().Be(defaultValue);
        }

        #region Default for non-primitave types
        // Non-primative types can't be used in Attribute parameter values, 
        // so we call the other test and pass the non-primitave type's value.

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_decimal(InputMode inputMode)
        {
            ToValueOr_Returns_Default((decimal)12.345, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_TimeSpan(InputMode inputMode)
        {
            ToValueOr_Returns_Default(TimeSpan.FromSeconds(365), inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_DateTime(InputMode inputMode)
        {
            ToValueOr_Returns_Default(new DateTime(2019, 8, 10, 14, 35, 46, DateTimeKind.Utc), inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_Boolean(InputMode inputMode)
        {
            ToValueOr_Returns_Default((bool?)true, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_ushort(InputMode inputMode)
        {
            ToValueOr_Returns_Default((ushort?)1234, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_short(InputMode inputMode)
        {
            ToValueOr_Returns_Default((short?)1234, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_int(InputMode inputMode)
        {
            ToValueOr_Returns_Default((int?)1234, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_uint(InputMode inputMode)
        {
            ToValueOr_Returns_Default((uint?)1234, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_long(InputMode inputMode)
        {
            ToValueOr_Returns_Default((long?)1234, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_ulong(InputMode inputMode)
        {
            ToValueOr_Returns_Default((ulong?)1234, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_float(InputMode inputMode)
        {
            ToValueOr_Returns_Default((float?)12.34, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_double(InputMode inputMode)
        {
            ToValueOr_Returns_Default((double?)12.345, inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_TimeSpan(InputMode inputMode)
        {
            ToValueOr_Returns_Default((TimeSpan?)TimeSpan.FromSeconds(365), inputMode);
        }

        [TestCase(InputMode.Null)]
        [TestCase(InputMode.Empty)]
        [TestCase(InputMode.WhiteSpace)]
        [TestCase(InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_DateTime(InputMode inputMode)
        {
            ToValueOr_Returns_Default((DateTime?)new DateTime(2019, 8, 10, 14, 35, 46, DateTimeKind.Utc), inputMode);
        }

        [TestCase(SomeEnumType.Jump, InputMode.Null)]
        [TestCase(SomeEnumType.Undefined, InputMode.Empty)]
        [TestCase(SomeEnumType.AfterJump, InputMode.WhiteSpace)]
        [TestCase(SomeEnumType.First, InputMode.Unparsable)]
        public void ToValueOr_Returns_Default_Nullable_Enum(SomeEnumType expectedDefault, InputMode inputMode)
        {
            ToValueOr_Returns_Default((SomeEnumType?)expectedDefault, inputMode);
        }
        #endregion

        public enum FormatProviderMode
        {
            Invariant,
            FrFR,
            EnCA,
            EsES,
            KoKR
        }

        private static readonly Dictionary<FormatProviderMode, CultureInfo> FormatProviders = new Dictionary<FormatProviderMode, CultureInfo>
        {
            {FormatProviderMode.Invariant, CultureInfo.InvariantCulture },
            {FormatProviderMode.FrFR, new CultureInfo("fr-FR", false) },
            {FormatProviderMode.EnCA, new CultureInfo("en-CA", false) },
            {FormatProviderMode.EsES, new CultureInfo("es-ES", false) },
            {FormatProviderMode.KoKR, new CultureInfo("ko-KR", false) },
        };

        [TestCase(true, FormatProviderMode.Invariant)]
        [TestCase(true, FormatProviderMode.FrFR)]
        [TestCase(true, FormatProviderMode.EnCA)]
        [TestCase(true, FormatProviderMode.EsES)]
        [TestCase(true, FormatProviderMode.KoKR)]
        [TestCase((ushort)1234, FormatProviderMode.Invariant)]
        [TestCase((ushort)1234, FormatProviderMode.FrFR)]
        [TestCase((ushort)1234, FormatProviderMode.EnCA)]
        [TestCase((ushort)1234, FormatProviderMode.EsES)]
        [TestCase((ushort)1234, FormatProviderMode.KoKR)]
        [TestCase((short)1234, FormatProviderMode.Invariant)]
        [TestCase((short)1234, FormatProviderMode.FrFR)]
        [TestCase((short)1234, FormatProviderMode.EnCA)]
        [TestCase((short)1234, FormatProviderMode.EsES)]
        [TestCase((short)1234, FormatProviderMode.KoKR)]
        [TestCase((int)1234, FormatProviderMode.Invariant)]
        [TestCase((int)1234, FormatProviderMode.FrFR)]
        [TestCase((int)1234, FormatProviderMode.EnCA)]
        [TestCase((int)1234, FormatProviderMode.EsES)]
        [TestCase((int)1234, FormatProviderMode.KoKR)]
        [TestCase((uint)1234, FormatProviderMode.Invariant)]
        [TestCase((uint)1234, FormatProviderMode.FrFR)]
        [TestCase((uint)1234, FormatProviderMode.EnCA)]
        [TestCase((uint)1234, FormatProviderMode.EsES)]
        [TestCase((uint)1234, FormatProviderMode.KoKR)]
        [TestCase((long)1234, FormatProviderMode.Invariant)]
        [TestCase((long)1234, FormatProviderMode.FrFR)]
        [TestCase((long)1234, FormatProviderMode.EnCA)]
        [TestCase((long)1234, FormatProviderMode.EsES)]
        [TestCase((long)1234, FormatProviderMode.KoKR)]
        [TestCase((ulong)1234, FormatProviderMode.Invariant)]
        [TestCase((ulong)1234, FormatProviderMode.FrFR)]
        [TestCase((ulong)1234, FormatProviderMode.EnCA)]
        [TestCase((ulong)1234, FormatProviderMode.EsES)]
        [TestCase((ulong)1234, FormatProviderMode.KoKR)]
        [TestCase((float)12.34, FormatProviderMode.Invariant)]
        [TestCase((float)12.34, FormatProviderMode.FrFR)]
        [TestCase((float)12.34, FormatProviderMode.EnCA)]
        [TestCase((float)12.34, FormatProviderMode.EsES)]
        [TestCase((float)12.34, FormatProviderMode.KoKR)]
        [TestCase((double)12.345, FormatProviderMode.Invariant)]
        [TestCase((double)12.345, FormatProviderMode.FrFR)]
        [TestCase((double)12.345, FormatProviderMode.EnCA)]
        [TestCase((double)12.345, FormatProviderMode.EsES)]
        [TestCase((double)12.345, FormatProviderMode.KoKR)]
        [TestCase(SomeEnumType.Jump, FormatProviderMode.Invariant)]
        [TestCase(SomeEnumType.Undefined, FormatProviderMode.FrFR)]
        [TestCase(SomeEnumType.AfterJump, FormatProviderMode.EnCA)]
        [TestCase(SomeEnumType.First, FormatProviderMode.EsES)]
        [TestCase(SomeEnumType.Second, FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString<T>(T expectedValue, FormatProviderMode formatProviderMode)
        {
            // Arrange
            string input;
            var originalCulture = CultureInfo.CurrentCulture;
            var targetProvider = FormatProviders[formatProviderMode];
            using (new Scoped(() => CultureInfo.CurrentCulture = targetProvider, () => CultureInfo.CurrentCulture = originalCulture))
            {
                input = expectedValue.ToString();
            }

            // Act
            var result = input.ToValueOr((T)default, targetProvider);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_decimal(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((decimal)12.345, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_TimeSpan(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString(TimeSpan.FromMinutes(365), formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_DateTime(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString(new DateTime(2019, 8, 10, 14, 35, 46, DateTimeKind.Utc), formatProviderMode);
        }

        #region Value From ToString for non-primitave types
        // Non-primative types can't be used in Attribute parameter values, 
        // so we call the other test and pass the non-primitave type's value.

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_Boolean(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((bool?)true, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_ushort(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((ushort?)1234, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_short(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((short?)1234, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_int(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((int?)1234, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_uint(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((uint?)1234, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_long(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((long?)1234, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_ulong(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((ulong?)1234, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_float(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((float?)12.34, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_double(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((double?)12.345, formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_TimeSpan(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((TimeSpan?)TimeSpan.FromMinutes(365), formatProviderMode);
        }

        [TestCase(FormatProviderMode.Invariant)]
        [TestCase(FormatProviderMode.FrFR)]
        [TestCase(FormatProviderMode.EnCA)]
        [TestCase(FormatProviderMode.EsES)]
        [TestCase(FormatProviderMode.KoKR)]
        public void ToValueOr_Converts_Value_From_ToString_Nullable_DateTime(FormatProviderMode formatProviderMode)
        {
            ToValueOr_Converts_Value_From_ToString((DateTime?)new DateTime(2019, 8, 10, 14, 35, 46, DateTimeKind.Utc), formatProviderMode);
        }
        #endregion

        [TestCase("HasCarbon, HasHydrogen, HasOxygen", SomeFlagEnumType.IsOrganic)]
        [TestCase("IsOrganic", SomeFlagEnumType.HasCarbon | SomeFlagEnumType.HasHydrogen | SomeFlagEnumType.HasOxygen)]
        [TestCase("HasCarbon, HasHydrogen", SomeFlagEnumType.HasCarbon | SomeFlagEnumType.HasHydrogen)]
        [TestCase("IsAnimal", SomeFlagEnumType.IsAnimal)]
        public void ToValueOr_Converts_Value_From<T>(string input, T expectedValue)
        {
            // Arrange
            // Act
            var result = input.ToValueOr((T)default);

            // Assert
            result.Should().Be(expectedValue);
        }

    }
}

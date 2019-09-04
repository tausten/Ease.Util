//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;

namespace Ease.Util.Tests.Extensions
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
}

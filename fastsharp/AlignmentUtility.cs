﻿using System.Numerics;

namespace FastSharp;

internal class AlignmentUtility
{
    public static T Align<T>(T value, T alignment)
        where T : IBinaryInteger<T>
    {
        return value + (alignment - (value % alignment));
    }
}

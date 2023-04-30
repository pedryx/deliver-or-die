using System;

namespace DeliverOrDie;
internal static class RandomExtension
{
    /// <summary>
    /// Generate random angle in radians in range [0; 2pi).
    /// </summary>
    public static float NextAngle(this Random random)
        => random.NextSingle() * 2 * MathF.PI;
}

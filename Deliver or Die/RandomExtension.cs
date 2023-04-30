using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie;
internal static class RandomExtension
{
    /// <summary>
    /// Generate random angle in radians in range [0; 2pi).
    /// </summary>
    public static float NextAngle(this Random random)
        => random.NextSingle() * 2 * MathF.PI;

    /// <summary>
    /// Generate random point int bounded space.
    /// </summary>
    /// <param name="bounds">Size of bounded space.</param>
    public static Vector2 Nextvector2(this Random random, Vector2 bounds)
        => new Vector2(random.NextSingle(), random.NextSingle()) * bounds;
}

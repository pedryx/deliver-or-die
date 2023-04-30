using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie;
/// <summary>
/// utility class for math.
/// </summary>
internal static class MathUtils
{
    /// <summary>
    /// Normalize angle so its in interval [0, 2pi).
    /// </summary>
    /// <param name="angle">Angle to normalize.</param>
    /// <returns>Normalized angle.</returns>
    public static float NormalizeAngle(float angle)
    {
        if (angle < 0.0f)
            angle += 2 * MathF.PI;
        if (angle >= 2 * MathF.PI)
            angle -= 2 * MathF.PI;

        return angle;
    }

    public static Vector2 AngleToVector(float angle)
        => new(MathF.Cos(angle), MathF.Sin(angle));

    public static float VectorToAngle(Vector2 vector)
        => MathF.Atan2(vector.Y, vector.X);
}

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
}

using Microsoft.Xna.Framework;

namespace DeliverOrDie.Components;
/// <summary>
/// Transform component.
/// </summary>
public struct Transform
{
    /// <summary>
    /// Position of the entity.
    /// </summary>
    public Vector2 Position;
    /// <summary>
    /// Scale of the entity.
    /// </summary>
    public float Scale;
    /// <summary>
    /// Rotation of the entity.
    /// </summary>
    public float Rotation;

    public static Transform Create(Vector2 position)
        => new Transform()
        {
            Position = position,
            Scale = 1.0f,
        };

    public static Transform Create()
        => Create(Vector2.Zero);
}

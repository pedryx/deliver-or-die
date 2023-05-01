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

    public Transform(Vector2 position)
    {
        Scale = 1.0f;
        Position = position;
    }

    public Transform()
        : this(Vector2.Zero) { }
}

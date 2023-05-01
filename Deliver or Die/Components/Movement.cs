namespace DeliverOrDie.Components;
/// <summary>
/// Movement component.
/// </summary>
internal struct Movement
{
    /// <summary>
    /// Movement direction in radians.
    /// </summary>
    public float Direction;
    /// <summary>
    /// Movement speed in pixels per second.
    /// </summary>
    public float Speed;

    public Movement(float speed = 0.0f, float direction = 0.0f)
    {
        Speed = speed;
        Direction = direction;
    }
}

using HypEcs;

namespace DeliverOrDie.Events;
/// <summary>
/// Handler for collision event.
/// </summary>
/// <param name="sender">Event sender.</param>
/// <param name="e">Event arguments.</param>
internal delegate void CollisionEventHandler(object sender, CollisionEventArgs e);

/// <summary>
/// Arguments for collision event.
/// </summary>
internal class CollisionEventArgs
{
    public Entity Self;
    public Entity Target;

    public CollisionEventArgs(Entity self, Entity target)
    {
        Self = self;
        Target = target;
    }
}

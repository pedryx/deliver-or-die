using HypEcs;

namespace DeliverOrDie.Events;
/// <summary>
/// Represent handler for entity related events.
/// </summary>
/// <param name="sender">Event sender.</param>
/// <param name="e">Event arguments.</param>
internal delegate void EntityEventHandler(object sender, EntityEventArgs e);

/// <summary>
/// Arguments for entity related events.
/// </summary>
internal class EntityEventArgs
{
    public Entity Self;

    public EntityEventArgs(Entity self)
    {
        Self = self;
    }
}

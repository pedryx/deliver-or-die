using System;
using DeliverOrDie.Events;

namespace DeliverOrDie.Components;
/// <summary>
/// Represent simple 2D circle collider.
/// </summary>
internal struct Collider
{
    /// <summary>
    /// To which layers entity deal damage.
    /// </summary>
    public Layers DamageLayer;
    /// <summary>
    /// To which layer entity collides, if target entity has <see cref="Movement"/> and matches this layer then
    /// her position is shifted, if <see cref="DestroyOnImpact"/> is set to true and entity is destroyed.
    /// </summary>
    public Layers CollisionLayer;
    /// <summary>
    /// To which layer entity reacts (<see cref="OnCollision"/> event is triggered).
    /// </summary>
    public Layers ReactionLayer;
    /// <summary>
    /// To which layers entity belongs.
    /// </summary>
    public Layers Layer;

    /// <summary>
    /// Circle radius.
    /// </summary>
    public float Radius;
    /// <summary>
    /// Damage deal to target entity if they collide, target's <see cref="DamageLayer"/> matches
    /// and target entity has <see cref="Health"/> component.
    /// </summary>
    public float Damage;

    /// <summary>
    /// Occur on collision with target entity, if they collide and layer of target entity is in
    /// <see cref="ReactionLayer"/>.
    /// </summary>
    public CollisionEventHandler OnCollision;

    /// <summary>
    /// Determine if entity will destroyed when it collides with target entity which matches
    /// <see cref="CollisionLayer"/>.
    /// matches.
    /// </summary>
    public bool DestroyOnImpact;
    public int EntityIndex;

    public Collider(int entityIndex, float radius, Layers layer = Layers.None, float damage = 0.0f)
    {
        EntityIndex = entityIndex;
        Radius = radius;
        Layer = layer;
        Damage = damage;
    }

    /// <summary>
    /// Layers for collisions checking, layer matches if (layer1 & layer2) > 0.
    /// </summary>
    [Flags]
    public enum Layers
    {
        None          = 0x00,
        Player        = 0x01,
        Bullet        = 0x02,
        Zombie        = 0x04,
        Obstacle      = 0x08,
        DeliverySpot  = 0x10,
    }
}
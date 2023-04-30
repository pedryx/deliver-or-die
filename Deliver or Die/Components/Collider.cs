using System;

namespace DeliverOrDie.Components;
internal struct Collider
{
    public Layers DamageLayer;
    public Layers CollisionLayer;
    public Layers ReactionLayer;
    public Layers Layer;

    public float Radius;
    public float Damage;

    public Action<object> Reaction;
    public object Data;

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

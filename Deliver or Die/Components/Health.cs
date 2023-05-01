using DeliverOrDie.Events;

namespace DeliverOrDie.Components;
internal struct Health
{
    public float Current;
    public float Max;

    public CollisionEventHandler OnDead;
    public CollisionEventHandler OnHit;

    public Health(float health)
    {
        Max = health;
        Current = health;
    }
}

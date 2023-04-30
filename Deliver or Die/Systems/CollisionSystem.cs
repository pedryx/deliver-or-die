using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;

namespace DeliverOrDie.Systems;
internal class CollisionSystem : GameSystem<Transform, Collider>
{
    private readonly Query<Transform, Collider, Movement, Health> movementHealthQuery;
    private readonly Query<Transform, Collider, Movement> movementQuery;
    private readonly Query<Transform, Collider, Health> healthQuery;

    public CollisionSystem(GameState gameState)
        : base(gameState)
    {
        movementHealthQuery = gameState.ECSWorld.Query<Transform, Collider, Movement, Health>().Build();
        movementQuery = gameState.ECSWorld.Query<Transform, Collider, Movement>().Not<Health>().Build();
        healthQuery = gameState.ECSWorld.Query<Transform, Collider, Health>().Not<Movement>().Build();
    }

    protected override void Update(ref Transform transform, ref Collider collider)
    {
        Vector2 position = transform.Position;
        float radius = collider.Radius;
        Collider.Layers damageLayer = collider.DamageLayer;
        Collider.Layers collisionLayer = collider.CollisionLayer;
        float damage = collider.Damage;

        movementHealthQuery.Run((count, transforms, colliders, movements, healths) =>
        {
            for (int i = 0; i < count; i++)
            {
                float distance = Vector2.Distance(position, transforms[i].Position);

                if (distance < radius + colliders[i].Radius)
                {
                    if ((collisionLayer & colliders[i].Layer) > 0)
                        RevertMovement(position, radius, colliders[i].Radius, movements[i].Speed, ref transforms[i]);
                    if ((damageLayer & colliders[i].Layer) > 0)
                        DealDamage(damage, ref transforms[i], ref healths[i]);
                }
            }
        });

        movementQuery.Run((count, transforms, colliders, movements) =>
        {
            for (int i = 0; i < count; i++)
            {
                float distance = Vector2.Distance(position, transforms[i].Position);

                if (distance < radius + colliders[i].Radius)
                {
                    if ((collisionLayer & colliders[i].Layer) > 0)
                        RevertMovement(position, radius, colliders[i].Radius, movements[i].Speed, ref transforms[i]);
                }
            }
        });

        healthQuery.Run((count, transforms, colliders, healths) =>
        {
            for (int i = 0; i < count; i++)
            {
                float distance = Vector2.Distance(position, transforms[i].Position);

                if (distance < radius + colliders[i].Radius)
                {
                    if ((damageLayer & colliders[i].Layer) > 0)
                        DealDamage(damage, ref transforms[i], ref healths[i]);
                }
            }
        });
    }

    private void RevertMovement(Vector2 positon1, float radius1, float radius2, float speed2, ref Transform transform2)
    {
        if (speed2 == 0.0f)
            return;

        Vector2 direction = transform2.Position - positon1;
        direction.Normalize();

        float distance = radius1 + radius2 - Vector2.Distance(transform2.Position, positon1);

        transform2.Position += direction * distance;
    }

    private void DealDamage(float damage, ref Transform transform, ref Health health)
    {
        health.Current -= damage;

        if (health.Current < 0)
        {
            health.OnDead?.Invoke(transform.Position);
            GameState.DestroyEntity(health.EntityIndex);
        }
    }
}

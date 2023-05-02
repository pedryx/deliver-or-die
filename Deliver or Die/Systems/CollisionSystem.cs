using DeliverOrDie.Components;
using DeliverOrDie.Events;
using DeliverOrDie.Extensions;

using HypEcs;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using UltimateQuadTree;

namespace DeliverOrDie.Systems;

/// <summary>
/// Handles collision between entities.
/// </summary>
internal class CollisionSystem : GameSystem<Transform, Collider>
{
    private readonly Query<Transform, Collider> query;
    private readonly World ecsWorld;
    //private readonly HashSet<int> quadTreeEntities = new();
    private readonly Vector2 worldSize;

    private QuadTree<Entity> quadTree;

    public CollisionSystem(GameState gameState, Vector2 worldSize)
        : base(gameState)
    {
        this.worldSize = worldSize;
        ecsWorld = gameState.ECSWorld;
        query = ecsWorld.Query<Transform, Collider>().Build();
    }

    protected override void PreUpdate()
    {
        quadTree = new QuadTree<Entity>(worldSize.X, worldSize.Y, new QuadTreeEntityBounds(ecsWorld));

        query.Run((count, transforms, colliders) =>
        {
            for (int i = 0; i < count; i++)
            {
                quadTree.Insert(GameState.GetEntity(colliders[i].EntityIndex));
            }
        });
    }

    protected override void Update(ref Transform transformReference, ref Collider colliderReference)
    {
        Entity entity1 = GameState.GetEntity(colliderReference.EntityIndex);

        var entities = quadTree.GetNearestObjects(entity1);
        foreach (var entity2 in entities)
        {
            HandleCollisions(entity1, entity2);
        }
    }

    private void HandleCollisions(Entity entity1, Entity entity2)
    {
        ref Transform transform1 = ref ecsWorld.GetComponent<Transform>(entity1);
        ref Collider collider1 = ref ecsWorld.GetComponent<Collider>(entity1);
        ref Transform transform2 = ref ecsWorld.GetComponent<Transform>(entity2);
        ref Collider collider2 = ref ecsWorld.GetComponent<Collider>(entity2);

        if (collider1.EntityIndex == collider2.EntityIndex)
            return;

        float distance = Vector2.Distance(transform1.Position, transform2.Position);
        if (distance > collider1.Radius + collider2.Radius)
            return;

        // reaction
        if ((collider1.ReactionLayer & collider2.Layer) > 0)
            collider1.OnCollision?.Invoke(this, new CollisionEventArgs(entity1, entity2));

        // damage
        if ((collider1.DamageLayer & collider2.Layer) > 0)
        {
            if (ecsWorld.HasComponent<Health>(entity2))
            {
                ref Health health2 = ref ecsWorld.GetComponent<Health>(entity2);

                health2.Current -= collider1.Damage;

                if (health2.Current <= 0.0f)
                {
                    health2.OnDead?.Invoke(this, new CollisionEventArgs(entity2, entity1));
                    RemoveFromQuadTree(entity2, collider2.EntityIndex);
                    GameState.DestroyEntity(collider2.EntityIndex);
                }
                else
                {
                    health2.OnHit?.Invoke(this, new CollisionEventArgs(entity2, entity1));
                }
            }
        }

        // impact
        if ((collider1.CollisionLayer & collider2.Layer) > 0)
        {
            if (ecsWorld.HasComponent<Movement>(entity1))
            {
                ref Movement movement = ref ecsWorld.GetComponent<Movement>(entity1);

                if (movement.Speed != 0.0f)
                {
                    Vector2 direction = transform1.Position - transform2.Position;

                    if (direction == Vector2.Zero)
                        direction = MathUtils.AngleToVector(GameState.Game.Random.NextAngle());

                    direction.Normalize();


                    float offset = collider1.Radius + collider2.Radius - distance;
                    transform1.Position += direction * offset;
                }
            }

            if (collider1.DestroyOnImpact)
            {
                RemoveFromQuadTree(entity1, collider1.EntityIndex);
                GameState.DestroyEntity(collider1.EntityIndex);
            }
        }
    }

    public void RemoveFromQuadTree(Entity entity, int index)
    {
        //quadTreeEntities.Remove(index);
        quadTree.Remove(entity);
    }

    private struct CollisionComponents
    {
        public Transform Transform1;
        public Collider Collider1;
        public Transform Transform2;
        public Collider Collider2;
    }
}

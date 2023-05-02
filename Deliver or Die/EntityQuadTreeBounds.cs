using DeliverOrDie.Components;
using DeliverOrDie.GameStates.Level;

using HypEcs;

using Microsoft.Xna.Framework;

using UltimateQuadTree;

namespace DeliverOrDie;
internal class QuadTreeEntityBounds : IQuadTreeObjectBounds<Entity>
{
    private World ecsWorld;

    public QuadTreeEntityBounds(World ecsWorld)
    {
        this.ecsWorld = ecsWorld;
    }

    public Rectangle GetBounds(Entity entity)
    {
        Transform transform = ecsWorld.GetComponent<Transform>(entity);
        Collider collider = ecsWorld.GetComponent<Collider>(entity);

        return new Rectangle()
        {
            Location = (transform.Position - new Vector2(collider.Radius) + WorldGenerator.WorldSize / 2.0f).ToPoint(),
            Size = new Point((int)collider.Radius),
        };
    }

    public double GetBottom(Entity entity)
        => GetBounds(entity).Bottom;

    public double GetLeft(Entity entity)
        => GetBounds(entity).Left;

    public double GetRight(Entity entity)
        => GetBounds(entity).Right;

    public double GetTop(Entity entity)
        => GetBounds(entity).Top;
}

using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;

namespace DeliverOrDie;
/// <summary>
/// Simple 2D camera.
/// </summary>
internal class Camera
{
    private readonly GameState gameState;

    public Entity? Target;

    public Vector2 Position = Vector2.Zero;
    public float Scale = 3.0f;
    public float Rotation = 0.0f;

    public Camera(GameState gameState)
    {
        this.gameState = gameState;
    }

    public void Update(World ecsWorld)
    {
        if (Target == null)
            return;

        Position = gameState.ECSWorld.GetComponent<Transform>(Target.Value).Position;
    }

    public Matrix GetTransformMatrix()
        => Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
        * Matrix.CreateScale(Scale, Scale, 1.0f)
        * Matrix.CreateRotationZ(Rotation)
        * Matrix.CreateTranslation(gameState.Game.Resolution.X / 2, gameState.Game.Resolution.Y / 2, 0);
}

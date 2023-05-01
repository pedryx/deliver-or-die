using DeliverOrDie.Components;
using DeliverOrDie.Extensions;

using HypEcs;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie;
/// <summary>
/// Simple 2D camera.
/// </summary>
internal class Camera
{
    private readonly GameState gameState;
    private readonly Random random;

    private float shakeElapsed;
    private bool shakeActive;
    private float shakeDuration;
    private float shakeMagnitude;

    public Entity? Target;

    public Vector2 Position = Vector2.Zero;
    public float Scale = 3.0f;
    public float Rotation = 0.0f;

    public Camera(GameState gameState)
    {
        this.gameState = gameState;
        random = gameState.Game.Random;
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeActive = true;
    }

    public void Update()
    {
        if (Target != null)
            Position = gameState.ECSWorld.GetComponent<Transform>(Target.Value).Position;

        if (shakeActive)
        {
            Position += random.NextUnitVector() * shakeMagnitude;

            shakeElapsed += gameState.Elapsed * gameState.Game.Speed;
            if (shakeElapsed >= shakeDuration)
            {
                shakeElapsed = 0.0f;
                shakeActive = false;
            }
        }
    }

    public Matrix GetTransformMatrix()
        => Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
        * Matrix.CreateScale(Scale, Scale, 1.0f)
        * Matrix.CreateRotationZ(Rotation)
        * Matrix.CreateTranslation(gameState.Game.Resolution.X / 2, gameState.Game.Resolution.Y / 2, 0);
}

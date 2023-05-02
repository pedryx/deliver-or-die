using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DeliverOrDie.Systems;
/// <summary>
/// handles control of camera's scale (zooming in/out).
/// </summary>
internal class CameraControlSystem : GameSystem
{
    /// <summary>
    /// Speed of zooming in/out.
    /// </summary>
    private const float zoomSpeed = 5.0f;
    /// <summary>
    /// Minimum zoomed value.
    /// </summary>
    private const float minZoom = 0.6f;
    /// <summary>
    /// Maximum zoomed value.
    /// </summary>
    private const float maxZoom = 0.9f;

    /// <summary>
    /// Camera which is being controlled.
    /// </summary>
    private readonly Camera camera;

    /// <summary>
    /// Mouse state from previous game loop iteration.
    /// </summary>
    private MouseState lastMouseState;

    public CameraControlSystem(GameState gameState)
        : base(gameState)
    {
        camera = gameState.Camera;
        camera.Scale = (maxZoom - minZoom) / 2.0f + minZoom;
        lastMouseState = Mouse.GetState();
    }

    protected override void Update()
    {
        MouseState mouseState = Mouse.GetState();

        float zoomDirection = 0.0f;
        if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
            zoomDirection = 1.0f;
        else if (mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
            zoomDirection = -1.0f;

        camera.Scale *= (1.0f + zoomDirection * zoomSpeed * GameState.Elapsed);
        camera.Scale = MathHelper.Clamp(camera.Scale, minZoom, maxZoom);

        lastMouseState = mouseState;
    }
}

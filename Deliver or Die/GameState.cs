using DeliverOrDie.UI;

using HypEcs;

namespace DeliverOrDie;
/// <summary>
/// Represent "game screen".
/// </summary>
internal class GameState
{
    /// <summary>
    /// ECS world local to this game state.
    /// </summary>
    public World ECSWorld { get; private set; } = new();
    public LDGame Game { get; private set; }
    /// <summary>
    /// Camera local to this game state.
    /// </summary>
    public Camera Camera { get; private set; }
    public UILayer UILayer { get; private set; }

    /// <summary>
    /// Systems used in update call.
    /// </summary>
    protected SystemGroup UpdateSystems { get; private set; } = new();
    /// <summary>
    /// Systems used in draw call.
    /// </summary>
    protected SystemGroup RenderSystems { get; private set; } = new();
    /// <summary>
    /// Time elapsed between update/draw calls.
    /// </summary>
    public float Elapsed { get; private set; }

    /// <summary>
    /// Create entities and systems.
    /// </summary>
    protected virtual void Initialize() { }

    public void Initialize(LDGame game)
    {
        Camera = new Camera(this);
        Game = game;
        UILayer = new UILayer(this);
        Initialize();
    }

    public void Update(float elapsed)
    {
        Elapsed = elapsed;
     
        UpdateSystems.Run(ECSWorld);
        UILayer.Update(Elapsed);
        Camera.Update();
    }

    public void Draw(float elapsed)
    {
        Elapsed = elapsed;
        
        RenderSystems.Run(ECSWorld);
        UILayer.Draw(Elapsed);
    }
}

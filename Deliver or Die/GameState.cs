using DeliverOrDie.UI;

using HypEcs;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using UltimateQuadTree;

namespace DeliverOrDie;
/// <summary>
/// Represent "game screen".
/// </summary>
internal abstract class GameState
{
    /// <summary>
    /// Contains tracked entities.
    /// </summary>
    private readonly Dictionary<int, Entity> entities = new();
    private readonly List<int> toRemove = new();

    /// <summary>
    /// index of next tracked entity.
    /// </summary>
    private int lastIndex = 0;

    /// <summary>
    /// ECS world local to this game state.
    /// </summary>
    public World ECSWorld { get; private set; } = new();
    public LDGame Game { get; private set; }
    /// <summary>
    /// Camera local to this game state.
    /// </summary>
    public Camera Camera { get; private set; }
    /// <summary>
    /// User interface layer local to this game state.
    /// </summary>
    public UILayer UILayer { get; private set; }
    /// <summary>
    /// Time elapsed between update/draw calls.
    /// </summary>
    public float Elapsed { get; private set; }
    /// <summary>
    /// Determine if state is visible (<see cref="Draw(float)"/> method is being called).
    /// </summary>
    public bool Visible = true;
    /// <summary>
    /// Determine if state is enabled (<see cref="Update(float)"/> method is being called).
    /// </summary>
    public bool Enabled = true;

    /// <summary>
    /// Systems used in update call.
    /// </summary>
    protected SystemGroup UpdateSystems { get; private set; } = new();
    /// <summary>
    /// Systems used in draw call.
    /// </summary>
    protected SystemGroup RenderSystems { get; private set; } = new();

    /// <summary>
    /// Create entities and systems.
    /// </summary>
    protected virtual void Initialize() { }

    /// <summary>
    /// Get index of next tracked entity. To track entity call <see cref="AddEntity(Entity)"/>.
    /// </summary>
    /// <returns>Index of next called entity.</returns>
    public int GetNextIndex()
    {
        return lastIndex;
    }

    /// <summary>
    /// Track entity by game system so it can be destroyed in future. To obtain index of tracked entity call
    /// <see cref="GetNextIndex"/> begore calling this method.
    /// </summary>
    /// <param name="entity">Entity to track.</param>
    public void AddEntity(Entity entity)
    {
        entities.Add(lastIndex, entity);
        lastIndex++;
    }

    /// <summary>
    /// Get tracked entity.
    /// </summary>
    /// <param name="index">Index of entity to get.</param>
    public Entity GetEntity(int index)
        => entities[index];

    /// <summary>
    /// Destroy entity tracked by this game state. To track entity call <see cref="AddEntity(Entity)"/> method.
    /// Entity will get destroyed at the end of current frame.
    /// </summary>
    /// <param name="index">index of entity to destroy.</param>
    public void DestroyEntity(int index)
        => toRemove.Add(index);

    public void Initialize(LDGame game)
    {
        Game = game;

        Camera = new Camera(this);
        UILayer = new UILayer(this);
        Initialize();
    }

    public void Update(float elapsed)
    {
        if (!Enabled)
            return;

        Elapsed = elapsed;
     
        UpdateSystems.Run(ECSWorld);
        UILayer.Update(Elapsed);
        Camera.Update();

        foreach (var i in toRemove)
        {
            if (!entities.ContainsKey(i))
                continue;

            ECSWorld.Despawn(entities[i]);
            entities.Remove(i);
        }
        toRemove.Clear();

        ECSWorld.Tick();
    }

    public void Draw(float elapsed)
    {
        if (!Visible)
            return;

        Elapsed = elapsed;
        
        RenderSystems.Run(ECSWorld);
        UILayer.Draw(Elapsed);
    }
}

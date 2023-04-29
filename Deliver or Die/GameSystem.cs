using HypEcs;

namespace DeliverOrDie;
internal abstract class GameSystem : ISystem
{
    /// <summary>
    /// Game state which owns this game system.
    /// </summary>
    public GameState GameState { get; private set; }

    public GameSystem(GameState gameState)
    {
        GameState = gameState;
    }

    public void Run(World world)
    {
        PreUpdate();
        Update();
        PostUpdate();
    }

    protected virtual void PreUpdate() { }
    protected abstract void Update();
    protected virtual void PostUpdate() { }
}

internal abstract class GameSystem<T1> : GameSystem
    where T1 : struct
{
    private readonly Query<T1> query;

    protected GameSystem(GameState gameState)
        : base(gameState) 
    {
        query = CreateQuery();
    }

    protected virtual Query<T1> CreateQuery()
        => GameState.ECSWorld.Query<T1>().Build();

    protected override void Update()
    {
        query.Run((count, components1) =>
        {
            for (int i = 0; i < count; i++)
            {
                Update(ref components1[i]);
            }
        });
    }

    protected abstract void Update(ref T1 component1);
}

internal abstract class GameSystem<T1, T2> : GameSystem
    where T1 : struct
    where T2 : struct
{
    private readonly Query<T1, T2> query;

    protected GameSystem(GameState gameState)
        : base(gameState)
    {
        query = CreateQuery();
    }

    protected virtual Query<T1, T2> CreateQuery()
        => GameState.ECSWorld.Query<T1, T2>().Build();

    protected override void Update()
    {
        query.Run((count, components1, components2) =>
        {
            for (int i = 0; i < count; i++)
            {
                Update(ref components1[i], ref components2[i]);
            }
        });
    }

    protected abstract void Update(ref T1 component1, ref T2 component2);
}

internal abstract class GameSystem<T1, T2, T3> : GameSystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
{
    private readonly Query<T1, T2, T3> query;

    protected GameSystem(GameState gameState)
        : base(gameState)
    {
        query = CreateQuery();
    }

    protected virtual Query<T1, T2, T3> CreateQuery()
        => GameState.ECSWorld.Query<T1, T2, T3>().Build();

    protected override void Update()
    {
        query.Run((count, components1, components2, components3) =>
        {
            for (int i = 0; i < count; i++)
            {
                Update(ref components1[i], ref components2[i], ref components3[i]);
            }
        });
    }

    protected abstract void Update(ref T1 component1, ref T2 component2, ref T3 component3);
}

internal abstract class GameSystem<T1, T2, T3, T4> : GameSystem
    where T1 : struct
    where T2 : struct
    where T3 : struct
    where T4 : struct
{
    private readonly Query<T1, T2, T3, T4> query;

    protected GameSystem(GameState gameState)
        : base(gameState)
    {
        query = CreateQuery();
    }

    protected virtual Query<T1, T2, T3, T4> CreateQuery()
        => GameState.ECSWorld.Query<T1, T2, T3, T4>().Build();

    protected override void Update()
    {
        query.Run((count, components1, components2, components3, components4) =>
        {
            for (int i = 0; i < count; i++)
            {
                Update(ref components1[i], ref components2[i], ref components3[i], ref components4[i]);
            }
        });
    }

    protected abstract void Update(ref T1 component1, ref T2 component2, ref T3 component3, ref T4 components4);
}
using DeliverOrDie.Systems;

using HypEcs;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Game state with main gameplay.
/// </summary>
internal class LevelState : GameState
{
    private LevelFactory factory;

    public TimeToLiveSystem TimeToLiveSystem { get; private set; }

    protected override void Initialize()
    {
        TimeToLiveSystem = new TimeToLiveSystem(this);
        factory = new LevelFactory(this);

        CreateSystems();
        CreateEntities();
    }

    private void CreateSystems()
    {
        UpdateSystems
            .Add(new CameraControlSystem(this))
            .Add(new PlayerControlSystem(this, factory))
            .Add(new MovementSystem(this))
            .Add(new AnimationSystem(this))
            .Add(TimeToLiveSystem)
        ;
        RenderSystems
            .Add(new RenderSystem(this))
        ;
    }

    private void CreateEntities()
    {
        Entity player = factory.CreatePlayer();

        Camera.Target = player;

        Game.SoundManager["AmbientNatureOutside"].PlayLoop();
    }
}

using DeliverOrDie.Systems;
using DeliverOrDie.UI.Elements;

using HypEcs;

using Microsoft.Xna.Framework;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Game state with main gameplay.
/// </summary>
internal class LevelState : GameState
{
    private LevelFactory factory;
    private Entity player;

    public TimeToLiveSystem TimeToLiveSystem { get; private set; }

    protected override void Initialize()
    {
        TimeToLiveSystem = new TimeToLiveSystem(this);
        factory = new LevelFactory(this);

        CreateEntities();
        CreateSystems();
        CreateUI();
    }

    private void CreateSystems()
    {
        UpdateSystems
            .Add(new CameraControlSystem(this))
            .Add(new PlayerControlSystem(this, factory))
            .Add(new ZombieSystem(this, player))
            .Add(new MovementSystem(this))
            .Add(new CollisionSystem(this))
            .Add(new AnimationSystem(this))
            .Add(TimeToLiveSystem)
        ;
        RenderSystems
            .Add(new RenderSystem(this))
        ;
    }

    private void CreateEntities()
    {
        player = factory.CreatePlayer();
        factory.CreateZombie(new Vector2(500.0f, 0.0f));

        Camera.Target = player;

        Game.SoundManager["AmbientNatureOutside"].PlayLoop();
    }

    private void CreateUI()
    {
        UILayer.AddElement(new AmmoCounter()
        {
            Offset = new Vector2(0.0f, Game.Resolution.Y - 20.0f),
            TrackedEntity = player,
        });
    }
}

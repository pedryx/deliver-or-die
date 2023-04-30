using DeliverOrDie.Systems;
using DeliverOrDie.UI;
using DeliverOrDie.UI.Elements;

using HypEcs;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Game state with main gameplay.
/// </summary>
internal class LevelState : GameState
{
    private const float deliveryTime = 2.0f * 60.0f;

    private readonly List<Entity> deliverySpots = new();

    private LevelFactory factory;
    private Entity player;
    private DirectionMarker directionMarker;
    private Timer timer;

    public TimeToLiveSystem TimeToLiveSystem { get; private set; }

    public int QuestDeliverySpotIndex { get; private set; }

    public void CompleteDelivery()
    {
        // TODO: upgrade
        GenerateDeliveryQuest(QuestDeliverySpotIndex);
        timer.Time = deliveryTime;
    }

    protected override void Initialize()
    {
        TimeToLiveSystem = new TimeToLiveSystem(this);
        factory = new LevelFactory(this);

        CreateEntities();
        CreateSystems();
        CreateUI();

        GenerateDeliveryQuest();
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

    private void CreateUI()
    {
        UILayer.AddElement(new AmmoCounter()
        {
            Offset = new Vector2(0.0f, Game.Resolution.Y - 20.0f),
            TrackedEntity = player,
        });

        UILayer.AddElement(new HealthBar()
        {
            Offset = new Vector2(Game.Resolution.X / 2.0f, Game.Resolution.Y - 10.0f),
            TrackedEntity = player,
        });

        directionMarker = new DirectionMarker()
        {
            TrackedEntity = player,
        };
        UILayer.AddElement(directionMarker);

        timer = new Timer()
        {
            Offset = new Vector2(Game.Resolution.X / 2.0f, 10.0f),
            Time = deliveryTime,
        };
        timer.OnFinish += (sender, e) =>
        {
            // TODO: game over
        };
        UILayer.AddElement(timer);
    }

    private void CreateEntities()
    {
        player = factory.CreatePlayer();
        //factory.CreateZombie(new Vector2(500.0f, 0.0f));

        CreateDeliverySpot(new Vector2(-500.0f, 0.0f));
        CreateDeliverySpot(new Vector2(500.0f, 0.0f));

        Camera.Target = player;

        Game.SoundManager["AmbientNatureOutside"].PlayLoop();
    }

    private void GenerateDeliveryQuest(int lastSpotIndex = -1)
    {
        while ((QuestDeliverySpotIndex = Game.Random.Next(deliverySpots.Count)) == lastSpotIndex) ;
        directionMarker.Destination = deliverySpots[QuestDeliverySpotIndex];
    }

    private void CreateDeliverySpot(Vector2 position)
    {
        Entity spot = factory.CreateDeliverySpot(position, deliverySpots.Count);
        deliverySpots.Add(spot);
    }
}

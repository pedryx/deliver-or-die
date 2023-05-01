using DeliverOrDie.GameStates.UpgradeMenu;
using DeliverOrDie.Systems;
using DeliverOrDie.UI;
using DeliverOrDie.UI.Elements;

using HypEcs;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Game state with main gameplay.
/// </summary>
internal class LevelState : GameState
{
    /// <summary>
    /// Time until delivery expires in seconds.
    /// </summary>
    private const float deliveryTime = 2.0f * 60.0f;

    /// <summary>
    /// Contains all delivery spots where delivery quest could occur.
    /// </summary>
    private readonly List<Entity> deliverySpots = new();

    private LevelFactory factory;
    /// <summary>
    /// Marker pointing towards current delivery quest location.
    /// </summary>
    private DirectionMarker directionMarker;
    /// <summary>
    /// Timer which counts time until delivery expires.
    /// </summary>
    private Timer timer;

    /// <summary>
    /// index of current delivery spot quest location.
    /// </summary>
    public int QuestDeliverySpotIndex { get; private set; }
    public Entity Player { get; private set; }

    public void CompleteDelivery()
    {
        Game.GameStatistics.Increment(Statistics.DeliveriesMade, 1.0f);

        Enabled = false;
        var upgradeMenuState = new UpgradeMenuState(this);
        upgradeMenuState.Initialize(Game);
        Game.AddGameState(upgradeMenuState);

        GenerateDeliveryQuest(QuestDeliverySpotIndex);
        timer.Time = deliveryTime;
    }

    public void CreateDeliverySpot(Vector2 position)
    {
        Entity spot = factory.CreateDeliverySpot(position, deliverySpots.Count);
        deliverySpots.Add(spot);
    }

    public void GameOver()
    {
        // TODO: game over
        throw new NotImplementedException();
    }

    protected override void Initialize()
    {
        factory = new LevelFactory(this);

        CreateEntities();
        CreateSystems();
        CreateUI();

        GenerateDeliveryQuest();

        Camera.Target = Player;
        Game.SoundManager["AmbientNatureOutside"].PlayLoop();
    }

    private void CreateSystems()
    {
        UpdateSystems
            .Add(new TimeToLiveSystem(this))
            .Add(new CameraControlSystem(this))
            .Add(new PlayerControlSystem(this, factory))
            .Add(new ZombieSpawningSystem(this, factory))
            .Add(new ZombieSystem(this, Player))
            .Add(new MovementSystem(this))
            .Add(new CollisionSystem(this))
            .Add(new AnimationSystem(this))
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
            TrackedEntity = Player,
        });

        UILayer.AddElement(new HealthBar()
        {
            Offset = new Vector2(Game.Resolution.X / 2.0f, Game.Resolution.Y - 10.0f),
            TrackedEntity = Player,
        });

        directionMarker = new DirectionMarker()
        {
            TrackedEntity = Player,
        };
        UILayer.AddElement(directionMarker);

        timer = new Timer()
        {
            Offset = new Vector2(Game.Resolution.X / 2.0f, 10.0f),
            Time = deliveryTime,
        };
        timer.OnFinish += (sender, e) => GameOver();
        UILayer.AddElement(timer);
    }

    private void CreateEntities()
    {
        Player = factory.CreatePlayer();

        WorldGenerator.Generate(this, factory);
    }

    private void GenerateDeliveryQuest(int lastSpotIndex = -1)
    {
        while ((QuestDeliverySpotIndex = Game.Random.Next(deliverySpots.Count)) == lastSpotIndex) ;
        directionMarker.Destination = deliverySpots[QuestDeliverySpotIndex];
    }
}

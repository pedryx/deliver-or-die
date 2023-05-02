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
    private const float zombieSpeedDifficultyMultiplier = 1.1f;
    private const float zombieDamageDifficultyMultiplier = 1.15f;
    private const float zombieHealthDifficultyUpgrade = 0.75f;
    /// <summary>
    /// Time until delivery expires in seconds.
    /// </summary>
    private const float deliveryTime = 3.0f * 60.0f;

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
    public ZombieSpawningSystem ZombieSpawningSystem { get; private set; }

    public void CompleteDelivery()
    {
        Game.SoundManager["Jingle_Lose_00"].Play(0.5f);
        Game.GameStatistics.Increment(Statistics.DeliveriesMade, 1.0f);

        Enabled = false;
        var upgradeMenuState = new UpgradeMenuState(this);
        upgradeMenuState.Initialize(Game);
        Game.AddGameState(upgradeMenuState);

        GenerateDeliveryQuest(QuestDeliverySpotIndex);
        timer.Time = deliveryTime;

        ZombieSpawningSystem.ZombieSpeed *= zombieSpeedDifficultyMultiplier;
        ZombieSpawningSystem.ZombieDamage *= zombieDamageDifficultyMultiplier;
        ZombieSpawningSystem.ZombieHealth += zombieHealthDifficultyUpgrade;
    }

    public void CreateDeliverySpot(Vector2 position)
    {
        Entity spot = factory.CreateDeliverySpot(position, deliverySpots.Count);
        deliverySpots.Add(spot);
    }

    public void GameOver()
    {
        Enabled = false;
        var gameOverState = new GameOverState();
        gameOverState.Initialize(Game);
        Game.AddGameState(gameOverState);
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
        Game.SoundManager["Iwan Gabovitch - Dark Ambience Loop"].PlayLoop(0.2f);
    }

    private void CreateSystems()
    {
        ZombieSpawningSystem = new ZombieSpawningSystem(this, factory);

        UpdateSystems
            .Add(new TimeToLiveSystem(this))
            .Add(new CameraControlSystem(this))
            .Add(new PlayerControlSystem(this, factory))
            .Add(ZombieSpawningSystem)
            .Add(new ZombieSystem(this, Player))
            .Add(new MovementSystem(this))
            .Add(new CollisionSystem(this, WorldGenerator.WorldSize))
            .Add(new BorderCollisionSystem(this, WorldGenerator.WorldSize))
            .Add(new AnimationSystem(this))
            .Add(new CloudsSystem(this, factory))
        ;
        RenderSystems
            .Add(new RenderSystem(this))
        ;
    }

    private void CreateUI()
    {
        UILayer.AddElement(new BloodOverlay()
        {
            Target = Player,
        });

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
            Offset = new Vector2(Game.Resolution.X / 2.0f, 0.0f),
            Time = deliveryTime,
        };
        timer.OnFinish += (sender, e) => GameOver();
        UILayer.AddElement(timer);
    }

    private void CreateEntities()
    {
        Player = factory.CreatePlayer();
        
        // CreateDeliverySpot(new Vector2(-500.0f, 0.0f));
        // CreateDeliverySpot(new Vector2(500.0f, 0.0f));

        WorldGenerator.Generate(this, factory);
    }

    private void GenerateDeliveryQuest(int lastSpotIndex = -1)
    {
        while ((QuestDeliverySpotIndex = Game.Random.Next(deliverySpots.Count)) == lastSpotIndex) ;
        directionMarker.Destination = deliverySpots[QuestDeliverySpotIndex];
    }
}

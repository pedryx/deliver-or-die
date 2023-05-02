using DeliverOrDie.Components;
using DeliverOrDie.Extensions;
using DeliverOrDie.GameStates.Level;

using HypEcs;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Systems;
/// <summary>
/// Spawns zombies at random positions.
/// </summary>
internal class ZombieSpawningSystem : GameSystem
{
    private const int maxZombies = 1_000;

    private readonly LevelFactory factory;
    private readonly Random random;
    private readonly Entity player;
    /// <summary>
    /// Minimal distance zombie can be spawned near player.
    /// </summary>
    private readonly float minPlayerSpawnDistance;

    /// <summary>
    /// How many seconds elapsed from last zombie spawn.
    /// </summary>
    private float elapsed = 0.0f;
    /// <summary>
    /// How many zombies will be spawned per second.
    /// </summary>
    public float SpawningSpeed = 10.0f;

    /// <summary>
    /// Movement speed of new zombies.
    /// </summary>
    public float ZombieSpeed = 100.0f;
    /// <summary>
    /// Damage of new zombies.
    /// </summary>
    public float ZombieDamage = 1.0f;
    /// <summary>
    /// Health of new zombies.
    /// </summary>
    public float ZombieHealth = 1.0f;

    public int ZombieCount { get; private set; }

    public ZombieSpawningSystem(LevelState levelState, LevelFactory factory)
        : base(levelState)
    {
        this.factory = factory;
        random = levelState.Game.Random;
        player = levelState.Player;

        minPlayerSpawnDistance = levelState.Game.Resolution.X * 2.5f;
    }

    public void ZombieKilled()
    {
        ZombieCount--;
    }

    protected override void Update()
    {
        if (ZombieCount >= maxZombies)
            return;

        elapsed += GameState.Elapsed;

        Transform playerTransform = GameState.ECSWorld.GetComponent<Transform>(player);

        int zombiesToSpawn = (int)(elapsed * SpawningSpeed);
        elapsed -= zombiesToSpawn / SpawningSpeed;
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            Vector2 position;

            do
            {
                position = random.Nextvector2(WorldGenerator.WorldSize) - WorldGenerator.WorldSize / 2.0f;
            }
            while (Vector2.Distance(playerTransform.Position, position) <= minPlayerSpawnDistance);

            factory.CreateZombie(position, ZombieSpeed, ZombieDamage, ZombieHealth);
            ZombieCount++;

            if (ZombieCount >= maxZombies)
                return;
        }
    }
}

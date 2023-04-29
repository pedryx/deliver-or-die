using DeliverOrDie.Components;
using DeliverOrDie.Systems;

using HypEcs;

using Microsoft.Xna.Framework;

using System.Threading;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Factory for creating entities for <see cref="LevelState"/>.
/// </summary>
internal class LevelFactory
{
    private readonly World ecsWorld;
    private readonly TextureManager textures;
    private readonly TimeToLiveSystem timeToLiveSystem;

    public LevelFactory(LevelState levelState)
    {
        ecsWorld = levelState.ECSWorld;
        textures = levelState.Game.TextureManager;
        timeToLiveSystem = levelState.TimeToLiveSystem;
    }

    public Entity CreateSquare(Vector2 position, float size, Color color, float layerDepth = 0.0f)
    {
        Entity square = ecsWorld.Spawn()
            .Add(Transform.Create(position))
            .Add(new Appearance()
            {
                Texture = textures.Square,
                Color = color,
                ScaleOffset = size,
                Origin = new Vector2(0.5f),
                LayerDepth = layerDepth
            })
            .Id();

        return square;
    }

    public Entity CreateBullet(Vector2 position, float direction)
    {
        Entity bullet = ecsWorld.Spawn()
            .Add(Transform.Create(position))
            .Add(new Appearance()
            {
                Texture = textures.Square,
                Color = Color.Black,
                ScaleOffset = 3.0f,
                Origin = new Vector2(0.5f),
            })
            .Add(new Movement()
            {
                Speed = 3000.0f,
                Direction = direction,
            })
            .Id();
        timeToLiveSystem.Add(bullet, 5.0f);

        return bullet;
    }

    public Entity CreatePlayer()
    {
        Entity player = ecsWorld.Spawn()
            .Add(Transform.Create())
            .Add(new Appearance()
            {
                Texture = textures["survivor-idle_rifle_0"],
                ScaleOffset = 0.5f,
                Origin = new Vector2(90, 120),
                Color = Color.White,
            })
            .Add(new Player()
            {
                MoveSpeed = 500.0f,
                ReloadTime = 2.0f,
                ShootingSpeed = 5.0f,
                Ammo = 5,
                MaxAmmo = 5,
            })
            .Add(new Animation()
            {
                TimePerFrame = 0.06f,
                Frames = Animations.Player.Idle,
            })
            .Id();

        return player;
    }
}

using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Factory for creating entities for <see cref="LevelState"/>.
/// </summary>
internal class LevelFactory
{
    private readonly World ecsWorld;
    private readonly TextureManager textures;

    public LevelFactory(GameState gameState)
    {
        ecsWorld = gameState.ECSWorld;
        textures = gameState.Game.TextureManager;
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

    public Entity CreatePlayer()
    {
        Entity player = ecsWorld.Spawn()
            .Add(Transform.Create())
            .Add(new Appearance()
            {
                Texture = textures["survivor-idle_rifle_0"],
                ScaleOffset = 0.7f,
                Origin = new Vector2(90, 120),
                Color = Color.White,
            })
            .Add(new PlayerControl()
            {
                Speed = 500.0f,
            })
            .Add(new Animation()
            {
                TimePerFrame = 0.08f,
                Frames = Animations.Player.Idle,
            })
            .Id();

        return player;
    }
}

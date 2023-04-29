using DeliverOrDie.Systems;

using HypEcs;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Game state with main gameplay.
/// </summary>
internal class LevelState : GameState
{
    private LevelFactory factory;

    protected override void Initialize()
    {
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

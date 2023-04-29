using DeliverOrDie.Systems;

using HypEcs;

using Microsoft.Xna.Framework;

namespace DeliverOrDie.GameStates.Level;
internal class LevelState : GameState
{
    protected override void Initialize()
    {
        CreateSystems();
        CreateEntities();
    }

    private void CreateSystems()
    {
        UpdateSystems
            .Add(new CameraControlSystem(this))
            .Add(new PlayerControlSystem(this))
            .Add(new MovementSystem(this))
        ;
        RenderSystems
            .Add(new RenderSystem(this))
        ;
    }

    private void CreateEntities()
    {
        LevelFactory factory = new(this);

        Entity player = factory.CreatePlayer();

        Camera.Target = player;
    }
}

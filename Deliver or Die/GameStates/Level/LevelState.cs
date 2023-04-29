using DeliverOrDie.Systems;

using HypEcs;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Game state with main gameplay.
/// </summary>
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
            .Add(new AnimationSystem(this))
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

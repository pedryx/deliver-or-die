using DeliverOrDie.Components;

namespace DeliverOrDie.Systems;
/// <summary>
/// Handles animations.
/// </summary>
internal class AnimationSystem : GameSystem<Appearance, Animation>
{
    public AnimationSystem(GameState gameState)
        : base(gameState) { }

    protected override void Update(ref Appearance appearance, ref Animation animation)
    {
        animation.Elapsed += GameState.Elapsed * GameState.Game.Speed;
        if (animation.Elapsed >= animation.TimePerFrame)
        {
            animation.Elapsed -= animation.TimePerFrame;
            
            appearance.Texture = animation.Frames[animation.FrameIndex].Texture;
            appearance.SourceRectangle = animation.Frames[animation.FrameIndex].SourceRectangle;

            animation.FrameIndex++;
            if (animation.FrameIndex >= animation.Frames.Count)
                animation.FrameIndex = 0;
        }
    }
}

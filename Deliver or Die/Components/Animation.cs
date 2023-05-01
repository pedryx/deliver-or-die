using System.Collections.Generic;
using DeliverOrDie.Resources;

namespace DeliverOrDie.Components;
/// <summary>
/// Animation component.
/// </summary>
internal struct Animation
{
    public List<AnimationFrame> Frames;
    /// <summary>
    /// Time elapsed from last frame switch in seconds.
    /// </summary>
    public float Elapsed;
    /// <summary>
    /// Time spend at one frame in seconds.
    /// </summary>
    public float TimePerFrame;
    /// <summary>
    /// index of current frame.
    /// </summary>
    public int FrameIndex;

    public Animation()
    {
        Frames = new List<AnimationFrame>();
    }

    public Animation(List<AnimationFrame> frames, float timePerFrame = 0.1f)
    {
        Frames = frames;
        TimePerFrame = timePerFrame;
    }
}

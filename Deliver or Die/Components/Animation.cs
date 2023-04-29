using System.Collections.Generic;

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
}

using System.Collections.Generic;
using DeliverOrDie.Resources;

namespace DeliverOrDie.Animation;
/// <summary>
/// Utility class used for creating animations.
/// </summary>
internal static class AnimationFactory
{
    /// <summary>
    /// Create simple animation composed of multiple textures with the same source rectangle.
    /// </summary>
    /// <param name="images">List of names of textures.</param>
    public static List<AnimationFrame> CreateImagesAnimation(List<string> images, TextureManager textures)
    {
        var frames = new List<AnimationFrame>();
        foreach (var image in images)
        {
            frames.Add(new AnimationFrame()
            {
                Texture = textures[image],
            });
        }

        return frames;
    }
}

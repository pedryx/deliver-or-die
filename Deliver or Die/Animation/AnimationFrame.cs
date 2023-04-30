using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.Animation;
/// <summary>
/// Represent one frame of an animation.
/// </summary>
internal struct AnimationFrame
{
    public Texture2D Texture;
    public Rectangle? SourceRectangle;
}

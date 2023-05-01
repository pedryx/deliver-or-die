using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.Extensions;
/// <summary>
/// Contains extension methods for <see cref="Texture2D"/> class.
/// </summary>
internal static class Texture2DExtension
{
    public static Vector2 GetSize(this Texture2D texture)
        => new(texture.Width, texture.Height);
}

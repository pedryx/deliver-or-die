using Microsoft.Xna.Framework.Graphics;

using SpriteFontPlus;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeliverOrDie;
/// <summary>
/// Manages fonts, fonts are lazy loaded (on first access).
/// </summary>
internal class FontManager
{
    private const int bitmapSize = 1024;
    /// <summary>
    /// Root folder where fonts are stored.
    /// </summary>
    private const string contentFolder = "Content/Fonts";

    private readonly Dictionary<string, SpriteFont> fonts = new();
    private readonly GraphicsDevice graphicsDevice;

    public FontManager(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    public SpriteFont this[string key]
    {
        get
        {
            if (fonts.TryGetValue(key, out SpriteFont value))
                return value;
            else
            {
                string file = Directory.GetFiles(contentFolder, $"{key.Split(';').First()}.*", SearchOption.AllDirectories).First();

                int size = int.Parse(key.Split(';').Last());
                SpriteFont font = TtfFontBaker.Bake
                (
                    File.ReadAllBytes(file),
                    size,
                    bitmapSize,
                    bitmapSize,
                    new[] { CharacterRange.BasicLatin }
                ).CreateSpriteFont(graphicsDevice);

                fonts.Add(key, font);
                return font;
            }
        }
    }
}

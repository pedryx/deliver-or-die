using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeliverOrDie.Resources;
/// <summary>
/// Manages textures, textures are lazy loaded (on first access).
/// </summary>
public class TextureManager
{
    /// <summary>
    /// Root folder where textures are stored.
    /// </summary>
    private const string contentFolder = "Content/Textures";

    private readonly Dictionary<string, Texture2D> textures = new();
    private readonly GraphicsDevice graphicsDevice;

    public Texture2D Square { get; private set; }

    public TextureManager(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;

        Square = new Texture2D(graphicsDevice, 1, 1);
        Square.SetData(new Color[] { Color.White });
    }

    public Texture2D this[string key]
    {
        get
        {
            if (textures.TryGetValue(key, out Texture2D value))
                return value;
            else
            {
                string file = Directory.GetFiles
                (
                    contentFolder,
                    $"{key}.*",
                    SearchOption.AllDirectories
                ).First();

                string name = file.Split('/', '\\').Last().Split('.').First();
                Texture2D texture = Texture2D.FromFile(graphicsDevice, file);

                textures.Add(name, texture);
                return texture;
            }
        }
    }
}

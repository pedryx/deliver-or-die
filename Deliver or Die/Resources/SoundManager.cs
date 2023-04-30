using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeliverOrDie.Resources;
/// <summary>
/// Manages sounds, Sounds are lazy loaded (on first access).
/// </summary>
internal class SoundManager
{
    /// <summary>
    /// Root folder where sounds are stored.
    /// </summary>
    private const string contentFolder = "Content/Sounds";

    private readonly Dictionary<string, Sound> soundEffects = new();

    public Sound this[string key]
    {
        get
        {
            if (soundEffects.TryGetValue(key, out Sound value))
                return value;
            else
            {
                string file = Directory.GetFiles(contentFolder, $"{key}.*", SearchOption.AllDirectories).First();

                string name = file.Split('/', '\\').Last().Split('.').First();
                Sound sound = Sound.FromFile(file);

                soundEffects.Add(name, sound);
                return sound;
            }
        }
    }
}

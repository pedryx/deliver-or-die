using Microsoft.Xna.Framework.Audio;

using System;

namespace DeliverOrDie;
/// <summary>
/// Wrapper around <see cref="SoundEffect"/> class with option to wait for end of effect.
/// </summary>
internal class Sound
{
    private SoundEffect effect;
    /// <summary>
    /// Time stamp when sound has been lastly played.
    /// </summary>
    private DateTime lastPlayed = DateTime.Now;

    private Sound() { }

    public TimeSpan Duration => effect.Duration;

    /// <summary>
    /// Play sound.
    /// </summary>
    /// <param name="wait">if true sound will only be played if it is not currently playing.</param>
    public void Play(float volume = 1.0f, bool wait = false, float waitTime = 0.0f)
    {
        if (!wait)
        {
            effect.Play(volume, 0.0f, 0.0f);
            lastPlayed = DateTime.Now;
        }
        else
        {
            TimeSpan elapsed = DateTime.Now - lastPlayed;
            if (elapsed > effect.Duration + TimeSpan.FromSeconds(waitTime))
            {
                effect.Play(volume, 0.0f, 0.0f);
                lastPlayed = DateTime.Now;
            }
        }
    }

    public static Sound FromFile(string path)
    {
        var sound = new Sound();
        sound.effect = SoundEffect.FromFile(path);

        return sound;
    }
}

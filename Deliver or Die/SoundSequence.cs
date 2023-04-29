using System;
using System.Collections.Generic;

namespace DeliverOrDie;
/// <summary>
/// Represent sequence with sounds.
/// </summary>
internal class SoundSequence
{
    private readonly List<Sound> sounds = new();
    /// <summary>
    /// How many seconds to pause between sounds.
    /// </summary>
    private readonly float pause;

    /// <summary>
    /// Determine if this is first play of the sound.
    /// </summary>
    private bool first = true;
    /// <summary>
    /// Timestamp of last play of the sound.
    /// </summary>
    private DateTime lastPlayed;
    /// <summary>
    /// Index of current sound.
    /// </summary>
    private int current = 0;

    /// <param name="pause">How many seconds to pause between sounds.</param>
    public SoundSequence(SoundManager manager, List<string> soundNames, float pause)
    {
        this.pause = pause;

        foreach (var soundName in soundNames)
        {
            sounds.Add(manager[soundName]);
        }
    }

    /// <summary>
    /// Continue playing sequence.
    /// </summary>
    public void Play(float volume = 1.0f)
    {
        if (first)
        {
            first = false;
            lastPlayed = DateTime.Now;
            sounds[current].Play(volume);
        }

        if (DateTime.Now - lastPlayed > sounds[current].Duration + TimeSpan.FromSeconds(pause))
        {
            current++;
            if (current == sounds.Count)
                current = 0;

            sounds[current].Play(volume);
            lastPlayed = DateTime.Now;
        }
    }
}

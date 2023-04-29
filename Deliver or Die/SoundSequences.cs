using System.Collections.Generic;

namespace DeliverOrDie;
/// <summary>
/// Contains sound sequences.
/// </summary>
internal class SoundSequences
{
    private static SoundManager sounds;

    /// <summary>
    /// Asociate this class with instance of sound manager.
    /// </summary>
    public static void Initialize(SoundManager sounds)
    {
        SoundSequences.sounds = sounds;
    }

    public static class Player
    {
        public static SoundSequence Walk { get; private set; }
            = new SoundSequence(sounds, new List<string>()
            {
                "Footstep_Dirt_01",
                "Footstep_Dirt_02",
                "Footstep_Dirt_03",
                "Footstep_Dirt_04",
                "Footstep_Dirt_05",
                "Footstep_Dirt_06",
                "Footstep_Dirt_07",
                "Footstep_Dirt_08",
                "Footstep_Dirt_09",
            }, 0.2f);
    }
}

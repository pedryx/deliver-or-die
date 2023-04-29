using System.Collections.Generic;

namespace DeliverOrDie;
/// <summary>
/// Contains animations for entities.
/// </summary>
internal static class Animations
{
    private static TextureManager textures;

    /// <summary>
    /// Asociate this class with instance of texture manager.
    /// </summary>
    /// <param name="textures"></param>
    public static void Initialize(TextureManager textures)
    {
        Animations.textures = textures;
    }

    public static class Player
    {
        public static List<AnimationFrame> Idle { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "survivor-idle_rifle_0",
                "survivor-idle_rifle_1",
                "survivor-idle_rifle_2",
                "survivor-idle_rifle_3",
                "survivor-idle_rifle_4",
                "survivor-idle_rifle_5",
                "survivor-idle_rifle_6",
                "survivor-idle_rifle_7",
                "survivor-idle_rifle_8",
                "survivor-idle_rifle_9",
                "survivor-idle_rifle_10",
                "survivor-idle_rifle_11",
                "survivor-idle_rifle_12",
                "survivor-idle_rifle_13",
                "survivor-idle_rifle_14",
                "survivor-idle_rifle_15",
                "survivor-idle_rifle_16",
                "survivor-idle_rifle_17",
                "survivor-idle_rifle_18",
                "survivor-idle_rifle_19",
            }, textures);
    }
}

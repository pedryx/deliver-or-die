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

        public static List<AnimationFrame> Move { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "survivor-move_rifle_0",
                "survivor-move_rifle_1",
                "survivor-move_rifle_2",
                "survivor-move_rifle_3",
                "survivor-move_rifle_4",
                "survivor-move_rifle_5",
                "survivor-move_rifle_6",
                "survivor-move_rifle_7",
                "survivor-move_rifle_8",
                "survivor-move_rifle_9",
                "survivor-move_rifle_10",
                "survivor-move_rifle_11",
                "survivor-move_rifle_12",
                "survivor-move_rifle_13",
                "survivor-move_rifle_14",
                "survivor-move_rifle_15",
                "survivor-move_rifle_16",
                "survivor-move_rifle_17",
                "survivor-move_rifle_18",
                "survivor-move_rifle_19",
            }, textures);

        public static List<AnimationFrame> Reload { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "survivor-reload_rifle_0",
                "survivor-reload_rifle_1",
                "survivor-reload_rifle_2",
                "survivor-reload_rifle_3",
                "survivor-reload_rifle_4",
                "survivor-reload_rifle_5",
                "survivor-reload_rifle_6",
                "survivor-reload_rifle_7",
                "survivor-reload_rifle_8",
                "survivor-reload_rifle_9",
                "survivor-reload_rifle_10",
                "survivor-reload_rifle_11",
                "survivor-reload_rifle_12",
                "survivor-reload_rifle_13",
                "survivor-reload_rifle_14",
                "survivor-reload_rifle_15",
                "survivor-reload_rifle_16",
                "survivor-reload_rifle_17",
                "survivor-reload_rifle_18",
                "survivor-reload_rifle_19",
            }, textures);

        public static List<AnimationFrame> Shoot { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "survivor-shoot_rifle_0",
                "survivor-shoot_rifle_1",
                "survivor-shoot_rifle_2",
            }, textures);
    }
}

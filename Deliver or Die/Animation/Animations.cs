using System.Collections.Generic;
using DeliverOrDie.Resources;

namespace DeliverOrDie.Animation;
/// <summary>
/// Contains animations for entities.
/// </summary>
internal static class Animations
{
    private static TextureManager textures;

    /// <summary>
    /// Associate this class with instance of texture manager.
    /// </summary>
    /// <param name="textures"></param>
    public static void Initialize(TextureManager textures)
    {
        Animations.textures = textures;
    }

    public static class Zombie
    {
        public static List<AnimationFrame> Idle { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "skeleton-idle_0",
                "skeleton-idle_1",
                "skeleton-idle_2",
                "skeleton-idle_3",
                "skeleton-idle_4",
                "skeleton-idle_5",
                "skeleton-idle_6",
                "skeleton-idle_7",
                "skeleton-idle_8",
                "skeleton-idle_9",
                "skeleton-idle_10",
                "skeleton-idle_11",
                "skeleton-idle_12",
                "skeleton-idle_13",
                "skeleton-idle_14",
                "skeleton-idle_15",
                "skeleton-idle_16",
            }, textures);

        public static List<AnimationFrame> Move { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "skeleton-move_0",
                "skeleton-move_1",
                "skeleton-move_2",
                "skeleton-move_3",
                "skeleton-move_4",
                "skeleton-move_5",
                "skeleton-move_6",
                "skeleton-move_7",
                "skeleton-move_8",
                "skeleton-move_9",
                "skeleton-move_10",
                "skeleton-move_11",
                "skeleton-move_12",
                "skeleton-move_13",
                "skeleton-move_14",
                "skeleton-move_15",
                "skeleton-move_16",
            }, textures);

        public static List<AnimationFrame> Attack { get; private set; }
            = AnimationFactory.CreateImagesAnimation(new List<string>()
            {
                "skeleton-attack_0",
                "skeleton-attack_1",
                "skeleton-attack_2",
                "skeleton-attack_3",
                "skeleton-attack_4",
                "skeleton-attack_5",
                "skeleton-attack_6",
                "skeleton-attack_7",
                "skeleton-attack_8",
            }, textures);
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

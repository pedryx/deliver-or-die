using DeliverOrDie.Extensions;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Generates environment.
/// </summary>
internal class WorldGenerator
{
    /// <summary>
    /// Minimal distance between delivery spots.
    /// </summary>
    private const float minDeliverySpotsDistance = 5_000.0f;
    private const int deliverySpotCount = 20;

    // TODO: border
    public static readonly Vector2 WorldSize = new(30_000.0f);

    private static LevelState state;
    private static LevelFactory factory;

    public static void Generate(LevelState state, LevelFactory factory)
    {
        WorldGenerator.state = state;
        WorldGenerator.factory = factory;

        GenerateDeliverySpots();

        WorldGenerator.state = null;
        WorldGenerator.factory = null;
    }

    private static void GenerateDeliverySpots()
    {
        var positions = new List<Vector2>();

        for (int i = 0; i < deliverySpotCount; i++)
        {
            Vector2 position;

            do
            {
                position = state.Game.Random.Nextvector2(WorldSize) - WorldSize / 2.0f;
            }
            while
            (
                positions
                    .Select(p => Vector2.Distance(position, p))
                    .Where(d => d <= minDeliverySpotsDistance)
                    .Any()
            );

            positions.Add(position);
            state.CreateDeliverySpot(position);
        }
    }
}

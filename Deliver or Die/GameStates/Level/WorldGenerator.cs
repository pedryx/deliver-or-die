using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace DeliverOrDie.GameStates.Level;
internal class WorldGenerator
{
    private const float minDeliverySpotsDistance = 5_000.0f;

    // TODO: border
    public static readonly Vector2 WorldSize = new(30_000.0f);

    private static int deliverySpotCount = 20;

    public static void Generate(LevelState state, LevelFactory factory)
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

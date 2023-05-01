using System;
using System.Collections.Generic;

namespace DeliverOrDie;
/// <summary>
/// Contains statistics about game.
/// </summary>
internal class GameStatistics
{
    private readonly Dictionary<Statistics, float> statistics = new();

    public GameStatistics()
    {
        Clear();
    }

    public void Clear()
    {
        foreach (Statistics stat in Enum.GetValues(typeof(Statistics)))
            statistics[stat] = 0.0f;
    }

    /// <summary>
    /// Increment statistic.
    /// </summary>
    /// <param name="stat">Statistic to increment.</param>
    /// <param name="value">by which amount to increment.</param>
    public void Increment(Statistics stat, float value)
        => statistics[stat] += value;

    public float this[Statistics stat] => statistics[stat];
}

internal enum Statistics
{
    DeliveriesMade,
    ZombiesKilled,
    PlayTime,
}
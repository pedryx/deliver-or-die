using System.Collections.Generic;

namespace DeliverOrDie;
internal class Statistics
{
    private readonly Dictionary<string, float> statistics = new()
    {
        { "deliveries made", 0.0f },
        { "zombie killed", 0.0f },
        { "play time", 0.0f }
    };

    public void Increment(string name, float value)
        => statistics[name] += value;
}

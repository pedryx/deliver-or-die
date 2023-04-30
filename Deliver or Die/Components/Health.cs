using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Components;
internal struct Health
{
    public float Current;
    public float Max;
    public int EntityIndex;

    public Action<Vector2> OnDead;
}

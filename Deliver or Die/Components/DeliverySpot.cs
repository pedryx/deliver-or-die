using DeliverOrDie.GameStates.Level;

namespace DeliverOrDie.Components;
internal struct DeliverySpot
{
    /// <summary>
    /// Index of delivery spot in <see cref="LevelState"/>.
    /// </summary>
    public int Index;

    public DeliverySpot(int index)
    {
        Index = index;
    }
}

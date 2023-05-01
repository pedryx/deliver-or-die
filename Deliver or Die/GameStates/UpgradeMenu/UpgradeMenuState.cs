using DeliverOrDie.GameStates.Level;
using DeliverOrDie.UI;
using DeliverOrDie.UI.Elements;

namespace DeliverOrDie.GameStates.UpgradeMenu;
internal class UpgradeMenuState : GameState
{
    private readonly LevelState levelState;

    public UpgradeMenuState(LevelState levelState)
    {
        this.levelState = levelState;
    }

    protected override void Initialize()
    {
        var upgradeMenuWindow = new UpgradeMenuWindow(levelState)
        {
            Offset = Game.Resolution / 2.0f,
        };
        UILayer.AddElement(upgradeMenuWindow);
    }

    public void Close()
    {
        Game.RemoveGameState(this);
        levelState.Enabled = true;
    }
}

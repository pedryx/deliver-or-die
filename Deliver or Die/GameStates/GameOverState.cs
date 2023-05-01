using DeliverOrDie.UI;
using DeliverOrDie.UI.Elements;

namespace DeliverOrDie.GameStates;
internal class GameOverState : GameState
{
    protected override void Initialize()
    {
        UILayer.AddElement(new GameOverWindow()
        {
            Offset = Game.Resolution / 2.0f,
        });
    }
}

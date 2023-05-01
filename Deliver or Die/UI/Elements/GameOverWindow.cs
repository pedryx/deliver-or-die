using DeliverOrDie.GameStates.Level;

using Microsoft.Xna.Framework;

using System;
using System.Text;

namespace DeliverOrDie.UI.Elements;
internal class GameOverWindow : UIElement
{
    private const string title = "Game Over";

    private Vector2 size;

    public override Vector2 Size => size;

    protected override void Initialize()
    {
        var image = new Image(Owner.GameState.Game.TextureManager["PauseMenu"]);
        size = image.Size;
        AddChild(image);

        AddChild(new Label(Owner.GameState.Game.FontManager["Comic Sans;85"], title)
        {
            Color = Color.White,
            Offset = new Vector2(0.0f, -260.0f),
        });

        SpawnLabels();

        var button = new Button()
        {
            Offset = new Vector2(0.0f, 100.0f),
        };
        button.Label.Font = Owner.GameState.Game.FontManager["Comic Sans;75"];
        button.Label.Text = "Restart";
        button.Label.Color = Color.White;
        button.HoverColor = Color.Gold;
        button.Label.Origin = button.Label.Font.MeasureString(button.Label.Text) / 2.0f;
        AddChild(button);

        button.OnClick += (sender, e) =>
        {
            foreach (var state in Owner.GameState.Game.ActiveStates)
                Owner.GameState.Game.RemoveGameState(state);

            var levelState = new LevelState();
            levelState.Initialize(Owner.GameState.Game);
            Owner.GameState.Game.AddGameState(levelState);

            Owner.GameState.Game.GameStatistics.Clear();
        };
    }

    private void SpawnLabels()
    {
        var values = Enum.GetValues<Statistics>();
        for (int i = 0; i < values.Length; i++)
        {
            string name = Enum.GetName(values[i]);
            var builder = new StringBuilder();

            builder.Append(char.ToLower(name[0]));

            for (int j = 1; j < name.Length; j++)
            {
                if (char.IsUpper(name[j]))
                {
                    builder.Append(' ');
                    builder.Append(char.ToLower(name[j]));
                }
                else
                    builder.Append(name[j]);
            }

            builder.Append(":  ");

            if (values[i] == Statistics.PlayTime)
            {
                int playTime = (int)Owner.GameState.Game.GameStatistics[values[i]];
                builder.Append($"{playTime / 60,2}:{(playTime % 60).ToString().PadLeft(2, '0')}");
            }
            else
                builder.Append((int)Owner.GameState.Game.GameStatistics[values[i]]);

            AddChild(new Label(Owner.GameState.Game.FontManager["Comic Sans;70"], builder.ToString())
            {
                Color = Color.White,
                Offset = new Vector2()
                {
                    Y = Size.Y / (Enum.GetValues(typeof(Statistics)).Length + 4) * (i + 1) - Size.Y / 2.0f,
                },
            });
        }
    }
}

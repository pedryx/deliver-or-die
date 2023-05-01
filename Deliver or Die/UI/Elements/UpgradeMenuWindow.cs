using DeliverOrDie.Components;
using DeliverOrDie.GameStates;
using DeliverOrDie.GameStates.Level;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
internal class UpgradeMenuWindow : UIElement
{
    private const float damageUpgrade = 1.0f;
    private const float reloadSpeedUpgradeMultiplier = 0.9f;
    private const int maxAmmoUpgrade = 5;
    private const float moveSpeedUpgradeMultiplier = 1.1f;
    private const float maxHPUpgrade = 2.0f;
    private const float heal = 0.33f;

    private const string title = "Pick an upgrade:";
    private const int rowSize = 6;
    private const int columnSize = 2;

    private readonly LevelState levelState;

    private Vector2 size;

    public override Vector2 Size => size;

    public UpgradeMenuWindow(LevelState levelState)
    {
        this.levelState = levelState;
    }

    protected override void Initialize()
    {
        Texture2D windowTexture = Owner.GameState.Game.TextureManager["PauseMenu"];
        var image = new Image()
        {
            Texture = windowTexture,
            Origin = new Vector2(windowTexture.Width, windowTexture.Height) / 2.0f,
        };
        size = new Vector2(windowTexture.Width, windowTexture.Height) * image.Scale;
        AddChild(image);

        SpriteFont font = Owner.GameState.Game.FontManager["Comic Sans;100"];
        AddChild(new Label()
        {
            Font = font,
            Text = title,
            Color = Color.White,
            Origin = font.MeasureString(title) / 2.0f,
            Offset = new Vector2(0, -280.0f),
        });

        var damageButton = CreateButton("Triple_headshot", "damage", 0, 0);
        var reloadTimeButton = CreateButton("Magnum_Master", "reload speed", 1, 0);
        var maxAmmoButton = CreateButton("Dirty_Harry", "max ammo", 2, 0);
        var moveSpeedButton = CreateButton("Ninja", "move speed", 0, 1);
        var maxHPButton = CreateButton("Survivor_Pro", "max HP", 1, 1);
        var healButton = CreateButton("Big_Man", "heal", 2, 1);

        damageButton.OnClick += DamageButton_OnClick;
        reloadTimeButton.OnClick += ReloadTimeButton_OnClick;
        maxAmmoButton.OnClick += MaxAmmoButton_OnClick;
        moveSpeedButton.OnClick += MoveSpeedButton_OnClick;
        maxHPButton.OnClick += MaxHPButton_OnClick;
        healButton.OnClick += HealButton_OnClick;
    }

    private void DamageButton_OnClick(object sender, System.EventArgs e)
    {
        levelState.ECSWorld.GetComponent<Player>(levelState.Player).Damage += damageUpgrade;
        (Owner.GameState as UpgradeMenuState).Close();
    }

    private void ReloadTimeButton_OnClick(object sender, System.EventArgs e)
    {
        levelState.ECSWorld.GetComponent<Player>(levelState.Player).ReloadTime *= reloadSpeedUpgradeMultiplier;
        (Owner.GameState as UpgradeMenuState).Close();
    }

    private void MaxAmmoButton_OnClick(object sender, System.EventArgs e)
    {
        levelState.ECSWorld.GetComponent<Player>(levelState.Player).MaxAmmo += maxAmmoUpgrade;
        (Owner.GameState as UpgradeMenuState).Close();
    }

    private void MoveSpeedButton_OnClick(object sender, System.EventArgs e)
    {
        ref Player player = ref levelState.ECSWorld.GetComponent<Player>(levelState.Player);
        ref Movement movement = ref levelState.ECSWorld.GetComponent<Movement>(levelState.Player);

        player.MoveSpeed *= moveSpeedUpgradeMultiplier;
        if (movement.Speed != 0.0f)
            movement.Speed = player.MoveSpeed;

        (Owner.GameState as UpgradeMenuState).Close();
    }

    private void MaxHPButton_OnClick(object sender, System.EventArgs e)
    {
        ref Health health = ref levelState.ECSWorld.GetComponent<Health>(levelState.Player);

        health.Max += maxHPUpgrade;
        health.Current += maxHPUpgrade;

        if (health.Current > health.Max)
            health.Current = health.Max;

        (Owner.GameState as UpgradeMenuState).Close();
    }

    private void HealButton_OnClick(object sender, System.EventArgs e)
    {
        ref Health health = ref levelState.ECSWorld.GetComponent<Health>(levelState.Player);

        health.Current += health.Max * heal;

        if (health.Current > health.Max)
            health.Current = health.Max;

        (Owner.GameState as UpgradeMenuState).Close();
    }

    private Button CreateButton(string textureName, string description, int x, int y)
    {
        Texture2D texture = Owner.GameState.Game.TextureManager[textureName];
        SpriteFont font = Owner.GameState.Game.FontManager["Comic Sans;32"];

        var button = new Button();
        button.Image.Texture = texture;
        button.Image.Origin = new Vector2(texture.Width, texture.Height) / 2.0f;
        button.Image.Scale = 0.2f;
        button.Offset = new Vector2()
        {
            X = (Size.X / (rowSize + 2)) * (x + 1) * 2.0f,
            Y = (Size.Y / (columnSize + 2) + 100.0f) * (y + 1) - 120.0f,
        } - Size / 2.0f;

        AddChild(new Label()
        {
            Font = font,
            Origin = font.MeasureString(description) / 2.0f,
            Offset = button.Offset + new Vector2(0.0f, 70.0f),
            Color = Color.White,
            Text = description,
        });
        AddChild(button);

        return button;
    }
}

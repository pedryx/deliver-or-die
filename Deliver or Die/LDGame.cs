using DeliverOrDie.GameStates.Level;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie;
/// <summary>
/// Main game class.
/// </summary>
internal class LDGame : Game
{
    /// <summary>
    /// Color used for clear graphics buffer.
    /// </summary>
    private readonly Color clearColor = new(0, 82, 22);

    /// <summary>
    /// How fast is the game running, should not affect performance.
    /// </summary>
    public float Speed = 1.0f;
    /// <summary>
    /// Currently active state.
    /// </summary>
    public GameState ActiveState { get; private set; } = new LevelState();
    public GraphicsDeviceManager Graphics { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public TextureManager TextureManager { get; private set; }
    public SoundManager SoundManager { get; private set; } = new();
    public FontManager FontManager { get; private set; }

    /// <summary>
    /// Width and height of game window.
    /// </summary>
    public Vector2 Resolution => new(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

    public LDGame()
    {
        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1280,
            PreferredBackBufferHeight = 720,
        };
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        TextureManager = new TextureManager(GraphicsDevice);
        FontManager = new FontManager(GraphicsDevice);
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Animations.Initialize(TextureManager);
        SoundSequences.Initialize(SoundManager);
        ActiveState.Initialize(this);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        ActiveState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(clearColor);
        ActiveState.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);

        base.Draw(gameTime);
    }
}

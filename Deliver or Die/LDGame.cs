using DeliverOrDie.GameStates.Level;
using DeliverOrDie.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace DeliverOrDie;
/// <summary>
/// Main game class.
/// </summary>
internal class LDGame : Game
{
    private const int randomGeneratorSeed = 0;

    private readonly List<GameState> activeStates = new() { new LevelState() };
    private readonly List<GameState> gameStatesToAdd = new();
    private readonly List<GameState> gameStatesToRemove = new();

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
    public IReadOnlyList<GameState> ActiveStates => activeStates;
    public GraphicsDeviceManager Graphics { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public TextureManager TextureManager { get; private set; }
    public SoundManager SoundManager { get; private set; } = new();
    public FontManager FontManager { get; private set; }
    public Random Random { get; private set; } = new(randomGeneratorSeed);
    public GameStatistics GameStatistics { get; private set; } = new();

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

    public void AddGameState(GameState state)
        => gameStatesToAdd.Add(state);

    public void RemoveGameState(GameState state)
        => gameStatesToRemove.Add(state);

    protected override void LoadContent()
    {
        TextureManager = new TextureManager(GraphicsDevice);
        FontManager = new FontManager(GraphicsDevice);
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Animations.Initialize(TextureManager);
        SoundSequences.Initialize(SoundManager);
        activeStates.ForEach(state => state.Initialize(this));

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        activeStates.ForEach(state => state.Update(elapsed));

        gameStatesToAdd.ForEach(activeStates.Add);
        gameStatesToRemove.ForEach(state => activeStates.Remove(state));
        gameStatesToAdd.Clear();
        gameStatesToRemove.Clear();

        GameStatistics.Increment(Statistics.PlayTime, elapsed);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(clearColor);
        activeStates.ForEach(state => state.Draw((float)gameTime.ElapsedGameTime.TotalSeconds));

        base.Draw(gameTime);
    }
}

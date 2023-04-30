﻿using DeliverOrDie.GameStates.Level;

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
    public List<GameState> ActiveStates { get; private set; } = new() { new LevelState() };
    public GraphicsDeviceManager Graphics { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public TextureManager TextureManager { get; private set; }
    public SoundManager SoundManager { get; private set; } = new();
    public FontManager FontManager { get; private set; }
    public Random Random { get; private set; } = new();

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
        ActiveStates.ForEach(state => state.Initialize(this));

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        ActiveStates.ForEach(state => state.Update((float)gameTime.ElapsedGameTime.TotalSeconds));

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(clearColor);
        ActiveStates.ForEach(state => state.Draw((float)gameTime.ElapsedGameTime.TotalSeconds));

        base.Draw(gameTime);
    }
}

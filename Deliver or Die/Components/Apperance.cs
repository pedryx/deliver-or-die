using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.Components;
/// <summary>
/// Appearance component.
/// </summary>
public struct Appearance
{
    public Texture2D Texture;
    /// <summary>
    /// Offset to the transform component's position.
    /// </summary>
    public Vector2 PositionOffset;
    /// <summary>
    /// Source rectangle which represent region of texture which will be rendered.
    /// </summary>
    public Rectangle? SourceRectangle;
    /// <summary>
    /// Color modification for the texture.
    /// </summary>
    public Color Color;
    /// <summary>
    /// Offset to the transform component's rotation.
    /// </summary>
    public float RotationOffset;
    /// <summary>
    /// Origin point for the rotation, scaling and rendering.
    /// </summary>
    public Vector2 Origin;
    /// <summary>
    /// Offset to the transform component's scale.
    /// </summary>
    public float ScaleOffset;
    public SpriteEffects SpriteEffects;
    public float LayerDepth;

    public static Appearance Create
    (
        Texture2D texture = null,
        float scale = 1.0f,
        Rectangle? sourceRectangle = null
    )
        => new Appearance()
        {
            Texture = texture,
            Color = Color.White,
            Origin = sourceRectangle.HasValue
                ? sourceRectangle.Value.Location.ToVector2() + sourceRectangle.Value.Size.ToVector2() / 2
                : new Vector2(texture.Width / 2, texture.Height / 2),
            ScaleOffset = scale,
        };
}

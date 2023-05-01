using DeliverOrDie.Extensions;
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

    public Appearance()
        : this(1.0f) { }

    public Appearance(float scale)
    {
        ScaleOffset = scale;
        Color = Color.White;
    }

    public Appearance(Texture2D texture, float scale = 1.0f)
        : this(scale)
    {
        Texture = texture;
        Origin = texture.GetSize() / 2.0f;
    }
}

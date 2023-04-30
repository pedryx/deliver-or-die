using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace DeliverOrDie.UI;
/// <summary>
/// Represent user interface element.
/// </summary>
internal abstract class UIElement
{
    private readonly List<UIElement> childs = new();

    /// <summary>
    /// Offset from parent element or position of element if element is directly in ui layer.
    /// </summary>
    public Vector2 Offset;

    /// <summary>
    /// UI layer to which this element belong.
    /// </summary>
    public UILayer Owner { get; private set; }

    /// <summary>
    /// Get size of an element.
    /// </summary>
    public virtual Vector2 Size { get; }

    protected virtual void Initialize() { }

    /// <summary>
    /// Associate element with ui layer.
    /// </summary>
    public void Initialize(UILayer owner)
    {
        Owner = owner;
        Initialize();
    }

    protected void AddChild(UIElement element)
    {
        element.Initialize(Owner);
        childs.Add(element);
    }

    protected void RemoveChild(UIElement element)
        => childs.Remove(element);

    public virtual void Update(float elapsed, Vector2 position)
    {
        foreach (var child in childs)
        {
            child.Update(elapsed, position + child.Offset);
        }
    }

    public virtual void Draw(float elapsed, Vector2 position)
    {
        foreach (var child in childs)
        {
            child.Draw(elapsed, position + child.Offset);
        }
    }
}

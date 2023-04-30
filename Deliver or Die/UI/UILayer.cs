using System.Collections.Generic;

namespace DeliverOrDie.UI;
/// <summary>
/// Represent layer with user interface elements.
/// </summary>
internal class UILayer
{
    /// <summary>
    /// Contains elements which belongs to this layer.
    /// </summary>
    private readonly List<UIElement> elements = new();

    /// <summary>
    /// Owner of this layer.
    /// </summary>
    public GameState GameState { get; private set; }

    /// <param name="gameState">Owner of this layer.</param>
    public UILayer(GameState gameState)
    {
        GameState = gameState;
    }

    public void AddElement(UIElement element)
    {
        element.Initialize(this);
        elements.Add(element);
    }

    public void RemoveElement(UIElement element)
        => elements.Remove(element);

    public void Update(float elapsed)
    {
        foreach (var element in elements)
        {
            element.Update(elapsed, element.Offset);
        }
    }

    public void Draw(float elapsed)
    {
        GameState.Game.SpriteBatch.Begin();
        foreach (var element in elements)
        {
            element.Draw(elapsed, element.Offset);
        }
        GameState.Game.SpriteBatch.End();
    }
}

using UnityEngine;

public class ScenarioMazeElement
{
    public ElementType elementChar;
    public SpriteRenderer elementSpriteRenderer;
    public bool elementCollectable = false;

    public ScenarioMazeElement(ElementType _elementChar)
    {
        this.elementChar = _elementChar;
    }
}

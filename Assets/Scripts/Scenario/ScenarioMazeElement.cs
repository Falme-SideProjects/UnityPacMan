using UnityEngine;

public class ScenarioMazeElement
{
    public ElementType elementType;
    public SpriteRenderer elementSpriteRenderer;
    public bool elementCollectable = false;
    public Vector2 elementPositionInWorld;

    public ScenarioMazeElement(ElementType _elementChar)
    {
        this.elementType = _elementChar;
    }

    public bool CompareElementType(ElementType _elementType)
    {
        return (int)elementType == (int)_elementType;
    }
}

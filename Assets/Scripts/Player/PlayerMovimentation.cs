using UnityEditor;
using UnityEngine;

public class PlayerMovimentation
{
    private Vector2 position;
    private Vector2 screenLimit;

    public PlayerMovimentation()
    {
        screenLimit = new Vector2(4f, 10f);
    }

    public Vector2 GetPosition()
    {
        return this.position;
    }

    public void SetPosition(Vector2 newPosition)
    {
        this.position = newPosition;
    }

    public void Move(Direction direction, float delta=1f)
    {
        switch(direction)
        {
            case Direction.up:
                this.position += Vector2.up * delta;
                break;
            case Direction.right:
                this.position += Vector2.right * delta;
                break;
            case Direction.left:
                this.position += Vector2.left * delta;
                break;
            case Direction.down:
                this.position += Vector2.down * delta;
                break;
        }

        CheckWarp(screenLimit);
    }

    public void CheckWarp(Vector2 limit)
    {
        if (position.x < -limit.x) position.x = limit.x;
        if (position.x > limit.x) position.x = -limit.x;

        if (position.y < -limit.y) position.y = limit.y;
        if (position.y > limit.y) position.y = -limit.y;
    }

    public Vector2 GetPositionInGrid(Vector2 initialPosition, Vector2 endPosition, Vector2 arrayLength)
    {
        Vector2 corneredEndPosition = new Vector2(endPosition.x-initialPosition.x, endPosition.x - initialPosition.y);
        Vector2 corneredPlayerPosition = new Vector2(GetPosition().x - initialPosition.x, GetPosition().y - initialPosition.y);

        if (corneredPlayerPosition.x == 0) return Vector2.zero;
        if (corneredPlayerPosition.x == corneredEndPosition.x) return new Vector2(arrayLength.x-1, 0);
        return Vector2.zero;
    }

    //Function to find the closest number
    public void GetPositionBetweenTwoNumbers()
    {

    }
}

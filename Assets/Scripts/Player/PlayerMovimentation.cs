using UnityEngine;

public class PlayerMovimentation
{
    private Vector2 position;

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
    }
}

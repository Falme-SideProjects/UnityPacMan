using UnityEngine;

public class PlayerMovimentation : CharacterMovimentation
{

    public PlayerMovimentation(ScreenDataScriptableObject screenData=null) : base(screenData)
    {

    }

    public override void Move(Direction direction, float delta = 1)
    {

        if (this.currentDirection != this.nextDirection &&
            GetMovementPermission().CanMoveAt(this.nextDirection))
            currentDirection = this.nextDirection;

        if (!GetMovementPermission().CanMoveAt(GetCurrentDirection()))
            return;


        switch (GetCurrentDirection())
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

        CheckWarp(GetScreenLimit());
    }
}

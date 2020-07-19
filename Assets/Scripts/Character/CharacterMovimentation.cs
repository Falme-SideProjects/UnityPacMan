using UnityEngine;

public class CharacterMovimentation
{
    private MovementPermission movementPermission;

    private Vector2 position;
    private Vector2 initialPosition;
    private Vector2 screenLimit;

    public CharacterMovimentation(ScreenDataScriptableObject screenData=null)
    {
        movementPermission = new MovementPermission();
        if (screenData != null) screenLimit = screenData.screenLimit;
    }

    public MovementPermission GetMovementPermission()
    {
        return this.movementPermission;
    }

    public Vector2 GetInitialPosition()
    {
        return this.initialPosition;
    }

    public void SetInitialPosition(Vector2 newInitialPosition)
    {
        this.initialPosition = newInitialPosition;
    }

    public void ResetPosition()
    {
        this.SetPosition(this.GetInitialPosition());
    }

    public Vector2 GetPosition()
    {
        return this.position;
    }

    public void SetPosition(Vector2 newPosition)
    {
        this.position = newPosition;
    }

    public void Move(Direction direction, float delta = 1f)
    {
        if (!movementPermission.CanMoveAt(direction))
            return;

        switch (direction)
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

    public void SetWarpLimit(Vector2 newLimit)
    {
        this.screenLimit = newLimit;
    }

    public Vector2 GetPositionInGrid(Vector2 initialPosition, Vector2 endPosition, Vector2 arrayLength)
    {
        Vector2 corneredEndPosition = new Vector2(endPosition.x - initialPosition.x, endPosition.y - initialPosition.y);
        Vector2 corneredPlayerPosition = new Vector2(GetPosition().x - initialPosition.x, GetPosition().y - initialPosition.y);

        int _x = GetPositionBetweenTwoNumbers((int)arrayLength.x, corneredEndPosition.x, corneredPlayerPosition.x);
        int _y = GetPositionBetweenTwoNumbers((int)arrayLength.y, corneredEndPosition.y, corneredPlayerPosition.y);

        return new Vector2(_x, _y);
    }

    //Function to find the closest number
    public int GetPositionBetweenTwoNumbers(int arrayLength, float distance, float compareNumber)
    {
        float numberGap = distance / (arrayLength - 1);
        float minimum = float.MaxValue;
        int index = -1;

        for (int a = 0; a < arrayLength; a++)
        {
            float currentPosition = numberGap * a;
            if (Mathf.Abs(currentPosition - compareNumber) < minimum)
            {
                minimum = Mathf.Abs(currentPosition - compareNumber);
                index = a;
            }
        }

        return index;
    }
}

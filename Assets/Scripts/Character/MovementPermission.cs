public class MovementPermission
{
    private bool up = true, 
                left = true, 
                right = true, 
                down = true;

    public bool CanMoveAt(Direction direction)
    {
        switch(direction)
        {
            case Direction.up: return this.up;
            case Direction.left: return this.left;
            case Direction.right: return this.right;
            case Direction.down: return this.down;
        }

        return false;
    }

    public void SetMovePermission(bool _up, bool _left, bool _right, bool _down)
    {
        this.up = _up;
        this.left = _left;
        this.right = _right;
        this.down = _down;
    }


    public void SetOneMovePermission(Direction direction, bool _value)
    {
        switch (direction)
        {
            case Direction.up: 
                this.up = _value;
                break;
            case Direction.left: 
                this.left = _value;
                break;
            case Direction.right: 
                this.right = _value;
                break;
            case Direction.down: 
                this.down = _value;
                break;
        }
    }
}

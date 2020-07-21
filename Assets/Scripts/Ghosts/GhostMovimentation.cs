public class GhostMovimentation : CharacterMovimentation
{
    private Direction currentDirection = Direction.up;

    public GhostMovimentation(ScreenDataScriptableObject screenData=null) : base(screenData)
    {
        
    }

    public Direction GetCurrentDirection()
    {
        return this.currentDirection;
    }

    public void SetCurrentDirection(Direction newDirection)
    {
        this.currentDirection = newDirection;
    }
}

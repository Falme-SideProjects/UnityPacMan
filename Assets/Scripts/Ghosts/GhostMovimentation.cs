using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovimentation : CharacterMovimentation
{
    private Direction currentDirection = Direction.up;

    public Direction GetCurrentDirection()
    {
        return this.currentDirection;
    }

    public void SetCurrentDirection(Direction newDirection)
    {
        this.currentDirection = newDirection;
    }
}

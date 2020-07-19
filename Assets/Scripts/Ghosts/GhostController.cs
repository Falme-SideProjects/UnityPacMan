using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : CharacterController
{
    private void Awake()
    {
        Initialize(new GhostMovimentation(screenData));
    }

    void Update()
    {
        CheckNextPosition();
        RefreshCharacter();
    }

    private void CheckNextPosition()
    {
        GhostMovimentation ghostMovimentation = (GhostMovimentation)characterMovimentation;

        bool CanMoveAtDefinedDirection =
            characterMovimentation.GetMovementPermission().CanMoveAt(ghostMovimentation.GetCurrentDirection());

        if (CanMoveAtDefinedDirection)
        {
            characterMovimentation.Move(ghostMovimentation.GetCurrentDirection(), Time.deltaTime);
        }else
        {

            bool _movedRandomly = MoveRandomPosition();
            if (!_movedRandomly)
            {
                if (characterMovimentation.GetMovementPermission().CanMoveAt(Direction.up))
                    ghostMovimentation.SetCurrentDirection(Direction.up);
                else if (characterMovimentation.GetMovementPermission().CanMoveAt(Direction.left))
                    ghostMovimentation.SetCurrentDirection(Direction.left);
                else if (characterMovimentation.GetMovementPermission().CanMoveAt(Direction.down))
                    ghostMovimentation.SetCurrentDirection(Direction.down);
                else if (characterMovimentation.GetMovementPermission().CanMoveAt(Direction.right))
                    ghostMovimentation.SetCurrentDirection(Direction.right);
            }
        }

    }

    private bool MoveRandomPosition()
    {
        GhostMovimentation ghostMovimentation = (GhostMovimentation)characterMovimentation;

        Direction _randomDirection = (Direction)Random.Range(0, 4);

        bool canGoToDirection = characterMovimentation.GetMovementPermission().CanMoveAt(_randomDirection);

        if (canGoToDirection)
            ghostMovimentation.SetCurrentDirection(_randomDirection);
        
        return canGoToDirection;
    }


}

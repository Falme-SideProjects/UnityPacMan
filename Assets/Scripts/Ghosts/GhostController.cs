using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : CharacterController
{
    [SerializeField] private GhostAI ghostAI;

    private void Awake()
    {
        ghostAI = new GhostAI();
        Initialize(new GhostMovimentation(screenData));
    }

    void Update()
    {
        CheckNextPosition();
        CheckNearestPosition();
        RefreshCharacter();
    }

    private void CheckNearestPosition()
    {
        int _limitY = ghostAI.GetScenarioGrid().Count;
        int _limitX = ghostAI.GetScenarioGrid()[_limitY-1].Count;

        Vector2 startPosition = ghostAI.GetScenarioGrid()[0][0].elementPositionInWorld;
        Vector2 endPosition = ghostAI.GetScenarioGrid()[_limitY-1][_limitX-1].elementPositionInWorld;

        Debug.Log(characterMovimentation.GetPositionInGrid(startPosition, endPosition, new Vector2(_limitX, _limitY)));


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

    public GhostAI GetGhostAI()
    {
        return this.ghostAI;
    }


}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GhostAI
{
    [SerializeField] private GhostType ghostType;
    private GhostState currentState;
    private List<List<ScenarioMazeElement>> scenarioGrid;
    private Vector2 targetPosition;
    private Vector2 ghostPosition;
    private IGhostState ghostState;
    private PlayerMovimentation playerMovimentation;
    private GhostController[] ghostControllers;



    public GhostType GetGhostType()
    {
        return ghostType;
    }

    public void SetGhostType(GhostType newType)
    {
        ghostType = newType;
    }

    public GhostState GetGhostCurrentState()
    {
        return currentState;
    }

    public void SetGhostCurrentState(GhostState newState)
    {
        currentState = newState;
        SetGhostStateClass();
    }

    private void SetGhostStateClass()
    {
        switch(currentState)
        {
            case GhostState.scatter:
                ghostState = new GhostStateScatter();
                break;
            case GhostState.chase:
                ghostState = new GhostStateChase();
                break;
            case GhostState.eaten:
                ghostState = new GhostStateEaten();
                break;
            case GhostState.frightened:
                ghostState = new GhostStateFrightened();
                break;
        }
    }

    public List<List<ScenarioMazeElement>> GetScenarioGrid()
    {
        if (this.scenarioGrid == null) this.scenarioGrid = new List<List<ScenarioMazeElement>>();
        return this.scenarioGrid;
    }

    public void SetScenarioGrid(List<List<ScenarioMazeElement>> scenarioMazeList)
    {
        this.scenarioGrid = scenarioMazeList;
    }
    public Vector2 GetCurrentTarget()
    {
        return this.targetPosition;
    }

    public void SetCurrentTarget()
    {
        if (GetScenarioGrid().Count == 0) return;

        if(IsGhostInBox(ghostPosition) && !CanEnterBox())
        {
            this.targetPosition = new Vector2(13,11);
            return;
        }

        this.targetPosition = ghostState.GetStateTarget(GetGhostType(), GetScenarioGrid(), this.playerMovimentation, this.ghostControllers);

        
    }

    public float GetDistanceBetweenTiles(Vector2 origin, Vector2 destiny)
    {
        float _x = (float)Math.Pow((origin.x - destiny.x), 2);
        float _y = (float)Math.Pow((origin.y - destiny.y), 2);
        return (float) Math.Sqrt(_x + _y);
    }

    public float GetDistanceBetweenTileToTarget(Vector2 origin)
    {
        Vector2 targetWorldPosition = GetTargetWorldPosition();

        float _x = (float)Math.Pow((origin.x - targetWorldPosition.x), 2);
        float _y = (float)Math.Pow((origin.y - targetWorldPosition.y), 2);
        return (float)Math.Sqrt(_x + _y);
    }

    private Vector2 GetTargetWorldPosition()
    {
        return this.scenarioGrid[(int)targetPosition.y][(int)targetPosition.x].elementPositionInWorld;
    }

    public Direction GetNextNearestMove(Vector2 characterPosition, Direction currentDirection)
    {
        List<Direction> directionList = GetPermittedDirections(characterPosition, currentDirection);
        Direction _closestDirection = Direction.up;
        float _minimumDistance = float.MaxValue;
        

        for(int a=0; a< directionList.Count; a++)
        {
            Vector2 _pos = GetTilePositionBasedOnDirection(characterPosition, directionList[a]);
            float _dist = GetDistanceBetweenTileToTarget(_pos);

            if(_dist < _minimumDistance)
            {
                _minimumDistance = _dist;
                _closestDirection = directionList[a];
            }
        }

        return _closestDirection;
    }

    public Vector2 GetTilePositionBasedOnDirection(Vector2 characterPosition, Direction direction)
    {
        if (IsOnEdge((int)characterPosition.x, (int)characterPosition.y, direction))
        {
            if (direction.Equals(Direction.right)) return this.scenarioGrid[(int)characterPosition.y][0].elementPositionInWorld;
            else return this.scenarioGrid[(int)characterPosition.y][scenarioGrid[0].Count-1].elementPositionInWorld;
        }

        switch(direction)
        {

            case Direction.up: 
            return this.scenarioGrid[(int)characterPosition.y - 1][(int)characterPosition.x].elementPositionInWorld;

            case Direction.down:
            return this.scenarioGrid[(int)characterPosition.y + 1][(int)characterPosition.x].elementPositionInWorld;

            case Direction.left:
            return this.scenarioGrid[(int)characterPosition.y][(int)characterPosition.x - 1].elementPositionInWorld;

            case Direction.right:
            return this.scenarioGrid[(int)characterPosition.y][(int)characterPosition.x + 1].elementPositionInWorld;
        }

        return Vector2.zero;
    }

    public List<Direction> GetPermittedDirections(Vector2 characterPosition, Direction currentDirection)
    {
        List<Direction> directionList = new List<Direction>();

        if (!this.scenarioGrid[(int)characterPosition.y - 1][(int)characterPosition.x].elementType.Equals(ElementType.wall) &&
            !currentDirection.Equals(Direction.down)) 
            directionList.Add(Direction.up);

        if (!this.scenarioGrid[(int)characterPosition.y+1][(int)characterPosition.x].elementType.Equals(ElementType.wall) &&
            !currentDirection.Equals(Direction.up) &&
            (!GhostAtBoxDoor(characterPosition) || CanEnterBox())) 
            directionList.Add(Direction.down);

        if(IsOnEdge((int)characterPosition.x, (int)characterPosition.y, Direction.left))
        {
            directionList.Add(Direction.left);
        } else
        {
            if (!this.scenarioGrid[(int)characterPosition.y][(int)characterPosition.x - 1].elementType.Equals(ElementType.wall) &&
                !currentDirection.Equals(Direction.right)) 
                directionList.Add(Direction.left);
        }

        if (IsOnEdge((int)characterPosition.x, (int)characterPosition.y, Direction.right))
        {
            directionList.Add(Direction.right);
        }
        else
        {
            if (!this.scenarioGrid[(int)characterPosition.y][(int)characterPosition.x + 1].elementType.Equals(ElementType.wall) &&
            !currentDirection.Equals(Direction.left))
                directionList.Add(Direction.right);
        }

        return directionList;
    }

    public bool IsGhostInBox(Vector2 characterPosition)
    {
        return
            (characterPosition.x >= 11 && characterPosition.x <= 16) &&
            (characterPosition.y >= 12 && characterPosition.y <= 15);
    }

    private bool GhostAtBoxDoor(Vector2 characterPosition)
    {
        int _x = (int)characterPosition.x;
        int _y = (int)characterPosition.y;
        return (_x == 13 || _x == 14) && _y == 11;
    }

    private bool CanEnterBox()
    {
        return this.GetGhostCurrentState().Equals(GhostState.eaten);
    }

    public bool IsOnEdge(int _x, int _y, Direction currentDirection)
    {
        return (_y == 14 && _x == 0 && currentDirection.Equals(Direction.left)) ||
                (_y == 14 && _x == (scenarioGrid[0].Count-1) && currentDirection.Equals(Direction.right));
    }

    public void SetPlayerMovimentation(PlayerMovimentation _playerMovimentation)
    {
        this.playerMovimentation = _playerMovimentation;
    }

    public void SetGhostControllers(GhostController[] _ghostControllers)
    {
        this.ghostControllers = _ghostControllers;
    }

    public void SetGhostPosition(Vector2 position)
    {
        ghostPosition = position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private GhostMovimentation ghostMovimentation;

    private void Awake()
    {
        ghostMovimentation = new GhostMovimentation();

        ghostMovimentation.SetInitialPosition(transform.position);
        ghostMovimentation.ResetPosition();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckNextPosition();
        RefreshGhost();
    }

    private void CheckNextPosition()
    {

        bool CanMoveAtDefinedDirection =
            ghostMovimentation.GetMovementPermission().CanMoveAt(ghostMovimentation.GetCurrentDirection());

        if (CanMoveAtDefinedDirection)
        {
            ghostMovimentation.Move(ghostMovimentation.GetCurrentDirection(), Time.deltaTime);
        }else
        {

            bool _movedRandomly = MoveRandomPosition();
            if (!_movedRandomly)
            {
                if (ghostMovimentation.GetMovementPermission().CanMoveAt(Direction.up))
                    ghostMovimentation.SetCurrentDirection(Direction.up);
                else if (ghostMovimentation.GetMovementPermission().CanMoveAt(Direction.left))
                    ghostMovimentation.SetCurrentDirection(Direction.left);
                else if (ghostMovimentation.GetMovementPermission().CanMoveAt(Direction.down))
                    ghostMovimentation.SetCurrentDirection(Direction.down);
                else if (ghostMovimentation.GetMovementPermission().CanMoveAt(Direction.right))
                    ghostMovimentation.SetCurrentDirection(Direction.right);
            }
        }

    }

    private bool MoveRandomPosition()
    {
        Direction _randomDirection = (Direction)Random.Range(0, 4);

        if(ghostMovimentation.GetMovementPermission().CanMoveAt(_randomDirection))
        {
            ghostMovimentation.SetCurrentDirection(_randomDirection);
            return true;
        } else
        {
            return false;
        }

    }

    private void RefreshGhost()
    {
        this.transform.position =
            new Vector3(ghostMovimentation.GetPosition().x,
                        ghostMovimentation.GetPosition().y,
                        transform.position.z);
    }

    public GhostMovimentation GetGhostMovimentation()
    {
        return this.ghostMovimentation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : CharacterController
{
    [SerializeField] private GhostAI ghostAI;
    private bool initialized = false;
    GhostMovimentation ghostMovimentation;

    private Vector2 cachedGhostPosition;

    private void Start()
    {
    }

    void Update()
    {
        if (!initialized) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) ghostAI.SetGhostCurrentState(GhostState.eaten);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ghostAI.SetGhostCurrentState(GhostState.scatter);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ghostAI.SetGhostCurrentState(GhostState.frightened);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) ghostAI.SetGhostCurrentState(GhostState.chase);

        CheckNearestPosition();
        RefreshCharacter();
    }

    public void InitializeGhost()
    {
        Initialize(new GhostMovimentation(screenData));
        ghostAI.SetGhostCurrentState(GhostState.scatter);
        ghostAI.SetCurrentTarget();

        ghostMovimentation = (GhostMovimentation)characterMovimentation;


        initialized = true;
    }

    private void CheckNearestPosition()
    {
        if (ghostAI.GetScenarioGrid().Count == 0) return;
        
        int _limitY = ghostAI.GetScenarioGrid().Count;
        int _limitX = ghostAI.GetScenarioGrid()[_limitY-1].Count;

        Vector2 startPosition = ghostAI.GetScenarioGrid()[0][0].elementPositionInWorld;
        Vector2 endPosition = ghostAI.GetScenarioGrid()[_limitY-1][_limitX-1].elementPositionInWorld;

        Vector2 _ghostPosition = characterMovimentation.GetPositionInGrid(startPosition, endPosition, new Vector2(_limitX, _limitY));

        Debug.Log(_ghostPosition);

        CheckNextPosition(_ghostPosition);
    }

    private void CheckNextPosition(Vector2 _ghostPosition)
    {


        if(_ghostPosition != cachedGhostPosition)
        {
            ghostAI.SetGhostPosition(_ghostPosition);
            ghostAI.SetCurrentTarget();
            ghostMovimentation.SetCurrentDirection(ghostAI.GetNextNearestMove(_ghostPosition, ghostMovimentation.GetCurrentDirection()));
            cachedGhostPosition = _ghostPosition;
        }

        characterMovimentation.Move(ghostMovimentation.GetCurrentDirection(), Time.deltaTime);


    }

    public GhostAI GetGhostAI()
    {
        return this.ghostAI;
    }


}

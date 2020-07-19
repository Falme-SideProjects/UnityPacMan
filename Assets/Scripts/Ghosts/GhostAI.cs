using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostAI
{
    [SerializeField] private GhostType ghostType;
    private GhostState currentState;
    private List<List<ScenarioMazeElement>> scenarioGrid;
    private Vector2 targetPosition;


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
        if(GetGhostCurrentState().Equals(GhostState.scatter))
        {
            switch(GetGhostType())
            {
                case GhostType.pinky:
                    this.targetPosition = new Vector2(0, 0);
                    break;
                case GhostType.blinky:
                    this.targetPosition = new Vector2(GetScenarioGrid()[0].Count-1, 0);
                    break;
                case GhostType.clyde:
                    this.targetPosition = new Vector2(0, GetScenarioGrid().Count-1);
                    break;
                case GhostType.inky:
                    this.targetPosition = new Vector2(GetScenarioGrid()[0].Count-1, GetScenarioGrid().Count-1);
                    break;
            }
        }
    }
}

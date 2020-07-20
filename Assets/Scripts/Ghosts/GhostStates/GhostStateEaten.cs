using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateEaten : IGhostState
{
    public Vector2 GetStateTarget(GhostType ghostType, List<List<ScenarioMazeElement>> scenarioGrid, PlayerMovimentation playerMovimentation)
    {
        return new Vector2(13,13);
    }
}

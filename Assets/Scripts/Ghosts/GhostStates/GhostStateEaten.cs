using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateEaten : IGhostState
{
    private const int X = 13, Y = 13;

    public Vector2 GetStateTarget(GhostType ghostType,
                                    List<List<ScenarioMazeElement>> scenarioGrid,
                                    PlayerMovimentation playerMovimentation,
                                    GhostController[] ghostControllers)
    {
        return new Vector2(X, Y);
    }
}

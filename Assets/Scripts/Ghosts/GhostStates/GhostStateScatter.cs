using System.Collections.Generic;
using UnityEngine;

public class GhostStateScatter : IGhostState
{
    public Vector2 GetStateTarget(GhostType ghostType,
                                    List<List<ScenarioMazeElement>> scenarioGrid,
                                    PlayerMovimentation playerMovimentation,
                                    GhostController[] ghostControllers)
    {
        switch (ghostType)
        {
            case GhostType.pinky:
                return new Vector2(0, 0);
            case GhostType.blinky:
                return new Vector2(scenarioGrid[0].Count - 1, 0);
            case GhostType.clyde:
                return new Vector2(0, scenarioGrid.Count - 1);
            case GhostType.inky:
                return new Vector2(scenarioGrid[0].Count - 1, scenarioGrid.Count - 1);
        }

        return Vector2.zero;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GhostStateFrightened : IGhostState
{
    public Vector2 GetStateTarget(GhostType ghostType,
                                    List<List<ScenarioMazeElement>> scenarioGrid,
                                    PlayerMovimentation playerMovimentation,
                                    GhostController[] ghostControllers)
    {
        int RandomDirection = Random.Range(0, 4);

        switch (RandomDirection)
        {
            case 0:
                return new Vector2(0, 0);
            case 1:
                return new Vector2(scenarioGrid[0].Count - 1, 0);
            case 2:
                return new Vector2(0, scenarioGrid.Count - 1);
            case 3:
                return new Vector2(scenarioGrid[0].Count - 1, scenarioGrid.Count - 1);
        }

        return Vector2.zero;
    }
}

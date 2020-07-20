using System.Collections.Generic;
using UnityEngine;

public interface IGhostState
{
    Vector2 GetStateTarget(GhostType ghostType, 
                            List<List<ScenarioMazeElement>> scenarioGrid, 
                            PlayerMovimentation playerMovimentation, 
                            GhostController[] ghostControllers);
}

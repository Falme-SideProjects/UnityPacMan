using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateChase : IGhostState
{
    public Vector2 GetStateTarget(GhostType ghostType, List<List<ScenarioMazeElement>> scenarioGrid, PlayerMovimentation playerMovimentation)
    {
        switch (ghostType)
        {
            case GhostType.pinky:

                switch(playerMovimentation.GetCurrentDirection())
                {
                    case Direction.up: return (playerMovimentation.GetCachedPosition()+(Vector2.up*4));
                    case Direction.left: return (playerMovimentation.GetCachedPosition() + (Vector2.left * 4));
                    case Direction.right: return (playerMovimentation.GetCachedPosition() + (Vector2.right * 4));
                    case Direction.down: return (playerMovimentation.GetCachedPosition() + (Vector2.down * 4));
                }
                break;
            case GhostType.blinky:
                return playerMovimentation.GetCachedPosition();
            case GhostType.clyde:
                return new Vector2(0, scenarioGrid.Count - 1);
            case GhostType.inky:
                return new Vector2(scenarioGrid[0].Count - 1, scenarioGrid.Count - 1);
        }

        return Vector2.zero;
    }
}

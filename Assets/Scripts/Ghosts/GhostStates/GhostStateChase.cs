using System;
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

                Vector2 nextTarget = Vector2.zero;

                switch(playerMovimentation.GetCurrentDirection())
                {
                    case Direction.up: 
                        nextTarget = (playerMovimentation.GetCachedPosition()+(Vector2.up*4));
                        break;
                    case Direction.left: 
                        nextTarget = (playerMovimentation.GetCachedPosition() + (Vector2.left * 4));
                        break;
                    case Direction.right: 
                        nextTarget = (playerMovimentation.GetCachedPosition() + (Vector2.right * 4));
                        break;
                    case Direction.down: 
                        nextTarget = (playerMovimentation.GetCachedPosition() + (Vector2.down * 4));
                        break;
                }

                nextTarget.x = Mathf.Clamp(nextTarget.x, 0, 27);
                nextTarget.y = Mathf.Clamp(nextTarget.y, 0, 30);

                return nextTarget;
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

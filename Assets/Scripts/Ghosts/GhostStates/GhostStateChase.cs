using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStateChase : IGhostState
{
    private GhostController blinkyCachedController, clydeCachedController;

    public Vector2 GetStateTarget(GhostType ghostType, 
                                    List<List<ScenarioMazeElement>> scenarioGrid, 
                                    PlayerMovimentation playerMovimentation,
                                    GhostController[] ghostControllers)
    {
        Vector2 nextTarget = Vector2.zero;

        switch (ghostType)
        {
            case GhostType.pinky:

                nextTarget = GetPlayerOffsetPosition(playerMovimentation, 4);

                nextTarget.x = Mathf.Clamp(nextTarget.x, 0, 27);
                nextTarget.y = Mathf.Clamp(nextTarget.y, 0, 30);

                return nextTarget;
            case GhostType.blinky:
                return playerMovimentation.GetCachedPosition();
            case GhostType.inky:

                nextTarget = GetPlayerOffsetPosition(playerMovimentation, 2);

                nextTarget.x = Mathf.Clamp(nextTarget.x, 0, 27);
                nextTarget.y = Mathf.Clamp(nextTarget.y, 0, 30);

                CacheGhostsMovimentation(ghostControllers);

                Vector2 blinkyPosition = GetBlinkyPosition();

                nextTarget = GetMirroredPositionBetweenBlinkyAndPlayer(nextTarget, blinkyPosition);


                nextTarget.x = Mathf.Clamp(nextTarget.x, 0, 27);
                nextTarget.y = Mathf.Clamp(nextTarget.y, 0, 30);

                return nextTarget;
            case GhostType.clyde:

                CacheGhostsMovimentation(ghostControllers);

                if (IsTooFarFromOtherTile(playerMovimentation.GetCachedPosition(), GetClydePosition(),8))
                    return playerMovimentation.GetCachedPosition();
                else
                    return new Vector2(0, scenarioGrid.Count - 1);
        }

        return Vector2.zero;
    }

    private void CacheGhostsMovimentation(GhostController[] ghostControllers)
    {
        if(blinkyCachedController == null || clydeCachedController == null)
            for(int a=0; a< ghostControllers.Length; a++)
                if (blinkyCachedController == null && ghostControllers[a].GetGhostAI().GetGhostType().Equals(GhostType.blinky))
                    blinkyCachedController = ghostControllers[a];
                else if (clydeCachedController == null && ghostControllers[a].GetGhostAI().GetGhostType().Equals(GhostType.clyde))
                    clydeCachedController = ghostControllers[a];
    }

    private Vector2 GetBlinkyPosition()
    {
        return blinkyCachedController.GetCharacterMovimentation().GetCachedPosition();
    }

    private Vector2 GetClydePosition()
    {
        return clydeCachedController.GetCharacterMovimentation().GetCachedPosition();
    }

    public Vector2 GetMirroredPositionBetweenBlinkyAndPlayer(Vector2 _playerNextPosition, Vector2 _blinkyPosition)
    {
        Vector2 difference = _playerNextPosition - _blinkyPosition;

        return (_playerNextPosition + difference);
    }

    private Vector2 GetPlayerOffsetPosition(PlayerMovimentation playerMovimentation, int _offset)
    {
        switch (playerMovimentation.GetCurrentDirection())
        {
            case Direction.up:
                return (playerMovimentation.GetCachedPosition() + (Vector2.up * _offset));
            case Direction.left:
                return (playerMovimentation.GetCachedPosition() + (Vector2.left * _offset));
            case Direction.right:
                return (playerMovimentation.GetCachedPosition() + (Vector2.right * _offset));
            case Direction.down:
                return (playerMovimentation.GetCachedPosition() + (Vector2.down * _offset));
        }

        return playerMovimentation.GetCachedPosition();
    }

    public bool IsTooFarFromOtherTile(Vector2 _playerPosition, Vector2 _ghostPosition, int _distance)
    {
        Vector2 result = _playerPosition - _ghostPosition;
        Vector2 absoluteResult = new Vector2(Mathf.Abs(result.x), Mathf.Abs(result.y));
        return (absoluteResult.x >= _distance || absoluteResult.y >= _distance) ;
    }
}

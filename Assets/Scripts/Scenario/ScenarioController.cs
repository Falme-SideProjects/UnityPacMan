using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;

public class ScenarioController : MonoBehaviour
{
    private ScenarioMap scenarioMap;
    private List<List<ScenarioMazeElement>> scenarioGrid;
    private PlayerMovimentation playerMovimentation;

    private Coroutine powerPelletCoroutine;

    private int pelletsCollected = 0;

    private int indexPhase = 0;
    private float timeForNextPhase = 0;
    private float currentTimePhase = 0;

    [Header("Data")]
    [SerializeField] private ScenarioDataScriptableObject scenarioData;
    [SerializeField] private GameplayTimersDataScriptableObject gameplayTimersData;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GhostController[] ghostController;
    [SerializeField] private ScoreController scoreController;

    private void Awake()
    {
        InitializeMap();
    }

    void Start()
    {
        playerMovimentation = (PlayerMovimentation)playerController.GetCharacterMovimentation();
        SpawnScenario();
        InitializeGhostController();
        NextGhostTimerPhase();
    }

    void Update()
    {
        CheckCharactersMovimentBasedOnLocal();
        CheckForOverlaps();
        CheckTimerPhase();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetScenario();
        }
    }

    private void InitializeGhostController()
    {
        for (int a = 0; a < ghostController.Length; a++)
        {
            ghostController[a].GetGhostAI().SetScenarioGrid(this.scenarioGrid);
            ghostController[a].GetGhostAI().SetPlayerMovimentation(playerMovimentation);
            ghostController[a].GetGhostAI().SetGhostControllers(ghostController);
            ghostController[a].InitializeGhost();
        }

    }


    private void InitializeMap()
    {
        scenarioMap = new ScenarioMap();
        string inlineMap = System.Text.RegularExpressions.Regex.Replace(scenarioData.mapText.text, @"\t|\n|\r", "");
        scenarioMap.SetScenarioString(inlineMap);
    }

    private void SpawnScenario()
    {
        this.scenarioGrid = scenarioMap.GetScenarioGrid(scenarioData.levelWidth, scenarioData.levelHeight);

        for(int h=0; h< this.scenarioGrid.Count; h++)
            for (int w = 0; w < this.scenarioGrid[h].Count; w++)
                CreateElementsInScenario(w, h);
    }

    private void CreateElementsInScenario(int x, int y)
    {
        ScenarioMazeElement _scenarioMazeElement = this.scenarioGrid[y][x];
        _scenarioMazeElement.elementPositionInWorld = GetInstatiateTilePosition(x, y);

        if (_scenarioMazeElement.elementType.Equals(ElementType.empty)) return;

        GameObject tileObject = Instantiate(scenarioData.tile, GetInstatiateTilePosition(x, y), Quaternion.identity, transform);

        _scenarioMazeElement.elementSpriteRenderer = tileObject.GetComponent<SpriteRenderer>();

        switch (_scenarioMazeElement.elementType)
        {
            case ElementType.pacdot:
                _scenarioMazeElement.elementSpriteRenderer.sprite = scenarioData.dotSprite;
                _scenarioMazeElement.elementCollectable = true;
                break;
            case ElementType.power:
                _scenarioMazeElement.elementSpriteRenderer.sprite = scenarioData.powerPelletSprite;
                _scenarioMazeElement.elementCollectable = true;
                break;
        }
    }

    private Vector3 GetInstatiateTilePosition(int x, int y)
    {
        Vector3 _tilePosition = Vector3.right * scenarioMap.GetCenteredTilePositionByIndex(scenarioData.levelWidth, x, scenarioData.distanceBetweenTiles);
        _tilePosition += Vector3.down * scenarioMap.GetCenteredTilePositionByIndex(scenarioData.levelHeight, y, scenarioData.distanceBetweenTiles);

        return _tilePosition;
    }

    private void ResetScenario()
    {
        for (int h = 0; h < this.scenarioGrid.Count; h++)
            for (int w = 0; w < this.scenarioGrid[h].Count; w++)
                ResetScenarioCell(this.scenarioGrid[h][w]);

        playerMovimentation.ResetPosition();

        for (int a = 0; a < ghostController.Length; a++)
        {
            ghostController[a].GetCharacterMovimentation().ResetPosition();

        }

        ResetTimers();

        pelletsCollected = 0;
    }

    private void ResetScenarioCell(ScenarioMazeElement mazeElement)
    {
        if (mazeElement.elementType.Equals(ElementType.pacdot) ||
                    mazeElement.elementType.Equals(ElementType.power))
        {
            mazeElement.elementSpriteRenderer.enabled = true;
            mazeElement.elementCollectable = true;
        }
    }

    private Vector2 GetLevelPosition()
    {
        float levelPositionX = scenarioMap.GetCenteredTilePositionByIndex(scenarioData.levelWidth, 0, scenarioData.distanceBetweenTiles);
        float levelPositionY = scenarioMap.GetCenteredTilePositionByIndex(scenarioData.levelHeight, 0, scenarioData.distanceBetweenTiles);

        return new Vector2(levelPositionX, levelPositionY);
    }

    private void CheckCharactersMovimentBasedOnLocal()
    {
        Vector2 _levelPosition = GetLevelPosition();

        for (int a = 0; a < ghostController.Length; a++)
        {
            GhostMovimentation ghostMovimentation = (GhostMovimentation)ghostController[a].GetCharacterMovimentation();
            
            CheckPlayerPermission(_levelPosition, ghostMovimentation);
        }

        CheckPlayerPermission(_levelPosition, playerMovimentation);
    }

    private void CheckPlayerPermission(Vector2 _levelPosition, CharacterMovimentation characterMovimentation)
    {

        Vector2 characterPositionGrid =
            characterMovimentation.GetPositionInGrid(new Vector2(_levelPosition.x, -_levelPosition.y),
                                              new Vector2(-_levelPosition.x, _levelPosition.y),
                                              new Vector2(scenarioData.levelWidth, scenarioData.levelHeight));

        ChangeMovementPermissionBasedOnLocal(characterPositionGrid, characterMovimentation);
        
        if(characterMovimentation is PlayerMovimentation)
            CheckCollectedPacDot(characterPositionGrid);
    }

    private void ChangeMovementPermissionBasedOnLocal(Vector2 characterPosition, CharacterMovimentation characterMovimentation)
    {
        int _x = (int)characterPosition.x;
        int _y = (int)characterPosition.y;

        MovementPermission _movementPermission = characterMovimentation.GetMovementPermission();

        _movementPermission.SetMovePermission(true, true, true, true);

        if (_y != 0)
            _movementPermission.
                SetOneMovePermission(Direction.up, !scenarioGrid[_y - 1][_x].elementType.Equals(ElementType.wall));

        if (_y != scenarioData.levelHeight -1)
            _movementPermission.
                SetOneMovePermission(Direction.down, !scenarioGrid[_y + 1][_x].elementType.Equals(ElementType.wall));

        if (_x != 0)
            _movementPermission.
                SetOneMovePermission(Direction.left, !scenarioGrid[_y][_x-1].elementType.Equals(ElementType.wall));

        if (_x != scenarioData.levelWidth - 1)
            _movementPermission.
                SetOneMovePermission(Direction.right, !scenarioGrid[_y][_x+1].elementType.Equals(ElementType.wall));

    }

    private void CheckCollectedPacDot(Vector2 characterPosition)
    {

        int _x = (int)characterPosition.x;
        int _y = (int)characterPosition.y;

        if(scenarioGrid[_y][_x].elementType.Equals(ElementType.pacdot) ||
            scenarioGrid[_y][_x].elementType.Equals(ElementType.power))
        {
            if(scenarioGrid[_y][_x].elementCollectable)
            {
                scenarioGrid[_y][_x].elementCollectable = false;
                scenarioGrid[_y][_x].elementSpriteRenderer.enabled = false;
                pelletsCollected++;

                if (scenarioGrid[_y][_x].elementType.Equals(ElementType.pacdot))
                {
                    scoreController.Score.AddScoreBasedOnItem(Items.pacdot);
                }
                else
                {
                    CollectedPowerPellet();
                    scoreController.Score.AddScoreBasedOnItem(Items.power);
                }


                scoreController.UpdateUI();

                CheckAllPacDotsCollected();
            }
        }
    }

    private void CheckAllPacDotsCollected()
    {
        if(pelletsCollected >= scenarioData.totalPelletsToCollect)
        {
            ResetScenario();
        }
    }

    private void CheckForOverlaps()
    {
        Vector2 _levelPosition = GetLevelPosition();
        Vector2 _playerPosition = playerMovimentation.GetPositionInGrid(
                                                new Vector2(_levelPosition.x, -_levelPosition.y),
                                              new Vector2(-_levelPosition.x, _levelPosition.y),
                                              new Vector2(scenarioData.levelWidth, scenarioData.levelHeight));
        Vector2[] _ghostPositions = new Vector2[ghostController.Length];

        for (int a = 0; a < ghostController.Length; a++)
            _ghostPositions[a] = ghostController[a].GetCharacterMovimentation().GetPositionInGrid(
                                                new Vector2(_levelPosition.x, -_levelPosition.y),
                                              new Vector2(-_levelPosition.x, _levelPosition.y),
                                              new Vector2(scenarioData.levelWidth, scenarioData.levelHeight));

        CheckForPlayerAndGhostCollision(_playerPosition, _ghostPositions);
        CheckForEatenGhostBackToBox(_ghostPositions);
    }

    private void CheckForPlayerAndGhostCollision(Vector2 _playerPosition, Vector2[] _ghostPositions)
    {
        for(int a=0; a< _ghostPositions.Length; a++)
            if (Vector2.Distance(_ghostPositions[a], _playerPosition) == 0)
            {
                if(ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.frightened))
                {
                    ghostController[a].GetGhostAI().SetGhostCurrentState(GhostState.eaten);
                    ghostController[a].UpdateGhostVisuals();
                } else if(!ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.eaten))
                {
                    PlayerDeath();
                }
                return;
            }
    }

    private void CheckForEatenGhostBackToBox(Vector2[] _ghostPositions)
    {
        for (int a = 0; a < _ghostPositions.Length; a++)
            if (_ghostPositions[a].x == 13 && _ghostPositions[a].y == 13)
            {
                ghostController[a].GetGhostAI().SetGhostCurrentState(GhostState.scatter);
                ghostController[a].UpdateGhostVisuals();
            }
    }

    private void CollectedPowerPellet()
    {
        for (int a = 0; a < ghostController.Length; a++)
        {
            ghostController[a].GetGhostAI().SetGhostCurrentState(GhostState.frightened);
            ghostController[a].UpdateGhostVisuals();
        }

        if (powerPelletCoroutine != null) StopCoroutine(powerPelletCoroutine);
        powerPelletCoroutine = StartCoroutine(PowerPelletDurationCoroutine());
    }

    private void FinishPowerPelletEffect()
    {
        for (int a = 0; a < ghostController.Length; a++)
        {
            if(ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.frightened))
            {
                ghostController[a].GetGhostAI().SetGhostCurrentState(GhostState.chase);
                ghostController[a].UpdateGhostVisuals();
            }
        }

        powerPelletCoroutine = null;
    }

    private void PlayerDeath()
    {
        playerMovimentation.ResetPosition();

        for (int a = 0; a < ghostController.Length; a++)
        {
            ghostController[a].GetCharacterMovimentation().ResetPosition();

        }
    }

    private IEnumerator PowerPelletDurationCoroutine()
    {
        yield return new WaitForSeconds(10f);
        FinishPowerPelletEffect();
    }

    private void CheckTimerPhase()
    {
        currentTimePhase += Time.deltaTime;
        if(currentTimePhase > timeForNextPhase)
        {
            NextGhostTimerPhase();
        }
    }

    private void NextGhostTimerPhase()
    {
        if(indexPhase >= gameplayTimersData.ghostStateTimestamps.Length)
        {
            for (int a = 0; a < ghostController.Length; a++)
                if (!ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.frightened) &&
                !ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.eaten))
                    ghostController[a].GetGhostAI().SetGhostCurrentState(GhostState.chase);
            
            return;
        }

        timeForNextPhase = gameplayTimersData.ghostStateTimestamps[indexPhase].duration;

        for(int a=0; a< ghostController.Length; a++)
        {
            if(!ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.frightened) &&
                !ghostController[a].GetGhostAI().GetGhostCurrentState().Equals(GhostState.eaten))
            {
                ghostController[a].GetGhostAI().SetGhostCurrentState(gameplayTimersData.ghostStateTimestamps[indexPhase].ghostState);
                ghostController[a].UpdateGhostVisuals();
            }
        }

        currentTimePhase = 0;
        indexPhase++;
    }

    private void ResetTimers()
    {
        if (powerPelletCoroutine != null)
        {
            StopCoroutine(powerPelletCoroutine);
            powerPelletCoroutine = null;
        }

        FinishPowerPelletEffect();

        indexPhase = 0;
        timeForNextPhase = 0;
        currentTimePhase = 0;
        NextGhostTimerPhase();
    }



}

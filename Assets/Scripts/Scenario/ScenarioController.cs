using UnityEngine;
using System.Collections.Generic;

public class ScenarioController : MonoBehaviour
{
    private ScenarioMap scenarioMap;
    private List<List<ScenarioMazeElement>> scenarioGrid;
    private PlayerMovimentation playerMovimentation;

    private int pelletsCollected = 0;

    [Header("Data")]
    [SerializeField] private ScenarioDataScriptableObject scenarioData;

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
    }

    void Update()
    {
        CheckCharactersMovimentBasedOnLocal();


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
                tileObject.GetComponent<SpriteRenderer>().color = Color.green;
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

                if (scenarioGrid[_y][_x].elementType.Equals(ElementType.pacdot)) scoreController.Score.AddScoreBasedOnItem(Items.pacdot);
                else scoreController.Score.AddScoreBasedOnItem(Items.power);


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



}

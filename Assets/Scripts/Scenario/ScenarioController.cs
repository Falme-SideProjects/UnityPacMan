using UnityEngine;
using System.Collections.Generic;

public class ScenarioController : MonoBehaviour
{
    private ScenarioMap scenarioMap;
    private List<List<char>> scenarioGrid;

    [Header("Data")]
    [SerializeField] private float distance = 1f;
    [SerializeField] private int levelWidth, levelHeight;
    [SerializeField] private TextAsset mapText;


    [Header("Prefabs")]
    [SerializeField] private GameObject tile;

    [Header("Sprites")]
    [SerializeField] private Sprite dotSprite;

    private void Awake()
    {
        scenarioMap = new ScenarioMap();

        string inlineMap = System.Text.RegularExpressions.Regex.Replace(mapText.text, @"\t|\n|\r", "");

        scenarioMap.SetScenarioString(inlineMap);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnScenario();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnScenario()
    {
        this.scenarioGrid = scenarioMap.GetScenarioGrid(levelWidth, levelHeight);

        for(int h=0; h< this.scenarioGrid.Count; h++)
        {
            for (int w = 0; w < this.scenarioGrid[h].Count; w++)
            {
                if (this.scenarioGrid[h][w].Equals('0')) continue;

                Vector3 _tilePosition = Vector3.right * scenarioMap.GetCenteredTilePositionByIndex(levelWidth, w, distance);
                _tilePosition += Vector3.down * scenarioMap.GetCenteredTilePositionByIndex(levelHeight, h, distance);

                GameObject tileObject = Instantiate(tile, _tilePosition, Quaternion.identity, transform);

                switch(this.scenarioGrid[h][w])
                {
                    case '2':
                        tileObject.GetComponent<SpriteRenderer>().sprite = dotSprite;
                        break;
                    case '3':
                        tileObject.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                }
            }
        }
    }

    
}

using UnityEngine;
using System.Collections.Generic;

public class ScenarioController : MonoBehaviour
{
    ScenarioMap scenarioMap;

    [Header("Data")]
    [SerializeField] private float distance = 1f;
    [SerializeField] private int levelWidth, levelHeight;


    [Header("Prefabs")]
    [SerializeField] private GameObject tile; 

    private void Awake()
    {
        scenarioMap = new ScenarioMap();

        scenarioMap.SetScenarioString("1111111111111111111111111111" +
                                      "1000000000000000000000000001" +
                                      "1000000000010000000000000001" +
                                      "1000000000000000000000000001" +
                                      "1111111111111111111111111111");
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
        List<List<char>> scenarioGrid = scenarioMap.GetScenarioGrid(levelWidth, levelHeight);

        for(int h=0; h<scenarioGrid.Count; h++)
        {
            for (int w = 0; w < scenarioGrid[h].Count; w++)
            {
                if (scenarioGrid[h][w].Equals('0')) continue;

                Vector3 _tilePosition = Vector3.right * scenarioMap.GetCenteredTilePositionByIndex(levelWidth, w, distance);
                _tilePosition += Vector3.down * scenarioMap.GetCenteredTilePositionByIndex(levelHeight, h, distance);

                Instantiate(tile, _tilePosition, Quaternion.identity, transform);
            }
        }
    }

    
}

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

    [Header("Sprites")]
    [SerializeField] private Sprite dotSprite;

    private void Awake()
    {
        scenarioMap = new ScenarioMap();

        scenarioMap.SetScenarioString("1111111111111111111111111111" +
                                      "1222222222222112222222222221" +
                                      "1211112111112112111112111121" +
                                      "1311112111112112111112111131" +
                                      "1211112111112112111112111121" +
                                      "1222222222222222222222222221" +
                                      "1211112112111111112112111121" +
                                      "1211112112111111112112111121" +
                                      "1222222112222112222112222221" +
                                      "1111112111110110111112111111" +
                                      "0000012111110110111112100000" +
                                      "0000012110000000000112100000" +
                                      "0000012110111441110112100000" +
                                      "1111112110100000010112111111" +
                                      "0000002000100000010002000000" +
                                      "1111112110100000010112111111" +
                                      "0000012110111111110112100000" +
                                      "0000012110000000000112100000" +
                                      "0000012110111111110112100000" +
                                      "1111112110111111110112111111" +
                                      "1222222222222112222222222221" +
                                      "1211112111112112111112111121" +
                                      "1211112111112112111112111121" +
                                      "1322112222222222222222112231" +
                                      "1112112112111111112112112111" +
                                      "1112112112111111112112112111" +
                                      "1222222112222112222112222221" +
                                      "1211111111112112111111111121" +
                                      "1211111111112112111111111121" +
                                      "1222222222222222222222222221" +
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

                GameObject tileObject = Instantiate(tile, _tilePosition, Quaternion.identity, transform);

                switch(scenarioGrid[h][w])
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

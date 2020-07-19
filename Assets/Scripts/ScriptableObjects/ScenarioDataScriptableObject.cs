using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "ScriptableObject/ScenarioData", order = 0)]
public class ScenarioDataScriptableObject : ScriptableObject
{
    [Header("Data")]
    public float distanceBetweenTiles = 1f;
    public int levelWidth, levelHeight;
    public int totalPelletsToCollect;
    public TextAsset mapText;

    [Header("Prefabs")]
    public GameObject tile;

    [Header("Sprites")]
    public Sprite dotSprite;
}

using UnityEngine;

[CreateAssetMenu(fileName = "GhostStateData", menuName = "ScriptableObject/GhostStateData", order = 0)]
public class GhostStateDataScriptableObject : ScriptableObject
{
    public GhostStateInfo[] ghostStateInfos;  
}

[System.Serializable]
public class GhostStateInfo
{
    public GhostState state;
    public Sprite stateSprite;
}
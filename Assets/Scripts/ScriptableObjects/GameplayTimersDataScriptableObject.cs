using UnityEngine;

[CreateAssetMenu(fileName = "GameplayTimersData", menuName = "ScriptableObject/GameplayTimersData", order = 0)]
public class GameplayTimersDataScriptableObject : ScriptableObject
{
    public GhostStateTimestamps[] ghostStateTimestamps;  
}

[System.Serializable]
public class GhostStateTimestamps
{
    public GhostState ghostState;
    public float duration;
}
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ScreenData", menuName = "ScriptableObject/ScreenData", order = 0)]
public class ScreenDataScriptableObject : ScriptableObject
{
    public Vector2 screenLimit;
}

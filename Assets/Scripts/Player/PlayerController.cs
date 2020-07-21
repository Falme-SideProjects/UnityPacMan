using UnityEngine;

public class PlayerController : CharacterController
{
    [Header("Data")]
    [SerializeField] private Vector2 initialPlayerPosition;
    [SerializeField] private CharacterDataScriptableObject playerData;

    private void Awake()
    {
        Initialize(new PlayerMovimentation(screenData));
    }

    void Update()
    {
        CheckInput();
        characterMovimentation.Move(characterMovimentation.GetCurrentDirection(), Time.deltaTime* playerData.velocity);
        RefreshCharacter();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.UpArrow)) characterMovimentation.SetNextDirection(Direction.up);
        else if (Input.GetKey(KeyCode.DownArrow)) characterMovimentation.SetNextDirection(Direction.down);
        else if (Input.GetKey(KeyCode.LeftArrow)) characterMovimentation.SetNextDirection(Direction.left);
        else if (Input.GetKey(KeyCode.RightArrow)) characterMovimentation.SetNextDirection(Direction.right);
    }

}

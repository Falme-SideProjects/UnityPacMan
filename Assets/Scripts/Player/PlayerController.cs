using UnityEngine;

public class PlayerController : CharacterController
{
    [SerializeField] private Vector2 initialPlayerPosition;

    private void Awake()
    {
        Initialize(new PlayerMovimentation(screenData));
    }

    void Update()
    {
        CheckInput();
        RefreshCharacter();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.UpArrow)) characterMovimentation.Move(Direction.up, Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow)) characterMovimentation.Move(Direction.down, Time.deltaTime);
        else if (Input.GetKey(KeyCode.LeftArrow)) characterMovimentation.Move(Direction.left, Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow)) characterMovimentation.Move(Direction.right, Time.deltaTime);
    }

}

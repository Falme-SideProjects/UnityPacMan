using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovimentation playerMovimentation;

    private void Awake()
    {
        playerMovimentation = new PlayerMovimentation();
    }

    void Update()
    {
        CheckInput();
        RefreshPlayer();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.UpArrow)) playerMovimentation.Move(Direction.up, Time.deltaTime);
        else if (Input.GetKey(KeyCode.LeftArrow)) playerMovimentation.Move(Direction.left, Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow)) playerMovimentation.Move(Direction.right, Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow)) playerMovimentation.Move(Direction.down, Time.deltaTime);
    }

    private void RefreshPlayer()
    {
        this.transform.position = playerMovimentation.GetPosition();
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovimentation playerMovimentation;

    [SerializeField] private Vector2 initialPlayerPosition;

    private void Awake()
    {
        playerMovimentation = new PlayerMovimentation();

        playerMovimentation.SetInitialPosition(initialPlayerPosition);
        playerMovimentation.ResetPosition();
    }

    void Update()
    {
        CheckInput();
        RefreshPlayer();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.UpArrow)) playerMovimentation.Move(Direction.up, Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow)) playerMovimentation.Move(Direction.down, Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow)) playerMovimentation.Move(Direction.left, Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow)) playerMovimentation.Move(Direction.right, Time.deltaTime);
    }

    private void RefreshPlayer()
    {
        this.transform.position = 
            new Vector3(playerMovimentation.GetPosition().x,
                        playerMovimentation.GetPosition().y,
                        transform.position.z);
    }

    public PlayerMovimentation GetPlayerMovimentation()
    {
        return this.playerMovimentation;
    }
}

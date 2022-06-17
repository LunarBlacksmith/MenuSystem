using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // static Vector3's will be assigned value from other scripts depending on what buttons the player clicks
    public static Vector3 playerPos;
    public static Quaternion playerRot;
    public float speed = 3f;
    public float rotateSpeed = 1.0f;
    public float smooth = 5.0f;
    public float tiltAngle = 60.0f;
   // public static string loadPath;
    void Update()
    { PlayerMove(); }

    private void Start()
    {
        // when the player starts up, we assign all the variables according the reading of a save file
        // the path it reads from is set up already based on the button clicked to load the scene
        // because that button passes a save slot index to a static string that this function reads
        GameSaves.ReadSaveFile();

        transform.position  = playerPos;    // assign player position on game loading
        transform.rotation  = playerRot;    // assign player rotation on game loading

    }
    private void PlayerMove()
    {
        // set a movement Vec2 to 0,0
        Vector2 moveDir = Vector2.zero;

        // depending on key pressed, move character at a speed that relates to the Time scale of the game
        if (Input.GetKey(KeyCode.W))
        { moveDir.y += speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S))
        { moveDir.y -= speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.D))
        { moveDir.x += speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.A))
        { moveDir.x -= speed * Time.deltaTime; }

        transform.position += (Vector3)moveDir;

        // depending on key pressed, rotate the character itself at a determined speed
        if (Input.GetKey(KeyCode.LeftArrow))
        { transform.Rotate(0, 0, rotateSpeed, Space.Self); }
        if (Input.GetKey(KeyCode.RightArrow))
        { transform.Rotate(0, 0, -rotateSpeed, Space.Self); }
    }
}
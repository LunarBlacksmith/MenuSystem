using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // static Vector3's will be assigned value from other scripts depending on what buttons the player clicks
    public static Vector3 playerPos;
    public static Vector3 playerRot;
    public float speed = 3f;
    public float rotateSpeed = 1.0f;
    public float smooth = 5.0f;
    public float tiltAngle = 60.0f;
    void Update()
    { PlayerMove(); }

    private void Awake()
    {
        transform.position  = playerPos;    // assign player position on game loading
        transform.up        = playerRot;    // assign player rotation on game loading
    }
    private void PlayerMove()
    {
        Vector2 moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        { moveDir.y += speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S))
        { moveDir.y -= speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.D))
        { moveDir.x += speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.A))
        { moveDir.x -= speed * Time.deltaTime; }

        transform.position += (Vector3)moveDir;

        if (Input.GetKey(KeyCode.LeftArrow))
        { transform.Rotate(0, 0, rotateSpeed, Space.Self); }
        if (Input.GetKey(KeyCode.RightArrow))
        { transform.Rotate(0, 0, -rotateSpeed, Space.Self); }
    }
}
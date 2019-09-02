using UnityEngine;

public static class MoveAndRotateUtil
{
    public static float speedMove = 3f;
    public static float speedRotate = 40f;
    private static KeyCode up, down, left, right;
    private static bool initial = false;
    // Update is called once per frame
    public static void Update(Transform transform)
    {
        Init();
        if (Input.GetKey(up))
        {
            transform.position += transform.forward * speedMove * Time.deltaTime;
        }
        if (Input.GetKey(down))
        {
            transform.position -= transform.forward * speedMove * Time.deltaTime;
        }
        if (Input.GetKey(left))
        {
            transform.Rotate(transform.up, -1 * speedRotate * Time.deltaTime);
        }
        if (Input.GetKey(right))
        {
            transform.Rotate(transform.up, speedRotate * Time.deltaTime);
        }
    }

    private static void Init()
    {
        if (!initial)
        {
            WASD();
            initial = true;
        }
    }

    private static void WASD()
    {
        up = KeyCode.W;
        down = KeyCode.S;
        left = KeyCode.A;
        right = KeyCode.D;
    }

    private static void Arrow()
    {
        up = KeyCode.UpArrow;
        down = KeyCode.DownArrow;
        left = KeyCode.LeftArrow;
        right = KeyCode.RightArrow;
    }
}
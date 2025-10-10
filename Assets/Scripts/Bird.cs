using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey;

public class Bird : MonoBehaviour
{
    private const float JUMP_FORCE = 100f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Keyboard.current.spaceKey.isPressed || Mouse.current.leftButton.isPressed)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = Vector2.up * JUMP_FORCE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CMDebug.TextPopupMouse("Dead!");
    }
}

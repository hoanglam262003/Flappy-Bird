using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Bird : MonoBehaviour
{
    private const float JUMP_FORCE = 100f;
    private Rigidbody2D rb;
    private static Bird instance;
    private State state;
    private enum State
    {
        WaitingToStart,
        Playing,
        Dead
    }

    public static Bird GetInstance()
    {
        return instance;
    }
    public event EventHandler Died;
    public event EventHandler StartedPlaying;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                if (Keyboard.current.spaceKey.isPressed || Mouse.current.leftButton.isPressed)
                {
                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (StartedPlaying != null) StartedPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Keyboard.current.spaceKey.isPressed || Mouse.current.leftButton.isPressed)
                {
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
        if (state != State.Playing) return;
        rb.linearVelocity = Vector2.up * JUMP_FORCE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        state = State.Dead;
        rb.bodyType = RigidbodyType2D.Static;
        if (Died != null) Died(this, EventArgs.Empty);
    }
}

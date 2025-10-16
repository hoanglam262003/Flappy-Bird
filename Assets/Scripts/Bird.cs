using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Bird : MonoBehaviour
{
    private const float JUMP_FORCE = 90f;
    private Rigidbody2D rb;
    private static Bird instance;
    private State state;
    private Animator animator;

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
        animator = GetComponent<Animator>();
        rb.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                animator.SetBool("IsFlapping", false);
                if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                {
                    state = State.Playing;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (StartedPlaying != null) StartedPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                animator.SetBool("IsFlapping", true);
                if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Jump();
                }
                transform.eulerAngles = new Vector3(0, 0, rb.linearVelocity.y * 0.2f);
                break;
            case State.Dead:
                animator.SetBool("IsFlapping", false);
                break;
        }
    }

    private void Jump()
    {
        if (state != State.Playing) return;
        rb.linearVelocity = Vector2.up * JUMP_FORCE;
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        state = State.Dead;
        rb.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        if (Died != null) Died(this, EventArgs.Empty);
    }
}

using System;
using UnityEngine;

public class Schnegge : MonoBehaviour
{
    private const float KShellGravityScale = 2f;
    private const float KPerfectBlockSpeedBoost = 2f;
    private const float KMaxSpeedSmooth = 4f;
    private const float KJumpDuration = 0.3f;

    [SerializeField] private Rigidbody2D _rigidBody;
    
    private SchneggeState[] _schneggeStates;

    public bool PerfectlyBlockedLastFrame;

    private State _state;
    private float _speedX = 2f;
    private float _timeSinceBeginningOfJump;

    private void Start()
    {
        _schneggeStates = GetComponentsInChildren<SchneggeState>();
    }
    
    private void Update()
    {
        if (IsWalking)
            _state = TransitionWalk();
        
        if (IsJumping)
            _state = TransitionJump();
        
        if (Input.GetKeyUp(KeyCode.Mouse0) && IsOnGround)
            Jump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            _state = State.Walk1;
        
        if (IsWalking)
            UpdateSpeed();

        if (IsJumping && PerfectlyBlockedLastFrame)
        {
            PerfectlyBlockedLastFrame = false;
            _speedX += KPerfectBlockSpeedBoost;
            UpdateSpeed();
        }

        UpdateVisuals();

        UpdateGravityForce();

        SmoothSpeed();
    }

    public bool IsOnGround => true;

    private State TransitionJump()
    {
        _timeSinceBeginningOfJump += Time.deltaTime;

        if (_timeSinceBeginningOfJump <= KJumpDuration)
            return _state;

        _timeSinceBeginningOfJump = 0f;
        return State.Shell;
    }

    private void Jump()
    {
        _state = State.Jump;
    }

    private void SmoothSpeed()
    {
        // TODO: make this smooth
    }

    public bool IsJumping => _state == State.Jump;

    private State TransitionWalk()
    {
        switch (_state)
        {
            case State.Walk1:
                return State.Walk2;
            case State.Walk2:
                return State.Walk1;
            default:
                throw new ArgumentOutOfRangeException("_state", _state, null);
        }
    }

    private void UpdateVisuals()
    {
        foreach (var schneggeState in _schneggeStates)
        {
            schneggeState.IsActive = schneggeState.State == _state;
        }
    }

    private void UpdateSpeed()
    {
        _rigidBody.velocity = new Vector2(_speedX, _rigidBody.velocity.y);
    }

    private void UpdateGravityForce()
    {
        _rigidBody.gravityScale = _state == State.Shell 
            ? KShellGravityScale 
            : 1;
    }

    private bool IsWalking
    {
        get
        {
            switch (_state)
            {
                case State.Shell:
                    return false;
                case State.Walk1:
                case State.Walk2:
                    return true;
                case State.Dead:
                case State.Jump:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException("_state", _state, null);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Schnegge");
    }
}
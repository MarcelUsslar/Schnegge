using System;
using UnityEngine;

public class Schnegge : MonoBehaviour
{
    private const float KShellGravityScale = 2f;
    private const float KPerfectBlockSpeedBoost = 2f;
    private const float KMaxSpeedSmooth = 4f;
    private const float KJumpDuration = 0.3f;
    private const float KWalkTransitionTime = 0.5f;
    private const float KJumpSpeed = 3f;
    private const float KBounciness = 0.4f;

    [SerializeField] private Rigidbody2D _rigidBody;
    
    private SchneggeState[] _schneggeStates;

    public bool PerfectlyBlockedLastFrame;

    private Action _walkSoundDisposable;
    private State _state;
    private float _speedX = 2f;
    private float _timeSinceBeginningOfJump;
    private float _timeSinceLastWalkingTransition;

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
        {
            _speedX = 2f;
            _rigidBody.transform.rotation = Quaternion.identity;
            _rigidBody.angularVelocity = 0f;
            _rigidBody.sharedMaterial.bounciness = 0f;
            _state = State.Walk1;
            _walkSoundDisposable = SoundService.PlaySound(Sound.Walk, true);
        }
            
        
        if (IsWalking)
            UpdateSpeed();

        if (PerfectlyBlockedLastFrame)
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
        if (_walkSoundDisposable != null)
        {
            _walkSoundDisposable();
            _walkSoundDisposable = null;
        }

        _state = State.Jump;
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, KJumpSpeed);
        _rigidBody.sharedMaterial.bounciness = KBounciness;
        _speedX /= 4;
        UpdateSpeed();
        SoundService.PlaySound(Sound.Jump);
    }

    private void SmoothSpeed()
    {
        // TODO: make this smooth
    }

    public bool IsJumping => _state == State.Jump;

    private State TransitionWalk()
    {
        _timeSinceLastWalkingTransition += Time.deltaTime;

        if (_timeSinceLastWalkingTransition <= KWalkTransitionTime)
            return _state;

        _timeSinceLastWalkingTransition = 0f;

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
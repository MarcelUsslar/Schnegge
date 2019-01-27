using System;
using System.Linq;
using UnityEngine;

public class Schnegge : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private SchneggeState[] _schneggeStates;
    [SerializeField] private PhysicsMaterial2D _shellMaterial;
    [SerializeField] private PhysicsMaterial2D _walkMaterial;
    [Space(10)]

    [SerializeField] private float _glideGravityScale = 0.2f;
    [SerializeField] private float _perfectBlockSpeedBoost = 2f;
    [SerializeField] private float _maxSpeedSmooth = 4f;
    [SerializeField] private float _jumpDuration = 0.3f;
    [SerializeField] private float _jumpSpeed = 3f;
    [SerializeField] private float _defaultSpeed = 5f;
    [SerializeField] private float _defaultGravity = 1f;

    private Action _walkSoundDisposable;
    private float _speedX;
    private float _timeSinceBeginningOfJump;
    private bool _isOnGround = true;

    private State State
    {
        get { return _schneggeStates.First(state => state.IsActive).State; }
        set
        {
            foreach (var state in _schneggeStates)
            {
                state.IsActive = state.State == value;
            }
        }
    }

    private void Start()
    {
        State = State.Shell;
    }
    
    private void Update()
    {
        if (IsJumping)
            EvaluateJumpTime();

        if (MousePressedUp)
            TryJumping();

        if (MousePressedDown)
            TryWalking();
        
        if (IsWalking)
            VelocityX = _speedX;
        
        Debug.Log(VelocityX);

        SmoothSpeed();

        if (IsWalking)
            ClampRotation(0.375f);
    }

    private void TryJumping()
    {
        if (_isOnGround)
            Jump();
        else
            Shield();
    }

    private void Jump()
    {
        _isOnGround = false;

        VelocityY = _jumpSpeed;
        VelocityX /= 4;

        SoundService.PlaySound(Sound.Jump);

        Shield();
    }

    private void Shield()
    {
        WalkSoundDisposable = null;

        State = State.Jump;

        _rigidBody.sharedMaterial = _shellMaterial;
        _rigidBody.gravityScale = _defaultGravity;
    }

    private void TryWalking()
    {
        if (_isOnGround)
            Walk();
        else
            Glide();
    }

    private void Walk()
    {
        OnLanding();

        ResetShellPhysics();
    }

    private void OnLanding()
    {
        Debug.LogWarning("OnLanding");
        State = State.Walk;
        WalkSoundDisposable = SoundService.PlaySound(Sound.Walk, true);
        VelocityX = _defaultSpeed;
    }
    
    private void Glide()
    {
        _rigidBody.gravityScale = _glideGravityScale;
        State = State.Glide;

        VelocityX = VelocityX;

        ResetShellPhysics();
    }

    private void ResetShellPhysics()
    {
        _rigidBody.transform.rotation = Quaternion.identity;
        _rigidBody.angularVelocity = 0f;
        _rigidBody.sharedMaterial = _walkMaterial;
    }

    private void ClampRotation(float boundary)
    {
        var rot = transform.rotation;
        var rotZClamped = Mathf.Clamp(rot.z, -boundary, boundary);
        transform.rotation = new Quaternion(rot.x, rot.y, rotZClamped, rot.w);
    }

    private void EvaluateJumpTime()
    {
        _timeSinceBeginningOfJump += Time.deltaTime;

        if (_timeSinceBeginningOfJump <= _jumpDuration)
            return;

        _timeSinceBeginningOfJump = 0f;
        State = State.Shell;
    }

    private void SmoothSpeed()
    {
        // TODO: make this smooth
    }

    public bool IsJumping => State == State.Jump;
    private bool IsWalking => State == State.Walk;
    private bool IsShielding => State == State.Shell;
    private bool IsGliding => State == State.Glide;

    private bool MousePressedDown => Input.GetMouseButtonDown(0);
    private bool MousePressedUp => Input.GetMouseButtonUp(0);

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsGliding)
            OnLanding();

        _isOnGround = !IsJumping;

        var danger = other.gameObject.GetComponent<Danger>();
        
        if (danger == null)
            danger = other.gameObject.GetComponentInParent<Danger>();

        if (danger != null)
        {
            SoundService.PlaySound(Sound.Danger);

            switch (State)
            {
                case State.Shell:
                    Block(danger);
                    break;
                case State.Walk:
                case State.Glide:
                    Danger(danger);
                    break;
                case State.Jump:
                    PerfectBlock(danger);
                    break;
            }
        }
        else
        {
            _rigidBody.gravityScale = _defaultGravity;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isOnGround = false;
    }

    private void Danger(Danger danger)
    {
        SoundService.PlaySound(Sound.Hit);
        State = State.Dead;
    }

    private void PerfectBlock(Danger danger)
    {
        VelocityX += _perfectBlockSpeedBoost;
        Block(danger);
    }

    private void Block(Danger danger)
    {
        SoundService.PlaySound(Sound.Block);
    }

    private Action WalkSoundDisposable
    {
        set
        {
            _walkSoundDisposable?.Invoke();
            _walkSoundDisposable = value;
        }
    }

    private float VelocityX
    {
        get { return _rigidBody.velocity.x; }
        set
        {
            _speedX = value;
            _rigidBody.velocity = new Vector2( value, VelocityY);
        }
    }

    private float VelocityY
    {
        get { return _rigidBody.velocity.y; }
        set { _rigidBody.velocity = new Vector2(VelocityX, value); }
    }
}
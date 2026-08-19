using System;
using EZCameraShake;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerJumpSmokeFx;

    public GameObject footstepFx;

    public Transform playerCam;

    public Transform orientation;

    private Rigidbody rb;

    public bool dead;

    public bool exploded;

    private float moveSpeed = 6000f;

    private float maxSpeed = 22f;

    public bool grounded;

    public LayerMask whatIsGround;

    private Vector3 crouchScale = new Vector3(1f, 1.05f, 1f);

    private Vector3 playerScale;

    private float slideForce = 800f;

    private float slideCounterMovement = 0.12f;

    private bool readyToJump = true;

    private float jumpCooldown = 0.25f;

    private float jumpForce = 13f;

    private float x;

    private float y;

    private float mouseDeltaX;

    private float mouseDeltaY;

    private bool jumping;

    private bool sliding;

    private bool crouching;

    private Vector3 normalVector;

    public ParticleSystem ps;

    private ParticleSystem.EmissionModule psEmission;

    private Collider playerCollider;

    private float fallSpeed;

    private Vector3 lastMoveSpeed;

    private float playerHeight;

    public GameObject playerSmokeFx;

    public AlideAudio slideAudio;

    private float distance;

    private int ticks;

    private bool onRamp;

    public bool simulate;

    public bool secondJump = true;

    [HideInInspector]
    public int jumpsLeft = 1;

    [HideInInspector]
    public int maxJumps = 1;

    private int resetJumpCounter;

    private int jumpCounterResetTime = 10;

    private float counterMovement = 0.14f;

    private float threshold = 0.01f;

    private int readyToCounterX;

    private int readyToCounterY;

    private bool cancelling;

    private float maxSlopeAngle = 35f;

    private bool airborne;

    private bool onGround;

    private bool surfing;

    private bool cancellingGrounded;

    private bool cancellingSurf;

    private float delay = 5f;

    private int groundCancel;

    private int wallCancel;

    private int surfCancel;

    public LayerMask whatIsHittable;

    private float vel;

    public static PlayerMovement Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().bounds.size.y;
    }

    public void Start()
    {
        playerScale = base.transform.localScale;
        playerCollider = GetComponent<Collider>();
        psEmission = ps.emission;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraShake();
    }

    public void Update()
    {
        if (!dead)
        {
            FootSteps();
            fallSpeed = rb.velocity.y;
            lastMoveSpeed = VectorExtensions.XZVector(rb.velocity);
        }
    }

    public void SetInput(Vector2 _dir, bool _crouching, bool _jumping)
    {
        x = _dir.x;
        y = _dir.y;
        crouching = _crouching;
        jumping = _jumping;
    }

    private void CheckInput()
    {
        if (crouching && !sliding)
        {
            StartCrouch();
        }
        if (!crouching && sliding)
        {
            StopCrouch();
        }
    }

    public void StartCrouch()
    {
        if (!sliding)
        {
            sliding = true;
            base.transform.localScale = crouchScale;
            base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.65f, base.transform.position.z);
            if (rb.velocity.magnitude > 0.5f && grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
                slideAudio.PlayStartSlide();
            }
        }
    }

    public void StopCrouch()
    {
        sliding = false;
        base.transform.localScale = playerScale;
        base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.65f, base.transform.position.z);
    }

    private void FootSteps()
    {
        if (!crouching && !dead && grounded)
        {
            const float num = 1f;
            float num2 = rb.velocity.magnitude;
            if (num2 > 20f)
            {
                num2 = 20f;
            }
            distance += num2 * Time.deltaTime * 50f;
            if (distance > 300f / num)
            {
                UnityEngine.Object.Instantiate(footstepFx, base.transform.position, Quaternion.identity);
                distance = 0f;
            }
        }
    }

    public void Movement(float _x, float _y)
    {
        UpdateCollisionChecks();
        SpeedLines();
        x = _x;
        y = _y;
        if (dead)
        {
            return;
        }
        CheckInput();
        if (!grounded)
        {
            rb.AddForce(Vector3.down * 2f);
        }
        Vector2 _velocity = FindVelRelativeToLook();
        CounterMovement(x, y, _velocity);
        RampMovement(_velocity);
        if (readyToJump && jumping)
        {
            Jump();
        }
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * 60f);
            return;
        }
        float _xNormal = x;
        float _yNormal = y;
        if (x > 0f && _velocity.x > maxSpeed)
        {
            _xNormal = 0f;
        }
        if (x < 0f && _velocity.x < -maxSpeed)
        {
            _xNormal = 0f;
        }
        if (y > 0f && _velocity.y > maxSpeed)
        {
            _yNormal = 0f;
        }
        if (y < 0f && _velocity.y < -maxSpeed)
        {
            _yNormal = 0f;
        }
        float _strafeMultiplier = 1f;
        float _forwardMultiplier = 1f;
        if (!grounded)
        {
            _strafeMultiplier = 0.6f;
            _forwardMultiplier = 0.6f;
            if (IsHoldingAgainstVerticalVel(_velocity))
            {
                float _forwardMulti2 = Mathf.Abs(_velocity.y * 0.025f);
                if (_forwardMulti2 < 0.5f)
                {
                    _forwardMulti2 = 0.5f;
                }
                _forwardMultiplier = Mathf.Abs(_forwardMulti2);
            }
        }
        if (grounded && crouching)
        {
            _forwardMultiplier = 0f;
        }
        if (surfing)
        {
            _strafeMultiplier = 0.6f;
            _forwardMultiplier = 0.3f;
        }
        const float num8 = 0.01f;
        rb.AddForce(orientation.forward * _yNormal * moveSpeed * 0.02f * _forwardMultiplier);
        rb.AddForce(orientation.right * _xNormal * moveSpeed * 0.02f * _strafeMultiplier);
        if (!grounded)
        {
            if (_xNormal != 0f)
            {
                rb.AddForce(-orientation.forward * _velocity.y * moveSpeed * 0.02f * num8);
            }
            if (_yNormal != 0f)
            {
                rb.AddForce(-orientation.right * _velocity.x * moveSpeed * 0.02f * num8);
            }
        }
        if (!readyToJump)
        {
            resetJumpCounter++;
            if (resetJumpCounter >= jumpCounterResetTime)
            {
                ResetJump();
            }
        }
    }

    private void RampMovement(Vector2 mag)
    {
        if (grounded && onRamp && !surfing && !crouching && !jumping && Math.Abs(x) < 0.05f && Math.Abs(y) < 0.05f)
        {
            rb.useGravity = false;
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);
            }
            else if (rb.velocity.y <= 0f && Math.Abs(mag.magnitude) < 1f)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void SpeedLines()
    {
        float _angle = Vector3.Angle(rb.velocity, playerCam.transform.forward) * 0.15f;
        if (_angle < 1f)
        {
            _angle = 1f;
        }
        float _multiplier = rb.velocity.magnitude / _angle;
        if (grounded)
        {
            _multiplier = 0f;
        }
        psEmission.rateOverTimeMultiplier = _multiplier;
    }

    private void CameraShake()
    {
        float _magnitude = rb.velocity.magnitude / 9f;
        CameraShaker.Instance.ShakeOnce(_magnitude, 0.1f * _magnitude, 0.25f, 0.2f);
        Invoke("CameraShake", 0.2f);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void Jump()
    {
        if ((grounded || surfing || jumpsLeft > 0) && readyToJump && secondJump)
        {
            readyToJump = false;
            jumpsLeft--;
            resetJumpCounter = 0;
            secondJump = false;
            rb.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode.Impulse);
            rb.AddForce(normalVector * jumpForce * 0.5f, ForceMode.Impulse);
            Vector3 _velocity = rb.velocity;
            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(_velocity.x, 0f, _velocity.z);
            }
            else if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector3(_velocity.x, 0f, _velocity.z);
            }
            Quaternion _up = Quaternion.LookRotation(Vector3.up);
            GameObject _obj = UnityEngine.Object.Instantiate(playerJumpSmokeFx, base.transform.position, _up);
            ParticleSystem.VelocityOverLifetimeModule _velocityOverLifetime = _obj.GetComponent<ParticleSystem>().velocityOverLifetime;
            _velocityOverLifetime.x = rb.velocity.x * 2f;
            _velocityOverLifetime.z = rb.velocity.z * 2f;
        }
    }

    private void CounterMovement(float _x, float _y, Vector2 _mag)
    {
        if (!grounded || jumping || exploded)
        {
            return;
        }
        if (crouching)
        {
            rb.AddForce(moveSpeed * 0.02f * -rb.velocity.normalized * slideCounterMovement);
            return;
        }
        if (Math.Abs(_mag.x) > threshold && Math.Abs(_x) < 0.05f && readyToCounterX > 1)
        {
            rb.AddForce(moveSpeed * orientation.transform.right * 0.02f * (-_mag.x) * counterMovement);
        }
        if (Math.Abs(_mag.y) > threshold && Math.Abs(_y) < 0.05f && readyToCounterY > 1)
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * 0.02f * (-_mag.y) * counterMovement);
        }
        if (IsHoldingAgainstHorizontalVel(_mag))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * 0.02f * (-_mag.x) * counterMovement * 2f);
        }
        if (IsHoldingAgainstVerticalVel(_mag))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * 0.02f * (-_mag.y) * counterMovement * 2f);
        }
        if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2f) + Mathf.Pow(rb.velocity.z, 2f)) > maxSpeed)
        {
            Vector3 _velocity = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(_velocity.x, rb.velocity.y, _velocity.z);
        }
        if (Math.Abs(_x) < 0.05f)
        {
            readyToCounterX++;
        }
        else
        {
            readyToCounterX = 0;
        }
        if (Math.Abs(_y) < 0.05f)
        {
            readyToCounterY++;
        }
        else
        {
            readyToCounterY = 0;
        }
    }

    private bool IsHoldingAgainstHorizontalVel(Vector2 _vel)
    {
        if (!(_vel.x < -threshold) || !(x > 0f))
        {
            if (_vel.x > threshold)
            {
                return x < 0f;
            }
            return false;
        }
        return true;
    }

    private bool IsHoldingAgainstVerticalVel(Vector2 _vel)
    {
        if (!(_vel.y < -threshold) || !(y > 0f))
        {
            if (_vel.y > threshold)
            {
                return y < 0f;
            }
            return false;
        }
        return true;
    }

    public Vector2 FindVelRelativeToLook()
    {
        float _current = orientation.transform.eulerAngles.y;
        float _target = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 57.29578f;
        float _deltaAngle = Mathf.DeltaAngle(_current, _target);
        float _magnitude = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        float _x = Mathf.Cos((90f - _deltaAngle) * (Mathf.PI / 180f));
        float _y = Mathf.Cos(_deltaAngle * (Mathf.PI / 180f));
        return new Vector2(_x, _y) * _magnitude;
    }

    private bool IsFloor(Vector3 _vector)
    {
        return Vector3.Angle(Vector3.up, _vector) < maxSlopeAngle;
    }

    private bool IsSurf(Vector3 _vector)
    {
        float num = Vector3.Angle(Vector3.up, _vector);
        if (num < 89f)
        {
            return num > maxSlopeAngle;
        }
        return false;
    }

    private bool IsWall(Vector3 _vector)
    {
        return Math.Abs(90f - Vector3.Angle(Vector3.up, _vector)) < 0.1f;
    }

    public bool IsRoof(Vector3 _vector)
    {
        return _vector.y == -1f;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!SameGround(other))
        {
            return;
        }
        Vector3 _contactNormal = other.contacts[0].normal;
        if (IsFloor(_contactNormal))
        {
            jumpsLeft = maxJumps;
            secondJump = true;
            MoveCamera.Instance.BobOnce(new Vector3(0f, fallSpeed, 0f));
            if (fallSpeed < -15f)
            {
                Vector3 _contact = other.contacts[0].point;
                Quaternion _rotation = Quaternion.LookRotation(base.transform.position - _contact);
                var _newObject = UnityEngine.Object.Instantiate(playerSmokeFx, _contact, _rotation);
                ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = _newObject.GetComponent<ParticleSystem>().velocityOverLifetime;
                velocityOverLifetime.x = rb.velocity.x * 2f;
                velocityOverLifetime.z = rb.velocity.z * 2f;
            }
        }
        const float _maxRayDistance = 1.3f;
        if (IsWall(_contactNormal))
        {
            Vector3 _speedNormal = lastMoveSpeed.normalized;
            Vector3 _vector = base.transform.position + (Vector3.up * 1.6f);
            UnityEngine.Debug.DrawLine(_vector, _vector + (_speedNormal * _maxRayDistance), Color.blue, 10f);
            if (!Physics.Raycast(_vector, _speedNormal, _maxRayDistance, whatIsGround) && Physics.Raycast(_vector + (_speedNormal * _maxRayDistance), Vector3.down, out RaycastHit hitInfo, 3f, whatIsGround))
            {
                Vector3 _collision = hitInfo.point + (Vector3.up * playerHeight * 0.5f);
                MoveCamera.Instance.vaultOffset += base.transform.position - _collision;
                base.transform.position = _collision;
                rb.velocity = lastMoveSpeed * 0.4f;
                jumpsLeft = maxJumps;
            }
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (!SameGround(other))
        {
            return;
        }
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 _contactNormal = other.contacts[i].normal;
            if (IsFloor(_contactNormal))
            {
                if (!grounded && crouching)
                {
                    slideAudio.PlayStartSlide();
                }
                onRamp = Vector3.Angle(Vector3.up, _contactNormal) > 1f;
                grounded = true;
                normalVector = _contactNormal;
                cancellingGrounded = false;
                groundCancel = 0;
            }
            if (IsSurf(_contactNormal))
            {
                surfing = true;
                cancellingSurf = false;
                surfCancel = 0;
            }
        }
    }

    private bool SameGround(Collision other)
    {
        int _layer = other.gameObject.layer;
        int _groundMask = (int)whatIsGround;
        return _groundMask == (_groundMask | (1 << _layer));
    }

    private void UpdateCollisionChecks()
    {
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
        }
        else
        {
            groundCancel++;
            if (groundCancel > delay)
            {
                StopGrounded();
            }
        }
        if (!cancellingSurf)
        {
            cancellingSurf = true;
            surfCancel = 1;
            return;
        }
        surfCancel++;
        if (surfCancel > delay)
        {
            StopSurf();
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    private void StopSurf()
    {
        surfing = false;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    public float GetFallSpeed()
    {
        return rb.velocity.y;
    }

    public Collider GetPlayerCollider()
    {
        return playerCollider;
    }

    public Transform GetPlayerCamTransform()
    {
        return playerCam.transform;
    }

    public Vector3 HitPoint()
    {
        RaycastHit[] _hits = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward, 100f, whatIsHittable);
        if (_hits.Length < 1)
        {
            return playerCam.transform.position + (playerCam.transform.forward * 100f);
        }
        if (_hits.Length > 1)
        {
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy") || _hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Object") || _hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    return _hits[i].point;
                }
            }
        }
        return _hits[0].point;
    }

    public bool IsCrouching() => crouching;
    public bool IsDead() => dead;
    public Rigidbody GetRb() => rb;
}

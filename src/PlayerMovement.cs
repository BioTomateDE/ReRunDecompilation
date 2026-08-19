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

    public void SetInput(Vector2 dir, bool crouching, bool jumping)
    {
        x = dir.x;
        y = dir.y;
        this.crouching = crouching;
        this.jumping = jumping;
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

    public void Movement(float x, float y)
    {
        UpdateCollisionChecks();
        SpeedLines();
        this.x = x;
        this.y = y;
        if (dead)
        {
            return;
        }
        CheckInput();
        if (!grounded)
        {
            rb.AddForce(Vector3.down * 2f);
        }
        Vector2 mag = FindVelRelativeToLook();
        float num = mag.x;
        float num2 = mag.y;
        CounterMovement(x, y, mag);
        RampMovement(mag);
        if (readyToJump && jumping)
        {
            Jump();
        }
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * 60f);
            return;
        }
        float num3 = x;
        float num4 = y;
        if (x > 0f && num > maxSpeed)
        {
            num3 = 0f;
        }
        if (x < 0f && num < 0f - maxSpeed)
        {
            num3 = 0f;
        }
        if (y > 0f && num2 > maxSpeed)
        {
            num4 = 0f;
        }
        if (y < 0f && num2 < 0f - maxSpeed)
        {
            num4 = 0f;
        }
        float num5 = 1f;
        float num6 = 1f;
        if (!grounded)
        {
            num5 = 0.6f;
            num6 = 0.6f;
            if (IsHoldingAgainstVerticalVel(mag))
            {
                float num7 = Mathf.Abs(mag.y * 0.025f);
                if (num7 < 0.5f)
                {
                    num7 = 0.5f;
                }
                num6 = Mathf.Abs(num7);
            }
        }
        if (grounded && crouching)
        {
            num6 = 0f;
        }
        if (surfing)
        {
            num5 = 0.6f;
            num6 = 0.3f;
        }
        const float num8 = 0.01f;
        rb.AddForce(orientation.forward * num4 * moveSpeed * 0.02f * num6);
        rb.AddForce(orientation.right * num3 * moveSpeed * 0.02f * num5);
        if (!grounded)
        {
            if (num3 != 0f)
            {
                rb.AddForce(-orientation.forward * mag.y * moveSpeed * 0.02f * num8);
            }
            if (num4 != 0f)
            {
                rb.AddForce(-orientation.right * mag.x * moveSpeed * 0.02f * num8);
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
        float num = Vector3.Angle(rb.velocity, playerCam.transform.forward) * 0.15f;
        if (num < 1f)
        {
            num = 1f;
        }
        float rateOverTimeMultiplier = rb.velocity.magnitude / num;
        if (grounded)
        {
            rateOverTimeMultiplier = 0f;
        }
        psEmission.rateOverTimeMultiplier = rateOverTimeMultiplier;
    }

    private void CameraShake()
    {
        float num = rb.velocity.magnitude / 9f;
        CameraShaker.Instance.ShakeOnce(num, 0.1f * num, 0.25f, 0.2f);
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
            Vector3 velocity = rb.velocity;
            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
            }
            else if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
            }
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = UnityEngine.Object.Instantiate(playerJumpSmokeFx, base.transform.position, Quaternion.LookRotation(Vector3.up)).GetComponent<ParticleSystem>().velocityOverLifetime;
            velocityOverLifetime.x = rb.velocity.x * 2f;
            velocityOverLifetime.z = rb.velocity.z * 2f;
        }
    }

    private void CounterMovement(float x, float y, Vector2 mag)
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
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f && readyToCounterX > 1)
        {
            rb.AddForce(moveSpeed * orientation.transform.right * 0.02f * (0f - mag.x) * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f && readyToCounterY > 1)
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * 0.02f * (0f - mag.y) * counterMovement);
        }
        if (IsHoldingAgainstHorizontalVel(mag))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * 0.02f * (0f - mag.x) * counterMovement * 2f);
        }
        if (IsHoldingAgainstVerticalVel(mag))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * 0.02f * (0f - mag.y) * counterMovement * 2f);
        }
        if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2f) + Mathf.Pow(rb.velocity.z, 2f)) > maxSpeed)
        {
            float num = rb.velocity.y;
            Vector3 vector = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(vector.x, num, vector.z);
        }
        if (Math.Abs(x) < 0.05f)
        {
            readyToCounterX++;
        }
        else
        {
            readyToCounterX = 0;
        }
        if (Math.Abs(y) < 0.05f)
        {
            readyToCounterY++;
        }
        else
        {
            readyToCounterY = 0;
        }
    }

    private bool IsHoldingAgainstHorizontalVel(Vector2 vel)
    {
        if (!(vel.x < 0f - threshold) || !(x > 0f))
        {
            if (vel.x > threshold)
            {
                return x < 0f;
            }
            return false;
        }
        return true;
    }

    private bool IsHoldingAgainstVerticalVel(Vector2 vel)
    {
        if (!(vel.y < 0f - threshold) || !(y > 0f))
        {
            if (vel.y > threshold)
            {
                return y < 0f;
            }
            return false;
        }
        return true;
    }

    public Vector2 FindVelRelativeToLook()
    {
        float current = orientation.transform.eulerAngles.y;
        float target = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 57.29578f;
        float num = Mathf.DeltaAngle(current, target);
        float num2 = 90f - num;
        float magnitude = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        return new Vector2(x: magnitude * Mathf.Cos(num2 * ((float)Math.PI / 180f)), y: magnitude * Mathf.Cos(num * ((float)Math.PI / 180f)));
    }

    private bool IsFloor(Vector3 v)
    {
        return Vector3.Angle(Vector3.up, v) < maxSlopeAngle;
    }

    private bool IsSurf(Vector3 v)
    {
        float num = Vector3.Angle(Vector3.up, v);
        if (num < 89f)
        {
            return num > maxSlopeAngle;
        }
        return false;
    }

    private bool IsWall(Vector3 v)
    {
        return Math.Abs(90f - Vector3.Angle(Vector3.up, v)) < 0.1f;
    }

    private bool IsRoof(Vector3 v)
    {
        return v.y == -1f;
    }

    public void OnCollisionEnter(Collision other)
    {
        int layer = other.gameObject.layer;
        Vector3 normal = other.contacts[0].normal;
        if ((int)whatIsGround != ((int)whatIsGround | (1 << layer)))
        {
            return;
        }
        if (IsFloor(normal))
        {
            jumpsLeft = maxJumps;
            secondJump = true;
            MoveCamera.Instance.BobOnce(new Vector3(0f, fallSpeed, 0f));
            if (fallSpeed < -15f)
            {
                Vector3 point = other.contacts[0].point;
                ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = UnityEngine.Object.Instantiate(playerSmokeFx, point, Quaternion.LookRotation(base.transform.position - point)).GetComponent<ParticleSystem>().velocityOverLifetime;
                velocityOverLifetime.x = rb.velocity.x * 2f;
                velocityOverLifetime.z = rb.velocity.z * 2f;
            }
        }
        const float num = 1.3f;
        if (IsWall(normal))
        {
            Vector3 normalized = lastMoveSpeed.normalized;
            Vector3 vector = base.transform.position + (Vector3.up * 1.6f);
            UnityEngine.Debug.DrawLine(vector, vector + (normalized * num), Color.blue, 10f);
            if (!Physics.Raycast(vector, normalized, num, whatIsGround) && Physics.Raycast(vector + (normalized * num), Vector3.down, out var hitInfo, 3f, whatIsGround))
            {
                Vector3 vector2 = hitInfo.point + (Vector3.up * playerHeight * 0.5f);
                MoveCamera.Instance.vaultOffset += base.transform.position - vector2;
                base.transform.position = vector2;
                rb.velocity = lastMoveSpeed * 0.4f;
                jumpsLeft = maxJumps;
            }
        }
    }

    public void OnCollisionStay(Collision other)
    {
        int layer = other.gameObject.layer;
        if ((int)whatIsGround != ((int)whatIsGround | (1 << layer)))
        {
            return;
        }
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloor(normal))
            {
                if (!grounded && crouching)
                {
                    slideAudio.PlayStartSlide();
                }
                onRamp = Vector3.Angle(Vector3.up, normal) > 1f;
                grounded = true;
                normalVector = normal;
                cancellingGrounded = false;
                groundCancel = 0;
            }
            if (IsSurf(normal))
            {
                surfing = true;
                cancellingSurf = false;
                surfCancel = 0;
            }
        }
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
            if ((float)groundCancel > delay)
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
        if ((float)surfCancel > delay)
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
        RaycastHit[] array = Physics.RaycastAll(playerCam.transform.position, playerCam.transform.forward, 100f, whatIsHittable);
        if (array.Length < 1)
        {
            return playerCam.transform.position + (playerCam.transform.forward * 100f);
        }
        if (array.Length > 1)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy") || array[i].transform.gameObject.layer == LayerMask.NameToLayer("Object") || array[i].transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    return array[i].point;
                }
            }
        }
        return array[0].point;
    }

    public bool IsCrouching()
    {
        return crouching;
    }

    public bool IsDead()
    {
        return dead;
    }

    public Rigidbody GetRb()
    {
        return rb;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Rigidbody rb;

    private Transform playerCam;

    private Vector3 startPos;

    private List<Vector3> velHistory;

    private Vector3 desiredBob;

    private float xBob = 0.12f;

    private float yBob = 0.08f;

    private float zBob = 0.1f;

    private float bobSpeed = 0.45f;

    private Vector3 recoilOffset;

    private Vector3 recoilRotation;

    private Vector3 recoilOffsetVel;

    private Vector3 recoilRotVel;

    private float reloadRotation;

    private float desiredReloadRotation;

    private float reloadTime;

    private float rVel;

    private float reloadPosOffset;

    private float rPVel;

    private float gunDrag = 0.2f;

    public float currentGunDragMultiplier = 1f;

    private float desX;

    private float desY;

    private Vector3 speedBob;

    private float reloadProgress;

    private int spins;

    public static Gun Instance { get; set; }

    public void Start()
    {
        Instance = this;
        velHistory = new List<Vector3>();
        startPos = base.transform.localPosition;
        rb = PlayerMovement.Instance.GetRb();
        playerCam = PlayerMovement.Instance.playerCam;
    }

    public void Update()
    {
        if (PlayerMovement.Instance && !GameManager.Instance.playerDead && !GameManager.Instance.paused && GameManager.Instance.playing)
        {
            MovementBob();
            ReloadGun();
            RecoilGun();
            SpeedBob();
            float _mouseX = (-Input.GetAxis("Mouse X")) * gunDrag * currentGunDragMultiplier;
            float _mouseY = (-Input.GetAxis("Mouse Y")) * gunDrag * currentGunDragMultiplier;
            desX = Mathf.Lerp(desX, _mouseX, Time.unscaledDeltaTime * 10f);
            desY = Mathf.Lerp(desY, _mouseY, Time.unscaledDeltaTime * 10f);
            Rotation(new Vector2(desX, desY));
            Vector3 _vector = startPos + new Vector3(desX, desY, 0f) + desiredBob + recoilOffset + new Vector3(0f, -reloadPosOffset, 0f) + speedBob;
            base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, _vector, Time.unscaledDeltaTime * 15f);
        }
    }

    private void Rotation(Vector2 offset)
    {
        float _magnitude = offset.magnitude * 0.03f;
        if (offset.x < 0f)
        {
            _magnitude = -_magnitude;
        }
        Vector3 euler = new Vector3((offset.y * 80f) + reloadRotation, (-offset.x) * 40f, _magnitude * 50f) + recoilRotation;
        try
        {
            if (!(Time.deltaTime <= 0f))
            {
                base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.Euler(euler), Time.deltaTime * 20f);
            }
        }
        catch (Exception)
        {
            // very good code dani
        }
    }

    public void SpawnGun()
    {
        base.transform.localPosition += Vector3.down * 2f;
        base.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void MovementBob()
    {
        if (rb)
        {
            if (Mathf.Abs(rb.velocity.magnitude) < 4f || !PlayerMovement.Instance.grounded || PlayerMovement.Instance.IsCrouching())
            {
                desiredBob = Vector3.zero;
                return;
            }
            float x = Mathf.PingPong(Time.time * bobSpeed, xBob) - (xBob / 2f);
            float y = Mathf.PingPong(Time.time * bobSpeed, yBob) - (yBob / 2f);
            float z = Mathf.PingPong(Time.time * bobSpeed, zBob) - (zBob / 2f);
            desiredBob = new Vector3(x, y, z);
        }
    }

    private void SpeedBob()
    {
        Vector2 _playerVelocity = PlayerMovement.Instance.FindVelRelativeToLook();
        Vector3 _vector = new(_playerVelocity.x, PlayerMovement.Instance.GetVelocity().y, _playerVelocity.y);
        _vector *= -0.01f;
        _vector = Vector3.ClampMagnitude(_vector, 0.6f);
        speedBob = Vector3.Lerp(speedBob, _vector, Time.deltaTime * 10f);
    }

    public void Shoot()
    {
        const float _rotationMultiplier = 1.5f;
        recoilOffset += -(Vector3.forward + (Vector3.up * 0.3f) - (Vector3.right * 0.35f));
        recoilRotation += -new Vector3(90f, UnityEngine.Random.Range(10f, 30f), UnityEngine.Random.Range(-50f, 50f)) * _rotationMultiplier;
    }

    private void RecoilGun()
    {
        recoilOffset = Vector3.SmoothDamp(recoilOffset, Vector3.zero, ref recoilOffsetVel, 0.05f);
        recoilRotation = Vector3.SmoothDamp(recoilRotation, Vector3.zero, ref recoilRotVel, 0.07f);
    }

    private void ReloadGun()
    {
        reloadProgress += Time.deltaTime;
        reloadRotation = Mathf.Lerp(0f, desiredReloadRotation, reloadProgress / reloadTime);
        reloadPosOffset = Mathf.SmoothDamp(reloadPosOffset, 0f, ref rPVel, reloadTime * 0.2f);
        if (reloadRotation / 360f > spins)
        {
            spins++;
        }
    }

    public void Reload(float _time, int _spinAmount)
    {
        reloadProgress = 0f;
        reloadRotation = 0f;
        reloadTime = _time;
        spins = 0;
        if (_spinAmount < 1)
        {
            _spinAmount = Mathf.RoundToInt(_time * 3f);
        }
        desiredReloadRotation = -360 * _spinAmount;
        reloadPosOffset = 0.45f;
        MonoBehaviour.print("rel time on gun: " + reloadTime);
    }

    public void ChangeWeapon()
    {
        base.transform.localPosition = Vector3.down * 1f;
        Reload(1f, 0);
    }
}

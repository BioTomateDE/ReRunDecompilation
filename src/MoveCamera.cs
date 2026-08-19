using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;

    public Vector3 desyncOffset;

    public Vector3 vaultOffset;

    private Camera cam;

    private Rigidbody rb;

    public PlayerInput playerInput;

    public bool cinematic;

    private float desiredTilt;

    private float tilt;

    private Vector3 desiredBob;

    private Vector3 bobOffset;

    private float bobSpeed = 15f;

    private float bobMultiplier = 1f;

    private readonly float bobConstant = 0.2f;

    public Camera mainCam;

    public static MoveCamera Instance { get; private set; }

    public void Start()
    {
        Instance = this;
        cam = base.transform.GetChild(0).GetComponent<Camera>();
        rb = PlayerMovement.Instance.GetRb();
        if (GameState.Instance)
        {
            GameState.Instance.ApplySettings();
        }
    }

    public void LateUpdate()
    {
        UpdateBob();
        MoveGun();
        base.transform.position = player.transform.position + bobOffset + desyncOffset + vaultOffset + offset;
        if (!cinematic)
        {
            Vector3 _cameraRot = playerInput.cameraRot;
            _cameraRot.x = Mathf.Clamp(_cameraRot.x, -90f, 90f);
            base.transform.rotation = Quaternion.Euler(_cameraRot);
            desyncOffset = Vector3.Lerp(desyncOffset, Vector3.zero, Time.deltaTime * 15f);
            vaultOffset = Vector3.Slerp(vaultOffset, Vector3.zero, Time.deltaTime * 7f);
            if (PlayerMovement.Instance.IsCrouching())
            {
                desiredTilt = 6f;
            }
            else
            {
                desiredTilt = 0f;
            }
            tilt = Mathf.Lerp(tilt, desiredTilt, Time.deltaTime * 8f);
            Vector3 _eulerAngles = base.transform.rotation.eulerAngles;
            _eulerAngles.z = tilt;
            base.transform.rotation = Quaternion.Euler(_eulerAngles);
        }
    }

    private void MoveGun()
    {
        if (rb && !(Mathf.Abs(rb.velocity.magnitude) < 4f) && PlayerMovement.Instance.grounded)
        {
            PlayerMovement.Instance.IsCrouching();
        }
    }

    public void UpdateFov(float _fov)
    {
        mainCam.fieldOfView = _fov;
    }

    public void BobOnce(Vector3 _bobDirection)
    {
        Vector3 vector = ClampVector(_bobDirection * 0.15f, -3f, 3f);
        desiredBob = vector * bobMultiplier;
    }

    private void UpdateBob()
    {
        desiredBob = Vector3.Lerp(desiredBob, Vector3.zero, Time.deltaTime * bobSpeed * 0.5f);
        bobOffset = Vector3.Lerp(bobOffset, desiredBob, Time.deltaTime * bobSpeed);
    }

    private static Vector3 ClampVector(Vector3 vec, float min, float max)
    {
        return new Vector3(Mathf.Clamp(vec.x, min, max), Mathf.Clamp(vec.y, min, max), Mathf.Clamp(vec.z, min, max));
    }
}

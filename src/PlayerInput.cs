using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
	private float xRotation;

	private float sensitivity = 50f;

	private float sensMultiplier = 1f;

	private float desiredX;

	private float x;

	private float y;

	private bool jumping;

	private bool crouching;

	private Transform playerCam;

	private Transform orientation;

	private PlayerMovement playerMovement;

	public bool active = true;

	private float actualWallRotation;

	private float wallRotationVel;

	public Vector3 cameraRot;

	private float wallRunRotation;

	public float mouseOffsetY;

	public static PlayerInput Instance { get; set; }

	private void Awake()
	{
		Instance = this;
		playerMovement = (PlayerMovement)GetComponent("PlayerMovement");
		playerCam = playerMovement.playerCam;
		orientation = playerMovement.orientation;
	}

	public void StopCinematic(float x)
	{
		active = true;
		xRotation = x;
	}

	private void Update()
	{
		if (active && !GameManager.Instance.playerDead && !GameManager.Instance.paused && GameManager.Instance.playing && (!Debug.Instance || !Debug.Instance.IsConsoleOpen()))
		{
			MyInput();
			Look();
		}
	}

	private void FixedUpdate()
	{
		if (active && !GameManager.Instance.isRewinding)
		{
			playerMovement.Movement(x, y);
		}
	}

	public void UpdateSensitivity(float s)
	{
		sensMultiplier = s;
		MonoBehaviour.print("sens set to: " + s);
	}

	private void MyInput()
	{
		if ((bool)playerMovement)
		{
			x = Input.GetAxisRaw("Horizontal");
			y = Input.GetAxisRaw("Vertical");
			jumping = Input.GetButton("Jump");
			crouching = Input.GetButton("Crouch");
			if (Input.GetButtonUp("Jump"))
			{
				PlayerMovement.Instance.secondJump = true;
			}
			playerMovement.SetInput(new Vector2(x, y), crouching, jumping);
			if (Input.GetButtonDown("Fire1"))
			{
				PlayerPowers.Instance.FireGun();
			}
			if (Input.GetButtonDown("Restart"))
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	private void Look()
	{
		float mouseX = GetMouseX();
		float num = Input.GetAxis("Mouse Y") * sensitivity * 0.02f * sensMultiplier;
		desiredX = playerCam.transform.localRotation.eulerAngles.y + mouseX;
		xRotation -= num;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);
		actualWallRotation = Mathf.SmoothDamp(actualWallRotation, wallRunRotation, ref wallRotationVel, 0.2f);
		cameraRot = new Vector3(xRotation, desiredX, actualWallRotation);
		orientation.transform.localRotation = Quaternion.Euler(0f, desiredX, 0f);
	}

	public Vector2 GetAxisInput()
	{
		return new Vector2(x, y);
	}

	public float GetMouseX()
	{
		return Input.GetAxis("Mouse X") * sensitivity * 0.02f * sensMultiplier;
	}

	public void SetMouseOffset(float o)
	{
		xRotation = o;
	}

	public float GetMouseOffset()
	{
		return xRotation;
	}
}

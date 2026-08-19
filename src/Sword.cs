using EZCameraShake;
using UnityEngine;

public class Sword : MonoBehaviour
{
	private Animator animator;

	public bool throwSword;

	[HideInInspector]
	public bool pickedUp;

	public Transform mainSword;

	public GameObject sword;

	public RandomSfx audio;

	public PlayerSword playerSword;

	public static Sword Instance;

	[HideInInspector]
	public bool blocking;

	private bool readyToThrow = true;

	private void Awake()
	{
		Instance = this;
		animator = GetComponent<Animator>();
		pickedUp = false;
	}

	public void Update()
	{
		if (!GameManager.Instance.playing || GameManager.Instance.playerDead)
		{
			return;
		}
		blocking = Input.GetButton("Fire2");
		animator.SetBool("Blocking", blocking);
		CheckIfThrowSword();
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			if (Input.GetButtonDown("Fire1"))
			{
				string stateName = ((!(Random.Range(0f, 1f) < 0.5f)) ? "Swing2" : "Swing1");
				animator.Play(stateName);
				CameraShaker.Instance.ShakeOnce(8f, 4f, 0.4f, 0.5f);
				audio.Randomize();
				playerSword.ResetList();
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				animator.Play("Throw");
			}
		}
	}

	public void Pickup()
	{
		pickedUp = true;
		if ((bool)animator)
		{
			animator.Play("Pickup");
		}
		CameraShaker.Instance.ShakeOnce(5f, 2f, 0.25f, 0.25f);
	}

	public bool IsBlocking()
	{
		if (blocking)
		{
			return pickedUp;
		}
		return false;
	}

	public void RemoveSword()
	{
		animator.Play("RemoveSword");
	}

	private void CheckIfThrowSword()
	{
		if (throwSword && pickedUp && readyToThrow)
		{
			readyToThrow = false;
			Invoke("GetReadyToThrow", 0.2f);
			throwSword = false;
			pickedUp = false;
			GameObject gameObject = Object.Instantiate(sword, mainSword.position, mainSword.rotation);
			Rigidbody component = gameObject.GetComponent<Rigidbody>();
			component.AddForce(PlayerMovement.Instance.playerCam.forward * 16000f);
			component.maxAngularVelocity = 300f;
			component.AddTorque(-gameObject.transform.up * 4050f);
			gameObject.GetComponent<LooseSword>().player = true;
			CameraShaker.Instance.ShakeOnce(6f, 6f, 0.2f, 0.45f);
		}
	}

	private void GetReadyToThrow()
	{
		readyToThrow = true;
	}
}

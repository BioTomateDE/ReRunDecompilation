using EZCameraShake;
using UnityEngine;

public class MoveObject : Manipulate
{
	public Vector3 offset;

	public float speed = 1f;

	private Vector3 startPos;

	private bool active;

	private AudioSource sfx;

	public float magnitude;

	public float roughness;

	public float inT;

	public float outT;

	public bool autoPlay;

	public void Awake()
	{
		startPos = base.transform.position;
		sfx = GetComponentInChildren<AudioSource>();
	}

	public void Start()
	{
		if (autoPlay)
		{
			Activate();
		}
	}

	public override void Activate()
	{
		active = true;
		if ((bool)sfx)
		{
			sfx.Play();
		}
		if (magnitude > 0f)
		{
			CameraShaker.Instance.ShakeOnce(magnitude, roughness, inT, outT);
		}
	}

	public void Update()
	{
		if (active)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, startPos + offset, Time.deltaTime * speed);
		}
	}
}

using EZCameraShake;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
	private Vector3 startPos;

	public float startTime = 2f;

	public float speed = 2f;

	public Vector3 offset = new Vector3(0f, -35f, 0f);

	private Vector3 vel;

	private bool done;

	private bool falling;

	private void Awake()
	{
		startPos = base.transform.position;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!done && other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Invoke("StartFall", startTime);
			done = true;
			CameraShaker.Instance.ShakeOnce(1f, 7f, 1f, 1f);
		}
	}

	private void StartFall()
	{
		falling = true;
	}

	private void Update()
	{
		if (falling)
		{
			base.transform.position = Vector3.SmoothDamp(base.transform.position, startPos + offset, ref vel, speed);
		}
	}
}

using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
	public Transform spikes;

	private Vector3 outPos;

	private Vector3 restPos;

	public AudioSource sfx;

	private bool ready = true;

	public void Awake()
	{
		outPos = base.transform.position;
		restPos = outPos + Vector3.down;
		spikes.position = restPos;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (ready)
		{
			ready = false;
			spikes.position = outPos;
			sfx.Play();
			Invoke("ResetSpikes", 2f);
		}
	}

	public void ResetSpikes()
	{
		ready = true;
		spikes.position = restPos;
	}
}

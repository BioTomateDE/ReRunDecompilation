using UnityEngine;

public class AlideAudio : MonoBehaviour
{
	public PlayerMovement player;

	private AudioSource sfx;

	public AudioSource startSlideSfx;

	public void Awake()
	{
		sfx = GetComponent<AudioSource>();
	}

	public void Update()
	{
		float b = 0f;
		if (player.grounded && player.IsCrouching())
		{
			b = PlayerMovement.Instance.GetVelocity().magnitude;
			b = Mathf.Clamp(b * 0.0125f, 0f, 0.6f);
		}
		sfx.volume = Mathf.Lerp(sfx.volume, b, Time.deltaTime * 15f);
	}

	public void PlayStartSlide()
	{
		startSlideSfx.Play();
	}
}

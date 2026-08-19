using UnityEngine;

public class RandomSfx : MonoBehaviour
{
	public AudioClip[] sounds;

	[Range(0f, 2f)]
	public float maxPitch = 0.8f;

	[Range(0f, 2f)]
	public float minPitch = 1.2f;

	private AudioSource s;

	public bool dontPlayOnAwake;

	private void Awake()
	{
		s = GetComponent<AudioSource>();
		if (!dontPlayOnAwake)
		{
			s.clip = sounds[Random.Range(0, sounds.Length)];
			s.pitch = Random.Range(minPitch, maxPitch);
			s.Play();
		}
	}

	public void Randomize()
	{
		s.clip = sounds[Random.Range(0, sounds.Length)];
		s.pitch = Random.Range(minPitch, maxPitch);
		s.Play();
	}
}

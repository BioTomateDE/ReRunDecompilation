using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioSource music;

	private AudioLowPassFilter lowpass;

	private float desiredFreq = 440f;

	public static MusicController Instance;

	private void Awake()
	{
		Instance = this;
		lowpass = GetComponent<AudioLowPassFilter>();
	}

	public void SetFreq(float f)
	{
		desiredFreq = 22000f * f;
	}

	private void Update()
	{
		lowpass.cutoffFrequency = Mathf.Lerp(lowpass.cutoffFrequency, desiredFreq, Time.deltaTime * 2f);
	}

	public void UpdateMusic(float f)
	{
		music.volume = f;
	}
}

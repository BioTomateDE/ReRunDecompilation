using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource music;

    private AudioLowPassFilter lowpass;

    private float desiredFreq = 440f;

    public static MusicController Instance;

    public void Awake()
    {
        Instance = this;
        lowpass = GetComponent<AudioLowPassFilter>();
    }

    public void SetFreq(float _freq)
    {
        desiredFreq = 22000f * _freq;
    }

    public void Update()
    {
        lowpass.cutoffFrequency = Mathf.Lerp(lowpass.cutoffFrequency, desiredFreq, Time.deltaTime * 2f);
    }

    public void UpdateMusic(float _volume)
    {
        music.volume = _volume;
    }
}

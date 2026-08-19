using System.Collections;
using UnityEngine;

public class Music : MonoBehaviour
{
    public enum MusicType
    {
        Ambient = 0,
        Intense = 1,
        Battle = 2
    }

    private AudioSource music;

    private float fadeSpeed = 1f;

    private float desiredVolume = 0.7f;

    private AudioClip ambient;

    private AudioClip intense;

    private AudioClip battle;

    public static Music Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        music = GetComponent<AudioSource>();
        ambient = Resources.Load<AudioClip>("Music/ambient");
        intense = Resources.Load<AudioClip>("Music/intense");
        battle = Resources.Load<AudioClip>("Music/battle");
        StartCoroutine(loadMusic());
    }

    public void PlayMusic(MusicType _musicType, float _fadeSpeed)
    {
        fadeSpeed = _fadeSpeed;
        desiredVolume = 1f;
        switch (_musicType)
        {
            case MusicType.Ambient:
                music.clip = ambient;
                music.Play();
                break;
            case MusicType.Intense:
                music.clip = intense;
                music.Play();
                break;
            case MusicType.Battle:
                music.clip = battle;
                music.Play();
                break;
        }
    }

    public void StopMusic(float fadeSpeed)
    {
        this.fadeSpeed = fadeSpeed;
        desiredVolume = 0f;
    }

    public void Update()
    {
        music.volume = Mathf.Lerp(music.volume, desiredVolume, Time.deltaTime * fadeSpeed);
    }

    private IEnumerator loadMusic()
    {
        string text = Application.dataPath + "/Resources/Music";
        // 'WWW' is obsolete: 'Use UnityWebRequest, a fully featured replacement which is more efficient and has additional features' (CS0618)
#pragma warning disable CS0618
        WWW _ambient = new("file:/" + text + "/ambient.ogg");
        WWW _intense = new("file:/" + text + "/intense.ogg");
        WWW _battle = new("file:/" + text + "/battle.ogg");
#pragma warning restore CS0618
        yield return _ambient;
        ambient = _ambient.GetAudioClip(threeD: false);
        intense = _intense.GetAudioClip(threeD: false);
        battle = _battle.GetAudioClip(threeD: false);
        music.clip = ambient;
        music.Play();
    }
}

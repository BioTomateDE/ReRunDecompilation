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

    public void PlayMusic(MusicType t, float fadeSpeed)
    {
        this.fadeSpeed = fadeSpeed;
        desiredVolume = 1f;
        switch (t)
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
        WWW www = new WWW("file:/" + text + "/ambient.ogg");
        WWW www2 = new WWW("file:/" + text + "/intense.ogg");
        WWW www3 = new WWW("file:/" + text + "/battle.ogg");
        yield return www;
        ambient = www.GetAudioClip(threeD: false);
        intense = www2.GetAudioClip(threeD: false);
        battle = www3.GetAudioClip(threeD: false);
        music.clip = ambient;
        music.Play();
    }
}

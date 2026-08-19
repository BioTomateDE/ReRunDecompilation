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
        float _magnitude = 0f;
        if (player.grounded && player.IsCrouching())
        {
            _magnitude = PlayerMovement.Instance.GetVelocity().magnitude;
            _magnitude = Mathf.Clamp(_magnitude * 0.0125f, 0f, 0.6f);
        }
        sfx.volume = Mathf.Lerp(sfx.volume, _magnitude, Time.deltaTime * 15f);
    }

    public void PlayStartSlide()
    {
        startSlideSfx.Play();
    }
}

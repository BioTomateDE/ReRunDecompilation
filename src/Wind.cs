using UnityEngine;

public class Wind : MonoBehaviour
{
    private AudioSource wind;

    public void Awake()
    {
        wind = GetComponent<AudioSource>();
    }

    public void Update()
    {
        float _magnitude = PlayerMovement.Instance.GetVelocity().magnitude / 60f;
        _magnitude = Mathf.Clamp(_magnitude, 0f, 0.85f);
        if (!PlayerMovement.Instance.grounded)
        {
            _magnitude *= 2f;
        }
        wind.volume = Mathf.Lerp(wind.volume, _magnitude, Time.deltaTime * 5f);
    }
}

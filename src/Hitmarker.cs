using EZCameraShake;
using UnityEngine;

public class Hitmarker : MonoBehaviour
{
    public static Hitmarker Instance;

    private Vector3 maxSize;

    private Vector3 desiredScale;

    private float speed = 2f;

    private AudioSource audio;

    public string[] texts;

    public void Awake()
    {
        Instance = this;
        maxSize = base.transform.localScale;
        audio = GetComponent<AudioSource>();
        base.transform.localScale = Vector3.zero;
    }

    public void Update()
    {
        base.transform.localScale = Vector3.Lerp(base.transform.localScale, desiredScale, Time.deltaTime * speed);
    }

    public void StartHitmarker()
    {
        speed = 40f;
        Invoke("UpSpeed", 0.1f);
        desiredScale = maxSize;
        Invoke("DelayRemove", 0.09f);
        CameraShaker.Instance.ShakeOnce(4f, 5f, 0.2f, 0.2f);
    }

    public void DelayRemove()
    {
        desiredScale = Vector3.zero;
    }

    public void UpSpeed()
    {
        speed = 25f;
    }
}

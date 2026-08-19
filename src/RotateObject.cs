using EZCameraShake;
using UnityEngine;

public class RotateObject : Manipulate
{
    public Vector3 offsetRotation;

    public float speed = 1f;

    private Quaternion startRotation;

    private Quaternion desiredRotation;

    private bool active;

    private AudioSource sfx;

    public float magnitude;

    public float roughness;

    public float inT;

    public float outT;

    public void Awake()
    {
        sfx = GetComponent<AudioSource>();
        startRotation = base.transform.rotation;
        desiredRotation = Quaternion.Euler(startRotation.eulerAngles + offsetRotation);
    }

    public override void Activate()
    {
        active = true;
        if ((bool)sfx)
        {
            sfx.Play();
        }
        if (magnitude > 0f)
        {
            CameraShaker.Instance.ShakeOnce(magnitude, roughness, inT, outT);
        }
    }

    public void Update()
    {
        if (active)
        {
            base.transform.rotation = Quaternion.Lerp(base.transform.rotation, desiredRotation, Time.deltaTime * speed);
        }
    }
}

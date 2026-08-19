using EZCameraShake;
using UnityEngine;

public class FallBlock : MonoBehaviour
{
    private Vector3 startPos;

    public float startTime = 2f;

    public float speed = 2f;

    public Vector3 offset = new(0f, -35f, 0f);

    private Vector3 vel;

    private bool done;

    private bool falling;

    public void Awake()
    {
        startPos = base.transform.position;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!done && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Invoke("StartFall", startTime);
            done = true;
            CameraShaker.Instance.ShakeOnce(1f, 7f, 1f, 1f);
        }
    }

    public void StartFall()
    {
        falling = true;
    }

    public void Update()
    {
        if (falling)
        {
            base.transform.position = Vector3.SmoothDamp(base.transform.position, startPos + offset, ref vel, speed);
        }
    }
}

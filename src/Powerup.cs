using UnityEngine;

public abstract class Powerup : MonoBehaviour, IPowerup
{
    public GameObject destroyFx;

    private Collider collider;

    private Vector3 size;

    public void Awake()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        Invoke("EnableCollider", 0.75f);
        size = base.transform.localScale;
        base.transform.localScale = Vector3.zero;
    }

    public void Update()
    {
        base.transform.localScale = Vector3.Lerp(base.transform.localScale, size, Time.deltaTime * 1.5f);
    }

    public abstract void Activate();

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Activate();
            Object.Destroy(base.gameObject);
            GameManager.Instance.StartRewind();
            if ((bool)destroyFx)
            {
                Object.Instantiate(destroyFx, base.transform.position, base.transform.rotation);
            }
        }
    }

    public void EnableCollider()
    {
        collider.enabled = true;
    }
}

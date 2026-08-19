using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Collider collider;

    private Rigidbody rb;

    public GameObject swordHit;

    public GameObject blood;

    public GameObject arrowHit;

    private bool done;

    public void Awake()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        collider.enabled = false;
        Invoke("ActivateCollider", 0.25f);
    }

    public void ActivateCollider()
    {
        collider.enabled = true;
    }

    public void Update()
    {
        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime * 5f);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (done)
        {
            return;
        }
        done = true;
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
        {
            if (Sword.Instance.IsBlocking())
            {
                Vector3 to = PlayerMovement.Instance.transform.position - base.transform.position;
                float num = Vector3.Angle(PlayerMovement.Instance.orientation.forward, to);
                MonoBehaviour.print("a: " + num);
                if (num > 100f)
                {
                    Object.Instantiate(swordHit, base.transform.position, Quaternion.identity);
                    Object.Destroy(base.gameObject);
                    return;
                }
            }
            Object.Instantiate(blood, other.contacts[0].point, Quaternion.identity);
            PlayerStatus.Instance.Damage(90);
            Object.Destroy(base.gameObject);
        }
        else if (layer == LayerMask.NameToLayer("Enemy"))
        {
            Player component = other.transform.root.GetComponent<Player>();
            if ((bool)component)
            {
                component.Damage(100, base.transform.position);
                Object.Destroy(base.gameObject);
                Object.Instantiate(blood, other.contacts[0].point, Quaternion.identity);
            }
        }
        else if (layer == LayerMask.NameToLayer("Ground"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Object.Destroy(base.transform.GetComponentInChildren<AudioSource>());
            Object.Destroy(this);
            base.gameObject.AddComponent<DestroyObject>().time = 5f;
            collider.enabled = false;
            ParticleSystemRenderer component2 = Object.Instantiate(arrowHit, other.contacts[0].point, Quaternion.identity).GetComponent<ParticleSystemRenderer>();
            Renderer component3 = other.gameObject.GetComponent<Renderer>();
            if ((bool)component3)
            {
                component2.material = component3.material;
            }
        }
        else
        {
            Object.Destroy(base.gameObject);
        }
    }
}

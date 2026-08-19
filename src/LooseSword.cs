using UnityEngine;

public class LooseSword : MonoBehaviour
{
    private bool ready = true;

    private Rigidbody rb;

    private Collider collider;

    public GameObject hitSfx;

    public bool player;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!ready)
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.relativeVelocity.magnitude < 15f)
            {
                return;
            }
            Player _player = other.transform.root.GetComponent<Player>();
            ready = false;
            collider.enabled = false;
            Invoke("GetReady", 0.25f);
            other.transform.root.GetComponent<Player>().Damage(100, other.transform.position);
            Vector3 _vector = rb.velocity * 0.2f;
            _player.GetTorso().GetComponent<Rigidbody>().AddForce(_vector * 500f);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Vector3 _force = ((Vector3.up * 1.4f) + (PlayerMovement.Instance.transform.position - base.transform.position).normalized).normalized;
            float num = Mathf.Clamp(Vector3.Distance(base.transform.position, PlayerMovement.Instance.transform.position) * 0.06f, 0.65f, 1f);
            rb.AddForce(_force * 4500f * num);
            Object.Instantiate(hitSfx, base.transform.position, Quaternion.identity);
            if (player)
            {
                Hitmarker.Instance.StartHitmarker();
            }
        }
        player = false;
    }

    public void GetReady()
    {
        ready = true;
        collider.enabled = true;
    }

    public void RemoveCollision()
    {
        collider.enabled = false;
        Invoke("GetReady", 0.25f);
    }
}

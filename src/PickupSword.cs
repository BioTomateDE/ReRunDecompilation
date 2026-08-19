using UnityEngine;

public class PickupSword : MonoBehaviour
{
    private bool ready;

    public void Awake()
    {
        Invoke("GetReady", 0.5f);
    }

    public void GetReady()
    {
        ready = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (ready && other.gameObject.layer == LayerMask.NameToLayer("Player") && !Sword.Instance.pickedUp)
        {
            Sword.Instance.Pickup();
            Object.Destroy(base.transform.parent.gameObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (ready && other.gameObject.layer == LayerMask.NameToLayer("Player") && !Sword.Instance.pickedUp)
        {
            Sword.Instance.Pickup();
            Object.Destroy(base.transform.parent.gameObject);
        }
    }
}

using UnityEngine;

public class SmokeLeg : MonoBehaviour
{
    public GameObject smokeFx;

    public float cooldown;

    private bool ready = true;

    public void OnTriggerEnter(Collider other)
    {
        if (ready && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ready = false;
            Invoke("GetReady", cooldown);
            Object.Instantiate(smokeFx, base.transform.position, smokeFx.transform.rotation);
        }
    }

    public void GetReady()
    {
        ready = true;
    }
}

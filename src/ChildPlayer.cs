using UnityEngine;

public class ChildPlayer : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement.Instance.transform.parent = base.transform;
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerMovement.Instance.transform.parent = null;
        }
    }
}

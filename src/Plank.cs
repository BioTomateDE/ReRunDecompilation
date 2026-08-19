using UnityEngine;

public class Plank : MonoBehaviour
{
    public GameObject plankBreak;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sword"))
        {
            return;
        }
        MonoBehaviour.print("yep");
        Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
        foreach (Transform transform in componentsInChildren)
        {
            if (!(transform == base.transform))
            {
                Rigidbody rigidbody = transform.gameObject.AddComponent<Rigidbody>();
                rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                rigidbody.AddForce(-base.transform.forward * Random.Range(50, 750) + base.transform.right * Random.Range(50, 300) + base.transform.up * Random.Range(50, 400));
                transform.SetParent(null);
                transform.gameObject.layer = LayerMask.NameToLayer("GroundOnly");
            }
        }
        Object.Instantiate(plankBreak, base.transform.position, plankBreak.transform.rotation);
        Object.Destroy(base.gameObject);
    }
}

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
        foreach (Transform _transform in componentsInChildren)
        {
            if (_transform != base.transform)
            {
                Rigidbody _rigidbody = _transform.gameObject.AddComponent<Rigidbody>();
                _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                _rigidbody.AddForce((-base.transform.forward * Random.Range(50, 750)) + (base.transform.right * Random.Range(50, 300)) + (base.transform.up * Random.Range(50, 400)));
                _transform.SetParent(null);
                _transform.gameObject.layer = LayerMask.NameToLayer("GroundOnly");
            }
        }
        Object.Instantiate(plankBreak, base.transform.position, plankBreak.transform.rotation);
        Object.Destroy(base.gameObject);
    }
}

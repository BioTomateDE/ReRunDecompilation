using UnityEngine;

public class PickupSword : MonoBehaviour
{
	private bool ready;

	private void Awake()
	{
		Invoke("GetReady", 0.5f);
	}

	private void GetReady()
	{
		ready = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (ready && other.gameObject.layer == LayerMask.NameToLayer("Player") && !Sword.Instance.pickedUp)
		{
			Sword.Instance.Pickup();
			Object.Destroy(base.transform.parent.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (ready && other.gameObject.layer == LayerMask.NameToLayer("Player") && !Sword.Instance.pickedUp)
		{
			Sword.Instance.Pickup();
			Object.Destroy(base.transform.parent.gameObject);
		}
	}
}

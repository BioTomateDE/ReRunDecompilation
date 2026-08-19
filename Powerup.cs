using UnityEngine;

public abstract class Powerup : MonoBehaviour, IPowerup
{
	public GameObject destroyFx;

	private Collider collider;

	private Vector3 size;

	private void Awake()
	{
		collider = GetComponent<Collider>();
		collider.enabled = false;
		Invoke("EnableCollider", 0.75f);
		size = base.transform.localScale;
		base.transform.localScale = Vector3.zero;
	}

	private void Update()
	{
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, size, Time.deltaTime * 1.5f);
	}

	public abstract void Activate();

	private void OnTriggerEnter(Collider other)
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

	private void EnableCollider()
	{
		collider.enabled = true;
	}
}

using UnityEngine;

public class KillPlayer : MonoBehaviour
{
	public GameObject blood;

	public void OnCollisionEnter(Collision other)
	{
		if (GameManager.Instance.isRewinding)
		{
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			GameManager.Instance.PlayerDied();
			if ((bool)blood)
			{
				Object.Instantiate(blood, other.transform.position, Quaternion.identity);
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Player component = other.transform.root.GetComponent<Player>();
			if (component.hp > 0)
			{
				component.Damage(5000, other.transform.position);
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (GameManager.Instance.isRewinding)
		{
			return;
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			GameManager.Instance.PlayerDied();
			if ((bool)blood)
			{
				Object.Instantiate(blood, other.transform.position, Quaternion.identity);
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
		{
			Player component = other.transform.root.GetComponent<Player>();
			if (component.hp > 0)
			{
				component.Damage(5000, other.transform.position);
			}
		}
	}
}

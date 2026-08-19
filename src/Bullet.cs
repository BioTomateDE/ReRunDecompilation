using UnityEngine;

public class Bullet : MonoBehaviour
{
	public GameObject enemyKillFx;

	public GameObject bulletHitFx;

	private void Start()
	{
		Invoke("DestroySelf", 2f);
	}

	private void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnCollisionEnter(Collision other)
	{
		int layer = other.gameObject.layer;
		if (layer == LayerMask.NameToLayer("Enemy"))
		{
			Object.Destroy(other.transform.root.gameObject);
			Object.Instantiate(enemyKillFx, other.gameObject.transform.position, enemyKillFx.transform.rotation);
		}
		if (layer == LayerMask.NameToLayer("Player"))
		{
			GameManager.Instance.PlayerDied();
		}
		Object.Destroy(base.gameObject);
	}
}

using UnityEngine;

public class SpawnObjectOnEnemyKill : MonoBehaviour
{
	private Player player;

	public GameObject spawnObject;

	private Vector3 size;

	private void Awake()
	{
		player = GetComponent<Player>();
		if (!spawnObject)
		{
			Object.Destroy(this);
		}
		else
		{
			spawnObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (player.hp <= 0)
		{
			ActivateObject();
			Object.Destroy(this);
		}
	}

	private void ActivateObject()
	{
		spawnObject.SetActive(value: true);
	}
}

using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
	public GameObject[] toSpawn;

	public void Start()
	{
		GameObject[] array = toSpawn;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			Object.Destroy(base.gameObject);
			GameObject[] array = toSpawn;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: true);
			}
		}
	}
}

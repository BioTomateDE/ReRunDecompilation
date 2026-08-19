using UnityEngine;

public class SpawnAction : Manipulate
{
	public GameObject[] enemies;

	public float delay;

	public override void Activate()
	{
		Invoke("Active", delay);
	}

	private void Active()
	{
		GameObject[] array = enemies;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
	}
}

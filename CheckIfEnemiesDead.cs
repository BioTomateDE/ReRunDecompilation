using UnityEngine;

public class CheckIfEnemiesDead : MonoBehaviour
{
	public Manipulate[] actions;

	public Player[] enemies;

	public float delay;

	private bool done;

	private void Update()
	{
		if (done)
		{
			return;
		}
		int num = 0;
		Player[] array = enemies;
		foreach (Player player in array)
		{
			if (!player || player.hp <= 0)
			{
				num++;
			}
		}
		if (num >= enemies.Length)
		{
			done = true;
			Invoke("Activate", delay);
		}
	}

	private void Activate()
	{
		Manipulate[] array = actions;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Activate();
		}
		Object.Destroy(base.gameObject);
	}
}

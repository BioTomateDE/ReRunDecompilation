using UnityEngine;

public class SpawnAction : Manipulate
{
    public GameObject[] enemies;

    public float delay;

    public override void Activate()
    {
        Invoke("Active", delay);
    }

    public void Active()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }
    }
}

using UnityEngine;

public class CheckIfEnemiesDead : MonoBehaviour
{
    public Manipulate[] actions;

    public Player[] enemies;

    public float delay;

    private bool done;

    public void Update()
    {
        if (done)
        {
            return;
        }
        int _deadCount = 0;
        foreach (Player player in enemies)
        {
            if (!player || player.hp <= 0)
            {
                _deadCount++;
            }
        }
        if (_deadCount >= enemies.Length)
        {
            done = true;
            Invoke("Activate", delay);
        }
    }

    public void Activate()
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Activate();
        }
        Object.Destroy(base.gameObject);
    }
}

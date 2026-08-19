using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    public GameObject[] toSpawn;

    public void Start()
    {
        for (int i = 0; i < toSpawn.Length; i++)
        {
            toSpawn[i].SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Object.Destroy(base.gameObject);
            for (int i = 0; i < toSpawn.Length; i++)
            {
                toSpawn[i].SetActive(true);
            }
        }
    }
}

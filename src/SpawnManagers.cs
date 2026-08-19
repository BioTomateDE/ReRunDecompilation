using UnityEngine;

public class SpawnManagers : MonoBehaviour
{
    public GameObject managers;

    public void Awake()
    {
        if (!Managers.Instance)
        {
            Object.Instantiate(managers);
        }
        if ((bool)MusicController.Instance)
        {
            MusicController.Instance.SetFreq(0.02f);
        }
        Object.Destroy(base.gameObject);
    }

    public void Start()
    {
    }
}

using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            MonoBehaviour.print("managers: " + Instance);
            Object.Destroy(base.gameObject);
        }
        else
        {
            Instance = this;
            Object.DontDestroyOnLoad(base.gameObject);
        }
    }
}

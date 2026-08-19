using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public enum CrosshairMode
    {
        Normal = 0,
        Button = 1
    }

    public GameObject crosshair;

    public GameObject button;

    public static Crosshair Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void ChangeCrosshair(CrosshairMode mode)
    {
        HideAll();
        switch (mode)
        {
            case CrosshairMode.Normal:
                crosshair.SetActive(value: true);
                break;
            case CrosshairMode.Button:
                button.SetActive(value: true);
                break;
        }
    }

    private void HideAll()
    {
        Transform[] componentsInChildren = base.transform.GetComponentsInChildren<Transform>();
        foreach (Transform transform in componentsInChildren)
        {
            if (!(transform == base.transform))
            {
                transform.gameObject.SetActive(value: false);
            }
        }
    }
}

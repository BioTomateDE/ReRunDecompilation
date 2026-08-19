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

    public void ChangeCrosshair(CrosshairMode _mode)
    {
        HideAll();
        switch (_mode)
        {
            case CrosshairMode.Normal:
                crosshair.SetActive(true);
                break;
            case CrosshairMode.Button:
                button.SetActive(true);
                break;
        }
    }

    private void HideAll()
    {
        Transform[] componentsInChildren = base.transform.GetComponentsInChildren<Transform>();
        foreach (Transform _transform in componentsInChildren)
        {
            if (_transform != base.transform)
            {
                _transform.gameObject.SetActive(false);
            }
        }
    }
}

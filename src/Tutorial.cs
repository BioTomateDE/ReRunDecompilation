using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI text;

    public static Tutorial Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void AddText(string text)
    {
        this.text.text = text;
    }
}

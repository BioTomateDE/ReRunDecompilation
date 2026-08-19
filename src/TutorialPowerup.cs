using UnityEngine;

public class TutorialPowerup : MonoBehaviour
{
    [TextArea]
    public string text;

    public void OnDestroy()
    {
        Tutorial.Instance.AddText(text);
    }
}

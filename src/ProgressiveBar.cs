using UnityEngine;
using UnityEngine.UI;

public class ProgressiveBar : MonoBehaviour
{
    public Image currentBar;

    public int barSpeed = 10;

    public float popSize = 1.5f;

    private float popScale;

    public bool popOut;

    private Vector3 defaultScale;

    private Vector3 desiredScale;

    private Vector3 desiredPopScale;

    public void Awake()
    {
        defaultScale = currentBar.transform.localScale;
        desiredScale = defaultScale;
    }

    public void Update()
    {
        currentBar.transform.localScale = Vector3.Lerp(currentBar.transform.localScale, desiredScale, Time.unscaledDeltaTime * barSpeed);
        if (popOut)
        {
            base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one * popScale, Time.unscaledDeltaTime * barSpeed);
            popScale = Mathf.Lerp(popScale, 1f, Time.unscaledDeltaTime * barSpeed);
        }
    }

    public void UpdateBar(float _current, float _total)
    {
        float num = _current / _total;
        desiredScale = new Vector3(num * defaultScale.x, defaultScale.y, defaultScale.z);
        if (popOut)
        {
            popScale = popSize;
        }
    }
}

using TMPro;
using UnityEngine;

public class MainText : MonoBehaviour
{
    public static MainText Instance;

    private TextMeshProUGUI textMesh;

    private Vector3 maxSize;

    private Vector3 desiredScale;

    private float speed = 2f;

    private AudioSource audio;

    public void Awake()
    {
        Instance = this;
        maxSize = base.transform.localScale;
        audio = GetComponent<AudioSource>();
        textMesh = GetComponent<TextMeshProUGUI>();
        base.transform.localScale = Vector3.zero;
    }

    public void Update()
    {
        base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, Time.deltaTime * speed);
    }

    public void PutText(string text)
    {
        textMesh.text = text;
        speed = 0.1f;
        Invoke("UpSpeed", 0.6f);
        base.transform.localScale = maxSize;
        audio.PlayDelayed(0.1f);
    }

    public void UpSpeed()
    {
        speed = 30f;
    }
}

using EZCameraShake;
using TMPro;
using UnityEngine;

public class SubText : MonoBehaviour
{
	public static SubText Instance;

	private TextMeshProUGUI textMesh;

	private Vector3 maxSize;

	private Vector3 desiredScale;

	private float speed = 2f;

	private AudioSource audio;

	public string[] texts;

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
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, desiredScale, Time.deltaTime * speed);
	}

	public void PutText()
	{
		textMesh.text = texts[Random.Range(0, texts.Length)];
		speed = 25f;
		Invoke("UpSpeed", 0.6f);
		desiredScale = maxSize;
		audio.PlayDelayed(0.1f);
		Invoke("DelayRemove", 0.5f);
		CameraShaker.Instance.ShakeOnce(7f, 5f, 0.3f, 0.3f);
	}

	public void DelayRemove()
	{
		desiredScale = Vector3.zero;
	}

	public void UpSpeed()
	{
		speed = 30f;
	}
}

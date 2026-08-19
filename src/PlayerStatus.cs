using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatus : MonoBehaviour
{
	private int hp;

	public int maxHp = 100;

	public PostProcessProfile pp;

	private Vignette vignette;

	private ColorGrading colorGrading;

	public RandomSfx sfx;

	public static PlayerStatus Instance;

	private bool healing;

	private float speed = 13f;

	private float defaultVignette = 0.3f;

	private float defaultContrast = 10f;

	private void Awake()
	{
		Instance = this;
		vignette = pp.GetSetting<Vignette>();
		colorGrading = pp.GetSetting<ColorGrading>();
		hp = maxHp;
	}

	public void Damage(int damage)
	{
		if (hp > 0 && GameManager.Instance.playing)
		{
			hp -= damage;
			vignette.intensity.value *= 1.5f;
			colorGrading.colorFilter.value = Color.red;
			healing = false;
			CancelInvoke("StartHealing");
			Invoke("StartHealing", 4f);
			sfx.Randomize();
			if (hp <= 0)
			{
				Kill();
			}
		}
	}

	private void StartHealing()
	{
		healing = true;
	}

	public void ResetStatus()
	{
		hp = maxHp;
	}

	private void Update()
	{
		float value = 1f - (float)hp / (float)maxHp;
		value = Mathf.Clamp(value, 0f, 1f);
		vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, defaultVignette + value * 0.25f, Time.deltaTime * speed);
		colorGrading.contrast.value = Mathf.Lerp(colorGrading.contrast.value, defaultContrast + value * 40f, Time.deltaTime * speed);
		colorGrading.colorFilter.value = Color.Lerp(colorGrading.colorFilter.value, new Color(1f, 1f - value * 0.3f, 1f - value * 0.3f), Time.deltaTime * speed * 2f);
		if (healing && hp < maxHp && hp > 0)
		{
			hp++;
		}
	}

	private void Kill()
	{
		GameManager.Instance.PlayerDied();
		PlayerMovement.Instance.GetRb().isKinematic = true;
		PlayerMovement.Instance.GetRb().velocity = Vector3.zero;
		PlayerMovement.Instance.SetInput(Vector2.zero, crouching: false, jumping: false);
		if (Sword.Instance.pickedUp)
		{
			Sword.Instance.RemoveSword();
		}
	}

	private void OnDestroy()
	{
		vignette.intensity.value = defaultVignette;
		colorGrading.contrast.value = defaultContrast;
		colorGrading.colorFilter.value = Color.white;
	}
}

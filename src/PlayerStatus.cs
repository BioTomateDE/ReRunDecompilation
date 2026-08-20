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

    public void Awake()
    {
        Instance = this;
        vignette = pp.GetSetting<Vignette>();
        colorGrading = pp.GetSetting<ColorGrading>();
        hp = maxHp;
    }

    public void Damage(int _damage)
    {
        if (hp > 0 && GameManager.Instance.playing && !Debug.Instance.god)
        {
            hp -= _damage;
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

    public void StartHealing()
    {
        healing = true;
    }

    public void ResetStatus()
    {
        hp = maxHp;
    }

    public void Update()
    {
        float _remaining = 1f - ((float)hp / maxHp);
        _remaining = Mathf.Clamp(_remaining, 0f, 1f);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, defaultVignette + (_remaining * 0.25f), Time.deltaTime * speed);
        colorGrading.contrast.value = Mathf.Lerp(colorGrading.contrast.value, defaultContrast + (_remaining * 40f), Time.deltaTime * speed);
        colorGrading.colorFilter.value = Color.Lerp(colorGrading.colorFilter.value, new Color(1f, 1f - (_remaining * 0.3f), 1f - (_remaining * 0.3f)), Time.deltaTime * speed * 2f);
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
        PlayerMovement.Instance.SetInput(Vector2.zero, _crouching: false, _jumping: false);
        if (Sword.Instance.pickedUp)
        {
            Sword.Instance.RemoveSword();
        }
    }

    public void OnDestroy()
    {
        vignette.intensity.value = defaultVignette;
        colorGrading.contrast.value = defaultContrast;
        colorGrading.colorFilter.value = Color.white;
    }
}

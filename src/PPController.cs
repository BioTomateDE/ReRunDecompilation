using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPController : MonoBehaviour
{
    public PostProcessProfile pp;

    private ColorGrading colorGrading;

    private ChromaticAberration chromaticAberration;

    private LensDistortion lensDistortion;

    private float desiredSaturation;

    private float desiredChroma;

    private float desiredDistortion;

    private float desiredGrain;

    private float speed = 6f;

    public static PPController Instance;

    public void Awake()
    {
        Instance = this;
        colorGrading = pp.GetSetting<ColorGrading>();
        chromaticAberration = pp.GetSetting<ChromaticAberration>();
        lensDistortion = pp.GetSetting<LensDistortion>();
    }

    public void Update()
    {
        if (!(Mathf.Abs(colorGrading.saturation.value - desiredSaturation) < 0.1f))
        {
            colorGrading.saturation.value = Mathf.Lerp(colorGrading.saturation.value, desiredSaturation, Time.deltaTime * speed);
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, desiredChroma, Time.deltaTime * speed);
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, desiredDistortion, Time.deltaTime * speed);
        }
    }

    public void UpdateFx(float _time)
    {
        float num = 1f - _time;
        desiredDistortion = -150f * num;
        desiredSaturation = -100f * num;
        desiredChroma = 1f * num;
        desiredGrain = 1f * num;
    }

    public void StartRewind()
    {
        lensDistortion.enabled.value = true;
        chromaticAberration.enabled.value = true;
    }

    public void StopRewind()
    {
        colorGrading.saturation.value = 0f;
        lensDistortion.enabled.value = false;
        chromaticAberration.enabled.value = false;
    }

    public void OnDestroy()
    {
        StopRewind();
    }
}

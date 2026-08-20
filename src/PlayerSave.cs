public class PlayerSave
{
    public float[] times = new float[100];

    public bool cameraShake { get; set; } = true;

    /// unused; copypasted from karlson
    public bool motionBlur { get; set; } = true;

    /// unused; copypasted from karlson
    public bool slowmo { get; set; } = true;

    public bool graphics { get; set; } = true;

    /// unused; copypasted from karlson
    public bool muted { get; set; }

    public float sensitivity { get; set; } = 1f;

    public float fov { get; set; } = 80f;

    public float volume { get; set; } = 0.5f;

    public float music { get; set; } = 0.2f;
}

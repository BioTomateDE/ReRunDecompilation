using UnityEngine;

public class ControlMusic : MonoBehaviour
{
    private float desiredFreq;

    public void Awake() { }

    public void Update()
    {
        if (MusicController.Instance)
        {
            float _float = 0.02f;
            if (PlayerMovement.Instance.GetVelocity().magnitude > 15f && !GameManager.Instance.playerDead && GameManager.Instance.playing)
            {
                _float = 1f;
            }
            desiredFreq = Mathf.Lerp(desiredFreq, _float, Time.deltaTime * 5f);
            MusicController.Instance.SetFreq(desiredFreq);
        }
    }
}

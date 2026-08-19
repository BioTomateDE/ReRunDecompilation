using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private Rigidbody rb;

    public AudioSource wind;

    public AudioSource foley;

    private float currentVol;

    private float volVel;

    public void Start()
    {
        rb = PlayerMovement.Instance.GetRb();
    }

    public void Update() { }
}

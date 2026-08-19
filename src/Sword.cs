using EZCameraShake;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator animator;

    public bool throwSword;

    [HideInInspector]
    public bool pickedUp;

    public Transform mainSword;

    public GameObject sword;

    public RandomSfx audio;

    public PlayerSword playerSword;

    public static Sword Instance;

    [HideInInspector]
    public bool blocking;

    private bool readyToThrow = true;

    public void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        pickedUp = false;
    }

    public void Update()
    {
        if (!GameManager.Instance.playing || GameManager.Instance.playerDead)
        {
            return;
        }
        blocking = Input.GetButton("Fire2");
        animator.SetBool("Blocking", blocking);
        CheckIfThrowSword();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                string _stateName = Random.Range(0f, 1f) < 0.5f ? "Swing1" : "Swing2";
                animator.Play(_stateName);
                CameraShaker.Instance.ShakeOnce(8f, 4f, 0.4f, 0.5f);
                audio.Randomize();
                playerSword.ResetList();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                animator.Play("Throw");
            }
        }
    }

    public void Pickup()
    {
        pickedUp = true;
        if (animator)
        {
            animator.Play("Pickup");
        }
        CameraShaker.Instance.ShakeOnce(5f, 2f, 0.25f, 0.25f);
    }

    public bool IsBlocking()
    {
        if (blocking)
        {
            return pickedUp;
        }
        return false;
    }

    public void RemoveSword()
    {
        animator.Play("RemoveSword");
    }

    private void CheckIfThrowSword()
    {
        if (throwSword && pickedUp && readyToThrow)
        {
            readyToThrow = false;
            Invoke("GetReadyToThrow", 0.2f);
            throwSword = false;
            pickedUp = false;
            GameObject _swordObject = Object.Instantiate(sword, mainSword.position, mainSword.rotation);
            Rigidbody _swordBody = _swordObject.GetComponent<Rigidbody>();
            _swordBody.AddForce(PlayerMovement.Instance.playerCam.forward * 16000f);
            _swordBody.maxAngularVelocity = 300f;
            _swordBody.AddTorque(-_swordObject.transform.up * 4050f);
            _swordObject.GetComponent<LooseSword>().player = true;
            CameraShaker.Instance.ShakeOnce(6f, 6f, 0.2f, 0.45f);
        }
    }

    public void GetReadyToThrow()
    {
        readyToThrow = true;
    }
}

using DitzelGames.FastIK;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private RigidEnemy ikController;

    private PlayerController playerController;

    public GameObject bloodFx;

    [Header("Attributes")]
    public new string name;

    public int hp;

    public int maxHp;

    public ProgressiveBar healthBar;

    public TextMeshProUGUI nameField;

    private Rigidbody rb;

    public DamagePlayer damagePlayer;

    public GameObject rightHand;

    public GameObject rightShoulder;

    public FastIKFabric ikHand;

    public GameObject sword;

    public Transform currentSword;

    private bool killed;

    public GameObject killFx;

    public void Awake()
    {
        ikController = GetComponent<RigidEnemy>();
        playerController = GetComponent<PlayerController>();
        rb = ikController.root.GetComponent<Rigidbody>();
    }

    public void Start()
    {
        maxHp = hp;
        nameField.text = name;
        if (ikHand)
        {
            ikHand.Target = PlayerMovement.Instance.transform;
        }
    }

    public void Damage(int damage, Vector3 damagePos)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Kill();
        }
        healthBar.UpdateBar(hp, maxHp);
        ParticleSystem _particleSystem = Object.Instantiate(bloodFx, damagePos, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem.Burst burst = _particleSystem.emission.GetBurst(0);
        burst.count = Mathf.Clamp(damage, 0, 50);
        _particleSystem.emission.SetBurst(0, burst);
    }

    private void Kill()
    {
        if (!killed)
        {
            killed = true;
            ikController.UpdateState(RigidEnemy.EnemyState.dead);
            Object.Destroy(damagePlayer);
            Rigidbody _rigidbody = ikController.torso.GetComponent<Rigidbody>();
            if (!ikHand)
            {
                GameObject _swordObject = Object.Instantiate(sword, currentSword.position, currentSword.rotation);
                _swordObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 4000f);
                _swordObject.GetComponent<LooseSword>().RemoveCollision();
                Object.Destroy(currentSword.gameObject);
            }
            if (ikHand)
            {
                Object.Destroy(ikHand);
                rightShoulder.AddComponent<HingeJoint>().connectedBody = _rigidbody;
                rightHand.AddComponent<HingeJoint>().connectedBody = rightShoulder.GetComponent<Rigidbody>();
            }
            Object.Instantiate(killFx);
            SubText.Instance.PutText();
        }
    }

    public Rigidbody GetRb() => rb;
    public Transform GetRoot() => ikController.root;
    public Transform GetTorso() => ikController.torso;
    public Transform GetTarget() => playerController.target;
}

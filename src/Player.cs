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
        if ((bool)ikHand)
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
        ParticleSystem component = Object.Instantiate(bloodFx, damagePos, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem.Burst burst = component.emission.GetBurst(0);
        burst.count = Mathf.Clamp(damage, 0, 50);
        component.emission.SetBurst(0, burst);
    }

    private void Kill()
    {
        if (!killed)
        {
            killed = true;
            ikController.UpdateState(RigidEnemy.EnemyState.dead);
            Object.Destroy(damagePlayer);
            Rigidbody component = ikController.torso.GetComponent<Rigidbody>();
            if (!ikHand)
            {
                GameObject obj = Object.Instantiate(sword, currentSword.position, currentSword.rotation);
                obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 4000f);
                obj.GetComponent<LooseSword>().RemoveCollision();
                Object.Destroy(currentSword.gameObject);
            }
            if ((bool)ikHand)
            {
                Object.Destroy(ikHand);
                rightShoulder.AddComponent<HingeJoint>().connectedBody = component;
                rightHand.AddComponent<HingeJoint>().connectedBody = rightShoulder.GetComponent<Rigidbody>();
            }
            Object.Instantiate(killFx);
            SubText.Instance.PutText();
        }
    }

    public Rigidbody GetRb()
    {
        return rb;
    }

    public Transform GetRoot()
    {
        return ikController.root;
    }

    public Transform GetTorso()
    {
        return ikController.torso;
    }

    public Transform GetTarget()
    {
        return playerController.target;
    }
}

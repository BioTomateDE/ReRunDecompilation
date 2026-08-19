using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;

    private RigidEnemy ikController;

    public Transform target;

    public Player enemy;

    public Rigidbody weapon;

    public Rigidbody[] armsRb;

    public Rigidbody rootRb;

    public Rigidbody torsoRb;

    public float distance = 5.5f;

    public bool archer;

    public Transform bow;

    public GameObject arrow;

    private bool attacking;

    private bool readyToAttack = true;

    public RandomSfx sfx;

    public float attackHoldForce;

    public float attackHoldLength;

    [Range(20f, 75f)]
    public float LaunchAngle = 45f;

    public void Awake()
    {
        player = GetComponent<Player>();
        ikController = GetComponent<RigidEnemy>();
        readyToAttack = false;
        Invoke("GetReadyToAttack", UnityEngine.Random.Range(1f, 2f));
    }

    public void Start()
    {
        target = PlayerMovement.Instance.transform;
    }

    public void FixedUpdate()
    {
        if (player.hp < 1 || ikController.state != RigidEnemy.EnemyState.active)
        {
            return;
        }
        if (rootRb.transform.position.y < -50f)
        {
            player.Damage(2000, Vector3.zero);
        }
        if (!target)
        {
            return;
        }
        Vector3 normalized = (target.position - ikController.root.position).normalized;
        float num = Vector3.Distance(target.position, ikController.root.position);
        MoveLogic(normalized, num);
        ikController.RotateBody(normalized);
        if (num < distance)
        {
            if (archer)
            {
                Launch();
            }
            else
            {
                Attack(normalized);
            }
        }
        AttackLogic();
    }

    public void LiftArms()
    {
        Rigidbody[] array = armsRb;
        for (int i = 0; i < array.Length; i++)
        {
            array[i].AddForce(Vector3.up * 35f);
            rootRb.AddForce(Vector3.down * 35f);
        }
    }

    private void MoveLogic(Vector3 moveDir, float distanceFromTarget)
    {
        int num = 1;
        if ((double)distanceFromTarget < 4.3)
        {
            num = -1;
        }
        ikController.MoveBody(moveDir * num);
    }

    private void Attack(Vector3 dir)
    {
        if (readyToAttack)
        {
            readyToAttack = false;
            Invoke("GetReadyToAttack", UnityEngine.Random.Range(0.7f, 3f));
            weapon.AddForce(dir * 3000f);
            sfx.Randomize();
            if (!(attackHoldForce <= 0f))
            {
                attacking = true;
                Invoke("StopAttacking", attackHoldLength);
            }
        }
    }

    public void StopAttacking()
    {
        attacking = false;
    }

    private void AttackLogic()
    {
        if (attacking)
        {
            Vector3 vector = target.position - weapon.position;
            weapon.AddForce(vector * attackHoldForce);
            torsoRb.AddForce(-vector * attackHoldForce);
        }
    }

    private void Launch()
    {
        if (readyToAttack)
        {
            readyToAttack = false;
            Invoke("GetReadyToAttack", UnityEngine.Random.Range(1.2f, 3.5f));
            GameObject gameObject = UnityEngine.Object.Instantiate(arrow, bow.position, Quaternion.identity);
            Vector3 a = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 vector = new Vector3(target.position.x, gameObject.transform.position.y, target.position.z);
            gameObject.transform.LookAt(vector);
            float num = Vector3.Distance(a, vector);
            LaunchAngle = UnityEngine.Random.Range(5f, Mathf.Clamp(num, 0f, 60f));
            float y = Physics.gravity.y;
            float num2 = Mathf.Tan(LaunchAngle * ((float)Math.PI / 180f));
            float num3 = target.position.y - gameObject.transform.position.y;
            float num4 = Mathf.Sqrt(y * num * num / (2f * (num3 - num * num2)));
            float y2 = num2 * num4;
            Vector3 direction = new Vector3(0f, y2, num4);
            Vector3 velocity = gameObject.transform.TransformDirection(direction);
            gameObject.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    public void GetReadyToAttack()
    {
        readyToAttack = true;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetSpeed(int s)
    {
        ikController.moveSpeed += s;
    }
}

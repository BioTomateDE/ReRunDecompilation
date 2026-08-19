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
        Vector3 _moveDir = (target.position - ikController.root.position).normalized;
        float _distanceFromTarget = Vector3.Distance(target.position, ikController.root.position);
        MoveLogic(_moveDir, _distanceFromTarget);
        ikController.RotateBody(_moveDir);
        if (_distanceFromTarget < distance)
        {
            if (archer)
            {
                Launch();
            }
            else
            {
                Attack(_moveDir);
            }
        }
        AttackLogic();
    }

    public void LiftArms()
    {
        for (int i = 0; i < armsRb.Length; i++)
        {
            armsRb[i].AddForce(Vector3.up * 35f);
            rootRb.AddForce(Vector3.down * 35f);
        }
    }

    private void MoveLogic(Vector3 _moveDir, float _distanceFromTarget)
    {
        int num = 1;
        if ((double)_distanceFromTarget < 4.3)
        {
            num = -1;
        }
        ikController.MoveBody(_moveDir * num);
    }

    private void Attack(Vector3 _direction)
    {
        if (readyToAttack)
        {
            readyToAttack = false;
            Invoke("GetReadyToAttack", UnityEngine.Random.Range(0.7f, 3f));
            weapon.AddForce(_direction * 3000f);
            sfx.Randomize();
            if (attackHoldForce > 0f)
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
            Vector3 _vector = target.position - weapon.position;
            weapon.AddForce(_vector * attackHoldForce);
            torsoRb.AddForce(-_vector * attackHoldForce);
        }
    }

    private void Launch()
    {
        if (readyToAttack)
        {
            readyToAttack = false;
            Invoke("GetReadyToAttack", UnityEngine.Random.Range(1.2f, 3.5f));
            GameObject gameObject = UnityEngine.Object.Instantiate(arrow, bow.position, Quaternion.identity);
            Vector3 a = new(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 vector = new(target.position.x, gameObject.transform.position.y, target.position.z);
            gameObject.transform.LookAt(vector);
            float num = Vector3.Distance(a, vector);
            LaunchAngle = UnityEngine.Random.Range(5f, Mathf.Clamp(num, 0f, 60f));
            float y = Physics.gravity.y;
            float num2 = Mathf.Tan(LaunchAngle * ((float)Math.PI / 180f));
            float num3 = target.position.y - gameObject.transform.position.y;
            float num4 = Mathf.Sqrt(y * num * num / (2f * (num3 - (num * num2))));
            float y2 = num2 * num4;
            Vector3 direction = new(0f, y2, num4);
            Vector3 velocity = gameObject.transform.TransformDirection(direction);
            gameObject.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    public void GetReadyToAttack()
    {
        readyToAttack = true;
    }

    public void SetTarget(Transform _transform)
    {
        target = _transform;
    }

    public void SetSpeed(int _speed)
    {
        ikController.moveSpeed += _speed;
    }
}

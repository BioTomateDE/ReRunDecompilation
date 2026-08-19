using EZCameraShake;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private bool ready = true;

    public int damage = 40;

    public GameObject blood;

    public GameObject swordHit;

    public Transform swordTip;

    public Transform enemyTorso;

    public void OnCollisionEnter(Collision other)
    {
        if (!ready || other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }
        MonoBehaviour.print("rel vel: " + other.relativeVelocity.magnitude);
        if (other.relativeVelocity.magnitude < 10f)
        {
            return;
        }
        ready = false;
        Invoke("GetReady", 0.5f);
        CameraShaker.Instance.ShakeOnce(12f, 3f, 0.3f, 0.3f);
        if (Sword.Instance.IsBlocking())
        {
            Vector3 _to = PlayerMovement.Instance.transform.position - enemyTorso.position;
            if (Vector3.Angle(PlayerMovement.Instance.orientation.forward, _to) > 130f)
            {
                Object.Instantiate(swordHit, swordTip.position, Quaternion.identity);
                PPController.Instance.StartRewind();
                return;
            }
        }
        Object.Instantiate(blood, other.contacts[0].point, Quaternion.identity);
        PlayerStatus.Instance.Damage(damage);
    }

    public void GetReady()
    {
        ready = true;
    }
}

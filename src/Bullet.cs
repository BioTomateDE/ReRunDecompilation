using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject enemyKillFx;

    public GameObject bulletHitFx;

    public void Start()
    {
        Invoke("DestroySelf", 2f);
    }

    public void DestroySelf()
    {
        Object.Destroy(base.gameObject);
    }

    public void OnCollisionEnter(Collision other)
    {
        int _layer = other.gameObject.layer;
        if (_layer == LayerMask.NameToLayer("Enemy"))
        {
            Object.Destroy(other.transform.root.gameObject);
            Object.Instantiate(enemyKillFx, other.gameObject.transform.position, enemyKillFx.transform.rotation);
        }
        if (_layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.PlayerDied();
        }
        Object.Destroy(base.gameObject);
    }
}

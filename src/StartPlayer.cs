using UnityEngine;

public class StartPlayer : MonoBehaviour
{
    public bool spawnWeapon;

    public void Start()
    {
        for (int num = base.transform.childCount - 1; num >= 0; num--)
        {
            base.transform.GetChild(num).parent = null;
        }
        Object.Destroy(base.gameObject);
        if (spawnWeapon)
        {
            Sword.Instance.Pickup();
        }
    }
}

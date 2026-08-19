using UnityEngine;

public class GenerateClouds : MonoBehaviour
{
    public GameObject cloud;

    private int n = 100;

    public void Start()
    {
        MakeClouds();
    }

    private void MakeClouds()
    {
        for (int i = 0; i < n; i++)
        {
            Vector3 _pos = base.transform.position + (Vector3.right * Random.Range(-250, 250)) + (Vector3.forward * Random.Range(-250, 250)) + (Vector3.up * Random.Range(-10, 10));
            Vector3 _localScale = base.transform.localScale * Random.Range(0.75f, 1.5f);
            Quaternion _rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            Object.Instantiate(cloud, _pos, _rotation).transform.localScale = _localScale;
        }
    }
}

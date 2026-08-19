using UnityEngine;

public class VectorExtensions : MonoBehaviour
{
    public static Vector3 XZVector(Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }
}

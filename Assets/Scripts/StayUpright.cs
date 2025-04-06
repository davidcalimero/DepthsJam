using UnityEngine;

public class StayUpright : MonoBehaviour
{
    private Quaternion originalRotation;

    void Awake()
    {
        originalRotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        transform.rotation = originalRotation;
    }
}
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float translateSmoothness;
    [SerializeField] private float rotationSmoothness;

    private Vector3 translateOffset;
    private Vector3 rotationOffset;

    private void Awake()
    {
        translateOffset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis { X, Y, Z }

public static class VectorsExtension
{
    public static Vector3 WithAxis(this Vector3 vector, Axis axis, float value) 
    {
        return new Vector3
        (
            axis == Axis.X ? value : vector.x,
            axis == Axis.Y ? value : vector.y,
            axis == Axis.Z ? value : vector.z
        );
    }
}

public class BulletTrail : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float progress;

    [SerializeField] private float speed = 40f;

    void Start()
    {
        startPosition = transform.position.WithAxis(Axis.Z, -1);
    }

    void Update()
    {
        progress += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
    
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition.WithAxis(Axis.Z, -1);
    }
}

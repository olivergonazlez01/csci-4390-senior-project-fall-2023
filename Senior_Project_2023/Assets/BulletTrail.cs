using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Josue : I got this script off of a youtube video so I'm not 100% sure what it all does

public enum Axis { X, Y, Z }
public static class VectorsExtension
{
    // Allows changes of x, y, and z values to be easier
    // This class is only used within the BulletTrail script, so it should not matter to much
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
        // startPosition should be the gunPoint of each gun
        startPosition = transform.position.WithAxis(Axis.Z, -1);
    }

    void Update()
    {
        // Allows the bullet trail to keep moving in the direction it was shot off
        progress += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
    
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition.WithAxis(Axis.Z, -1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_entity : MonoBehaviour
{
    private UnityEngine.AI.NavMeshPath path;

    private Transform _target = null;
    private float _speed = 0.0f;
    // path iterator
    private int i = 0;
    // if target changed

    // call this method to set location to go to
    // set to null if the entity should not be moving
    public void setTarget(Transform target) {
        _target = target;
    }
    // call this method to set entity speed
    public void setEntitySpeed(float speed) {
        _speed = speed;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        path = new UnityEngine.AI.NavMeshPath();
    }

    // returns true if the entity is moving, false otherwise
    public bool isMoving() {
        // entity should not be moving
        if (_target == null) {return false;}

        // check if we need to make a new path
        // if path does not exisit, target changed, last corner != current target's position
        if (path.corners.Length == 0 || path.corners[path.corners.Length - 1] != _target.transform.position) {
            UnityEngine.AI.NavMesh.CalculatePath((Vector2)transform.position, (Vector2)_target.transform.position, UnityEngine.AI.NavMesh.AllAreas, path);
            i = 1;
        }
    
        // make sure the index does not go out of bound
        if (i >= path.corners.Length) {return false;}

        // calculate the next path direction
        Vector2 nextStep = path.corners[i] - transform.position;

        // if current location is close enough to the corner, set location to corner and move to next point
        // if not, keep moving towards the current point
        if (nextStep.magnitude < 0.1f) {
            transform.position = path.corners[i];
            i++;
        } else {
            transform.position += (Vector3)nextStep.normalized * _speed * Time.deltaTime;
        }

        // Debuging for paths
        for (int j = 0; j < path.corners.Length - 1; j++) {
            Debug.DrawLine(path.corners[j], path.corners[j + 1], Color.green);
        }
        
        return true;
    }
}

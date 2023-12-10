using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_entity : MonoBehaviour
{
    private UnityEngine.AI.NavMeshPath path;

    private Transform _target = null;
    private float _speed = 0.0f;
    public short health;
    public SFX_Controller soundController;
    private Vector2 _dir = Vector2.zero;
    // path iterator
    private int i = 0;

    // call this method to set location to go to
    // set to null if the entity should not be moving
    public void setTarget(Transform target) {
        _target = target;
    }
    // call this method to set entity speed
    public void setEntitySpeed(float speed) {
        _speed = speed;
    }

    // call this method to deal a certain amount of damage to entity
    // send -1 to instantly kill zombie
    public void Damage_Zombie(short damage_dealt) {
        if (damage_dealt < 0) {
            health = 0;
        } else {
            health -= damage_dealt;
        }
    }

    // call this method to return current direction entity is heading
    public Vector2 getDirection() {
        return _dir;
    }

    // call this method to determine how far back to push the zombie after being shot
    // pushForce will be divided by 100 and zombie will teleport a short distance opposite
    // of the direction the bullet hit them
    public void pushBack(float pushForce, Vector3 gunPoint) {
        Vector3 pushDirection = -(transform.position - gunPoint).normalized;
        pushDirection *= pushForce / 100.0f;
        transform.position = new Vector3(transform.position.x - pushDirection.x, transform.position.y - pushDirection.y, 0.0f);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        path = new UnityEngine.AI.NavMeshPath();
        soundController = GameObject.Find("SFX_Controller").transform.GetComponent<SFX_Controller>();
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

        // set what direction the entity is facing for animation
        _dir = nextStep;

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

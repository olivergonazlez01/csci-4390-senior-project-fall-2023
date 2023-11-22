using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awareness : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // get spitter location
            Transform parent = GetComponentInParent<Transform>();

            // Add 0.2 for the offset head position of zombie
            Vector3 head_position = new Vector3(parent.position.x, parent.position.y + 0.2f, 0.0f);

            // load up attack and chase functions
            Spitter parentScript = GetComponentInParent<Spitter>();

            //cast raycast from the spitter head location to player location. Only check for walls
            RaycastHit2D hit = Physics2D.Raycast (
                head_position,
                collision.transform.position - head_position,
                Vector3.Distance(head_position, collision.transform.position),
                LayerMask.GetMask("Wall")
            );
            // for debuging purposes
            //Debug.DrawRay(head_position, collision.transform.position - head_position, Color.blue);
            if (hit.collider == null) {
                // there is no wall in the way, attack!
                parentScript.attack();
            } else {
                // move closer to player until line of sight is made
                Debug.Log(hit.collider);
                parentScript.chase();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        // player is too far, chase them
        if (collision.tag == "Player") {
            Spitter parent = GetComponentInParent<Spitter>();
            parent.chase();
        }
    }
}

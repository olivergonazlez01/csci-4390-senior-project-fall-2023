using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private const float SPEED = 10.0f;
    private Vector3 direction;
    private Vector3 head_position = new Vector3(0.0f, 0.2f, 0.0f);
    private bool isShooting = false;

    public void shoot(Transform playerLocation) {
        // turn on spit 
        gameObject.SetActive(true);

        // load parent spitter
        Transform spitter = transform.GetComponentInParent<Transform>();

        // calculate the direction to fire and get the angle to rotate sprite for spit
        direction = playerLocation.position - spitter.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);

        // update while spit is flying and begin its lifetime timer
        isShooting = true;
        StartCoroutine(Shooting());
    }

    // Update is called once per frame
    void Update()
    {
        // make spit move
        if (isShooting) {
            transform.position += direction.normalized * SPEED * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            // Deal Damage to Player and reset spit 
            collision.GetComponent<Player>().Damage_Player();
            gameObject.SetActive(false);
            isShooting = false;
            transform.localPosition = Vector3.zero;
        } 
    }

    IEnumerator Shooting() {
        // timer for spit duration
        yield return new WaitForSeconds(2.5f);
        isShooting = false;
        gameObject.SetActive(false);
        transform.localPosition = head_position;
    }
}

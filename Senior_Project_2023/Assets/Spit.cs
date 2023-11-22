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
        gameObject.SetActive(true);
        Transform spitter = transform.GetComponentInParent<Transform>();
        direction = playerLocation.position - spitter.position;
        isShooting = true;
        StartCoroutine(Shooting());
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting) {
            transform.position += direction.normalized * SPEED * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            // Deal Damage to Player and despawn
            collision.GetComponent<Player>().Damage_Player();
            gameObject.SetActive(false);
            isShooting = false;
            transform.localPosition = Vector3.zero;
        } 
    }

    IEnumerator Shooting() {
        yield return new WaitForSeconds(2.5f);
        isShooting = false;
        gameObject.SetActive(false);
        transform.localPosition = head_position;
    }
}

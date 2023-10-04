using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    public Image[] slots;
    public Sprite icon;
    public GameObject player;

    private void OnTriggerStay2D(Collider2D collider) {
        if (Input.GetKey(KeyCode.E)) {
            if (collider.gameObject.CompareTag("Player")) {
                this.transform.parent = player.transform;
                this.transform.position = new Vector3(player.transform.position.x - 0.4f, player.transform.position.y, player.transform.position.z);
                for (int i = 0; i < 2; i++) {
                    if (slots[i].sprite == null) {
                        slots[i].sprite = icon;
                        slots[i].enabled = true;
                        break;
                    }
                }
            }
        }
    }
}

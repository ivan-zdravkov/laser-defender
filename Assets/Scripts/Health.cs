using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    int health = 100;
    float rotation = 360f;

    private void Update()
    {
        this.transform.Rotate(0, 0, this.rotation * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.GainHealth(health);

            Destroy(this.gameObject);
        }
    }
}

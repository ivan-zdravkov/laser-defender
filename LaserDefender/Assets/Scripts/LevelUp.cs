using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
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
            player.GainLevel();

            Destroy(this.gameObject);
        }
    }
}

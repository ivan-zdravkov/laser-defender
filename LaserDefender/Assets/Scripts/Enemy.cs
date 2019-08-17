using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 2.4f;

    float shotCounter;

    // Start is called before the first frame update
    void Start()
    {
        ResetShoutCounter();
    }

    private void ResetShoutCounter()
    {
        this.shotCounter = UnityEngine.Random.Range(this.minTimeBetweenShots, this.maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        this.CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f)
        {
            this.Fire();
            this.ResetShoutCounter();
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
            original: this.laserPrefab,
            position: new Vector3(
                x: this.transform.position.x,
                y: this.transform.position.y - 0.75f,
                z: this.transform.position.z
            ),
            rotation: Quaternion.identity
        ) as GameObject;

        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(
            x: 0,
            y: -this.projectileSpeed
        );

        AudioSource.PlayClipAtPoint(this.shootSFX, this.transform.position);
    }
}

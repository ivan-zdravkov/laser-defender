using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] AlignmentEnum alignment = AlignmentEnum.Enemy;
    [SerializeField] float health = 100f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 2.4f;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject deathVFX;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if (damageDealer.Alignment != this.alignment)
            this.ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.DestroyOnHit();

        this.health -= damageDealer.Damage;

        if (this.health <= 0)
            this.Destroy();
        else
            this.PlayVFX(this.hitVFX);

    }

    private void Destroy()
    {
        this.PlayVFX(this.deathVFX);

        Destroy(this.gameObject);
    }

    private void PlayVFX(GameObject vfx)
    {
        GameObject explosion = Instantiate(vfx, this.transform.position, Quaternion.identity);

        Destroy(explosion, 1f);
    }
}

using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] AlignmentEnum alignment = AlignmentEnum.Enemy;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float tiltX = 150f;
    [SerializeField] float tiltY = 200f;
    [SerializeField] float tiltZ = 50f;
    [SerializeField] float tiltSmooth = 50f;
    [SerializeField] int health = 1000;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject deathVFX;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    float xMin, xMax, yMin, yMax;

    Coroutine firingCoroutine;

    void Start()
    {
        SetUpMoveBoundaries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void SetUpMoveBoundaries()
    {
        Camera camera = Camera.main;

        this.xMin = Camera.main.ViewportToWorldPoint(new Vector3(0.075f, 0, 0)).x;
        this.xMax = Camera.main.ViewportToWorldPoint(new Vector3(0.925f, 0, 0)).x;
        this.yMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.035f, 0)).y;
        this.yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.45f, 0)).y;
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * this.moveSpeed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * this.moveSpeed;

        this.transform.position = new Vector2(
            x: Mathf.Clamp(this.transform.position.x + deltaX, this.xMin, this.xMax),
            y: Mathf.Clamp(this.transform.position.y + deltaY, this.yMin, this.yMax)
        );

        this.TiltHorizontal(deltaX);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
            this.firingCoroutine = StartCoroutine(FireContinuously());

        if (Input.GetButtonUp("Fire1"))
            StopCoroutine(this.firingCoroutine);
            
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            FireNoseLaser();

            yield return new WaitForSeconds(this.projectileFiringPeriod);
        }
    }

    private void FireNoseLaser()
    {
        GameObject laser = Instantiate(
            original: this.laserPrefab,
            position: new Vector3(
                x: this.transform.position.x +
                    (transform.position.x > 0 ? (this.transform.rotation.x * -25) : (this.transform.rotation.x * 25)),
                y: this.transform.position.y + 0.5f,
                z: this.transform.position.z
            ),
            rotation: this.transform.rotation
        ) as GameObject;

        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(
            x: this.transform.forward.x * this.projectileSpeed,
            y: this.projectileSpeed
        );
    }

    private void TiltHorizontal(float deltaX)
    {
        transform.rotation = Quaternion.Slerp(
            a: transform.rotation,
            b: Quaternion.Euler(0, deltaX * this.tiltY, deltaX * -this.tiltZ),
            t: Time.deltaTime * this.tiltSmooth
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

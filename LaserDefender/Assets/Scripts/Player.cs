using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ship
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float tiltX = 150f;
    [SerializeField] float tiltY = 200f;
    [SerializeField] float tiltZ = 50f;
    [SerializeField] float tiltSmooth = 50f;

    [Header("Projectile")]
    [SerializeField] float projectileFiringPeriod = 0.1f;

    int level = 1;

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
            switch (this.level)
            {
                case 1:
                    FireNoseLaser();
                    break;
                case 2:
                    FireSideLasers();
                    break;
                case 3:
                    FireNoseLaser();
                    FireSideLasers();
                    break;
            }

            AudioSource.PlayClipAtPoint(this.shootSFX, Camera.main.transform.position, this.shootVolume);

            yield return new WaitForSeconds(this.projectileFiringPeriod);
        }
    }

    private void FireNoseLaser()
    {
        FireLaser(offset: 0);
    }

    private void FireSideLasers()
    {
        FireLaser(offset: 0.5f);
        FireLaser(offset: -0.5f);
    }

    private void FireLaser(float offset)
    {
        GameObject laser = Instantiate(
            original: this.laserPrefab,
            position: new Vector3(
                x: this.transform.position.x + offset +
                    (transform.position.x > 0 ? (this.transform.rotation.x * -25) : (this.transform.rotation.x * 25)),
                y: this.transform.position.y + (offset == 0 ? 0.5f : 0.25f),
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

    protected override void Hit()
    {
        if (this.level > 1)
            this.level--;

        base.Hit();
    }

    protected override void Die()
    {
        FindObjectOfType<Level>().End();

        base.Die();
    }

    public float Health
    {
        get
        {
            return this.health;
        }
    }

    public void GainHealth(int health)
    {
        this.health += health;
    }

    public void GainLevel()
    {
        if (this.level < 3)
            this.level++;
        else
            this.GainHealth(50); 
    }
}

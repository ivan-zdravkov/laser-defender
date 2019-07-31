using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] GameObject laserPrefab;

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
            GameObject laser = Instantiate(
                original: this.laserPrefab,
                position: this.transform.position,
                rotation: Quaternion.identity
            ) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, this.projectileSpeed);

            yield return new WaitForSeconds(this.projectileFiringPeriod);
        }
    }
}

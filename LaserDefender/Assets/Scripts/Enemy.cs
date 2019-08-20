using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 2.4f;
    [SerializeField] int scorePoints;
    [SerializeField] [Range(10, 50)] int scoreRandomness = 10;
    [SerializeField] GameObject healthPack;
    [SerializeField] GameObject levelUp;

    int dropFactor = 10000;

    GameSession gameSession;

    float initialHealth;
    float shotCounter;

    // Start is called before the first frame update
    void Start()
    {
        this.initialHealth = health;

        ResetShoutCounter();

        this.gameSession = FindObjectOfType<GameSession>();
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

        AudioSource.PlayClipAtPoint(this.shootSFX, Camera.main.transform.position, this.shootVolume);
    }

    protected override void Die()
    {
        this.gameSession.AddToScore(scoreValue: CalculateScore());

        if (DropBonus())
            this.DropHealthPack();

        if (DropBonus())
            DropLevelUp();

        base.Die();
    }

    private bool DropBonus()
    {
        return UnityEngine.Random.Range(0, this.dropFactor) < this.initialHealth;
    }

    private void DropHealthPack()
    {
        GameObject health = Instantiate(
            original: this.healthPack,
            position: new Vector3(
                x: this.transform.position.x,
                y: this.transform.position.y - 0.75f,
                z: this.transform.position.z
            ),
            rotation: Quaternion.identity
        ) as GameObject;

        health.GetComponent<Rigidbody2D>().velocity = new Vector2(
            x: 0,
            y: -this.projectileSpeed
        );
    }

    private void DropLevelUp()
    {
        GameObject level = Instantiate(
            original: this.levelUp,
            position: new Vector3(
                x: this.transform.position.x,
                y: this.transform.position.y - 0.75f,
                z: this.transform.position.z
            ),
            rotation: Quaternion.identity
        ) as GameObject;

        level.GetComponent<Rigidbody2D>().velocity = new Vector2(
            x: 0,
            y: -this.projectileSpeed
        );
    }

    private int CalculateScore()
    {
        int randomFactor = this.scorePoints * this.scoreRandomness / 100;

        return UnityEngine.Random.Range(
            min: this.scorePoints - randomFactor,
            max: this.scorePoints + randomFactor);
    }
}

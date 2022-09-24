using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject laserBullet;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Sound Effects")]
    [SerializeField] GameObject explodeParticles;
    [SerializeField] float durationOfExplosion=1f;
    [SerializeField] AudioClip deathSoundEffect;
    [SerializeField] AudioClip[] shootSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float laserShootVolume = 0.75f;

    float shotCounter;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();

            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        InstantiateLaser();

        PlayShootSoundEffect();
    }

    private void InstantiateLaser()
    {
        var laserBulletInstance = Instantiate(laserBullet, transform.position, Quaternion.identity);

        laserBulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    private void PlayShootSoundEffect()
    {
        var r = Random.Range(0, shootSFX.Length);

        AudioSource.PlayClipAtPoint(shootSFX[r], Camera.main.transform.position, laserShootVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();

        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
        else
        {
            var explodeParticleInstance = Instantiate(explodeParticles, transform.position, transform.rotation);

            Destroy(explodeParticleInstance, durationOfExplosion);

            AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position, deathSoundVolume);
        }

    }

    public void Die()
    {
        ProcessDeath();

        AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position, deathSoundVolume);
    }

    private void ProcessDeath()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);

        var explodeParticleInstance = Instantiate(explodeParticles, transform.position, transform.rotation);

        Destroy(explodeParticleInstance, durationOfExplosion);
    }
}

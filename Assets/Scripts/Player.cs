using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float speed = 3f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 200;
    [SerializeField] GameObject explodeParticles;
    [SerializeField] AudioClip deathSoundEffect;
    [SerializeField] AudioClip failSFX;
    [SerializeField] float durationOfExplosion =1;

    public int GetHealth()
    {
        return health;
    }

    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float failSoundVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float laserShootVolume = 0.25f;

    [Header("Projectile")]
    [SerializeField] GameObject laserBullet;
    [SerializeField]  float projectileSpeed =10f;
    [SerializeField] float projectileFiringPeriod = 0.2f;
    [SerializeField] AudioClip[] shootSFX;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;

    float yMin;
    float yMax;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            InstantiateLaser();

            PlayShootSoundEffect();

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void InstantiateLaser()
    {
        var laserBulletInstance = Instantiate(laserBullet, transform.position, Quaternion.identity);

        laserBulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    }

    private void PlayShootSoundEffect()
    {
        var r = UnityEngine.Random.Range(0, shootSFX.Length);

        AudioSource.PlayClipAtPoint(shootSFX[r], Camera.main.transform.position, laserShootVolume);
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        var deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
        
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
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
            Destroy(gameObject);

            var explodeParticleInstance = Instantiate(explodeParticles, transform.position, transform.rotation);

            Destroy(explodeParticleInstance, durationOfExplosion);

            AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position, deathSoundVolume);

            AudioSource.PlayClipAtPoint(failSFX, Camera.main.transform.position, failSoundVolume);

            FindObjectOfType<GameLoader>().DeferGameOverSceneLoading();

            health = 0;
        }
        else
        {
            var explodeParticleInstance = Instantiate(explodeParticles, transform.position, transform.rotation);

            Destroy(explodeParticleInstance, durationOfExplosion);

            AudioSource.PlayClipAtPoint(deathSoundEffect, Camera.main.transform.position, deathSoundVolume);
        }
    }
}

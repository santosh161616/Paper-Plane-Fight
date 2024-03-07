using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float health = 100;
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject powerUp;
    [SerializeField] GameObject coinSpawn;
 
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip enemyShootSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float enemyLaserVolume = 0.5f;

    float enemyLaserSpeed = 10f;
    float pickupSpeed = 4f;
    float shotCounter;
    int scoreValue = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            EnemyFire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void EnemyFire()
    {
        GameObject enemyLaser = Instantiate(
            enemyProjectile, 
            transform.position, Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);

        //Audio for Emeny to shoot laser
        AudioSource.PlayClipAtPoint(enemyShootSFX, Camera.main.transform.position, enemyLaserVolume);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit(); 
        if (health <= 0)
        {
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
            Destroy(explosion, 1.5f);
            PickUpChance();
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
        }
    }

    private void PickUpChance()
    {
        int randomFactor = Random.Range(0, 100);
        if(randomFactor < 20)
        {
            GameObject rewardPickup = Instantiate(powerUp, transform.position, transform.rotation);
            rewardPickup.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -pickupSpeed);
        }
        if(randomFactor > 85)
        {
            GameObject rewardCoin = Instantiate(coinSpawn, transform.position, transform.rotation);
            rewardCoin.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -pickupSpeed);

        }
    }
}

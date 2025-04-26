using DG.Tweening;
using Plane.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float speed;
    [SerializeField] float laserSpeed;
    [SerializeField] float padding = 0.2f;
    [SerializeField] int playerHealth;

    [Header("Projectile")]
    [SerializeField] float prjectileFiringPeriod = .15f;
    [SerializeField] GameObject playerLaser;

    [Header("Sounds")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip playerShootSFX;
    [SerializeField] AudioClip healthPickupSFX;

    [SerializeField] float healthPickUpSFXVolume = 0.4f;
    [SerializeField][Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField][Range(0, 1)] float playerShootVolume = 0.4f;

    Coroutine firingCoroutine;
    [SerializeField] float magnetStrength = 5f;
    [SerializeField] bool isMagnetic = true;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBounderies();
        GameEvents.Instance.OnHealthReceived += HealthEarned;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Fire();

        //Enabling Magnet field
        if(isMagnetic)
            CoinMagnetSystem();
    }

    private void Movement()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetupMoveBounderies()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(PlayerShooting());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator PlayerShooting()
    {
        while (true)
        {
            GameObject laser =
                    Instantiate(playerLaser, transform.position, Quaternion.identity)
                    as GameObject;
            laser.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, laserSpeed);
            yield return new WaitForSeconds(prjectileFiringPeriod);

            //Player shooting soundSFX
            AudioSource.PlayClipAtPoint(playerShootSFX, Camera.main.transform.position, playerShootVolume);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    public void HealthEarned(int health)
    {
        if (playerHealth < GameSession.Instance.hearts.Length)
        {
            AudioSource.PlayClipAtPoint(healthPickupSFX, Camera.main.transform.position, healthPickUpSFXVolume);
            playerHealth += health;
            GameEvents.Instance.UpdateHealthUI(playerHealth);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        playerHealth -= damageDealer.GetDamage();
        damageDealer.Hit();
        GameEvents.Instance.UpdateHealthUI(playerHealth);
        if (playerHealth <= 0f)
        {
            GameEvents.Instance.GameOver();
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
        }
    }


    private void CoinMagnetSystem()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var coin in coins)
        {
            float distance = Vector2.Distance(transform.position, coin.transform.position);
            if (distance <= magnetStrength)
            {
                Debug.Log("Distance -- "+distance);
                coin.transform.DOMove(transform.position, 1f).OnComplete(() => coin.GetComponent<SpriteRenderer>().DOFade(0, 0.5f));               
            }
        }

    }

    private void OnDisable()
    {
        GameEvents.Instance.OnHealthReceived -= HealthEarned;
    }
}

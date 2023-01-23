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
    [SerializeField]float prjectileFiringPeriod = .15f;    
    [SerializeField] GameObject playerLaser;

    [Header("Sounds")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip playerShootSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float playerShootVolume = 0.4f;

    [Header("Health")]
    [SerializeField] Image[] hearts; 
    [SerializeField] Sprite fullHearts;
    [SerializeField] Sprite emptyHearts;

    [SerializeField] int earnedCoin;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBounderies();
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Fire();
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
        if(Input.GetButtonDown("Fire1"))
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
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
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

    public int GetHealth()
    {
        return playerHealth;
    }

    public void HealthEarned(int health)
    {
        if(playerHealth < hearts.Length)
        {            
            playerHealth += health;
            UpdateHealthUI(playerHealth);
        }                
    }

    public void RewardCoin(int coin)
    {
        earnedCoin += coin;
    }

    public int GetCoin()
    {
        Debug.Log(earnedCoin);
        return earnedCoin;        
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        playerHealth -= damageDealer.GetDamage();
        damageDealer.Hit();
        UpdateHealthUI(playerHealth);
        if(playerHealth <= 0f)
        {
            FindObjectOfType<Level>().LoadGameOver();
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }
        }
    }

}

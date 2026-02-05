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

    [Header("Shield")]
    [SerializeField] private Shield _playerShield;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Touch movement helpers
    Vector3 touchOffset = Vector3.zero;
    bool isTouchMoving = false;
    int movementTouchId = -1;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBounderies();
        if (GameEvents.Instance != null)
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
        // Mobile: touch-drag movement (left half of screen) with offset so plane isn't hidden under finger,
        // and keep plane clamped inside screen bounds.
        if (Application.isMobilePlatform)
        {
            Camera gameCamera = Camera.main;
            if (gameCamera == null)
                gameCamera = FindObjectOfType<Camera>();

            if (gameCamera == null)
                return;

            // Process touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);

                // Movement touches are only considered if they start on the left half (designer choice).
                bool touchIsLeft = t.position.x < Screen.width * 0.5f;

                if (t.phase == TouchPhase.Began && touchIsLeft && !isTouchMoving)
                {
                    // Start movement touch and calculate offset so the plane does not sit directly under the finger.
                    movementTouchId = t.fingerId;
                    Vector3 touchWorld = gameCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, Mathf.Abs(gameCamera.transform.position.z - transform.position.z)));
                    touchOffset = transform.position - touchWorld;
                    isTouchMoving = true;
                }

                if (t.fingerId == movementTouchId && isTouchMoving)
                {
                    if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                    {
                        Vector3 touchWorld = gameCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, Mathf.Abs(gameCamera.transform.position.z - transform.position.z)));
                        Vector3 desired = touchWorld + touchOffset;

                        // Clamp so the plane never leaves the visible area (xMin/xMax/yMin/yMax set in SetupMoveBounderies)
                        float clampedX = Mathf.Clamp(desired.x, xMin, xMax);
                        float clampedY = Mathf.Clamp(desired.y, yMin, yMax);

                        Vector2 target = new Vector2(clampedX, clampedY);
                        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    }

                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        // End movement touch
                        isTouchMoving = false;
                        movementTouchId = -1;
                        touchOffset = Vector3.zero;
                    }
                }
            }

            // If no touch is controlling movement, do nothing (prevents drift)
            return;
        }

        // Desktop / Editor: axis-based input
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetupMoveBounderies()
    {
        Camera gameCamera = Camera.main;

        if (gameCamera == null)
        {
            // Try to find any camera in the scene as a fallback
            gameCamera = FindObjectOfType<Camera>();
            if (gameCamera == null)
            {
                Debug.LogWarning("[Player] No Camera found. Movement boundaries will use large defaults.");
                xMin = -100f;
                xMax = 100f;
                yMin = -100f;
                yMax = 100f;
                return;
            }
        }

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Fire()
    {
        // Mobile touch controls: right half of screen to fire (tap & hold)
        if (Application.isMobilePlatform)
        {
            bool fireDown = false;
            bool fireUp = false;

            foreach (Touch t in Input.touches)
            {
                if (t.position.x >= Screen.width * 0.5f)
                {
                    if (t.phase == TouchPhase.Began) fireDown = true;
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) fireUp = true;
                }
            }

            if (fireDown && firingCoroutine == null)
                firingCoroutine = StartCoroutine(PlayerShooting());

            if (fireUp && firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = null;
            }

            return;
        }

        // Desktop / Editor input
        if (Input.GetButtonDown("Fire1"))
        {
            if (firingCoroutine == null)
                firingCoroutine = StartCoroutine(PlayerShooting());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = null;
            }
        }
    }

    IEnumerator PlayerShooting()
    {
        while (true)
        {
            if (playerLaser != null)
            {
                GameObject laser = Instantiate(playerLaser, transform.position, Quaternion.identity) as GameObject;
                var rb = laser.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.linearVelocity = new Vector2(0, laserSpeed);
            }

            //Player shooting soundSFX (safe: only if camera exists)
            if (playerShootSFX != null && Camera.main != null)
                AudioSource.PlayClipAtPoint(playerShootSFX, Camera.main.transform.position, playerShootVolume);

            yield return new WaitForSeconds(prjectileFiringPeriod);
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
        Shield shield = GetComponentInChildren<Shield>();
        
        if (shield==null || !shield.IsActive)
        {
            var activeShield = Instantiate(_playerShield, transform);
            activeShield.Activate();
            Debug.Log("Shield Activated via Health Pickup");
        }

        if (playerHealth < GameSession.Instance.GetPlayerLife)
        {
            if (healthPickupSFX != null && Camera.main != null)
                AudioSource.PlayClipAtPoint(healthPickupSFX, Camera.main.transform.position, healthPickUpSFXVolume);

            playerHealth += health;
            GameEvents.Instance.UpdateHealthUI(playerHealth);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int incoming = damageDealer.GetDamage();

        // If player has an active shield, let it try to absorb first.
        Shield shield = GetComponentInChildren<Shield>();
        if (shield != null && shield.IsActive)
        {
            incoming = shield.AbsorbDamage(incoming);
        }

        // Destroy the damaging object (projectile) in any case.
        damageDealer.Hit();

        // If shield absorbed all damage, update UI and return early.
        if (incoming <= 0)
        {
            GameEvents.Instance.UpdateHealthUI(playerHealth);
            return;
        }

        // Apply remaining damage to player health.
        playerHealth -= incoming;
        GameEvents.Instance.UpdateHealthUI(playerHealth);
        if (playerHealth <= 0f)
        {
            GameEvents.Instance.GameOver();
            gameObject.SetActive(false);
            if (deathSFX != null && Camera.main != null)
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
                // Safe-guard DOTween usage (may throw if plugin not available)
                try
                {
                    coin.transform.DOMove(transform.position, 1f).OnComplete(() =>
                    {
                        var sr = coin.GetComponent<SpriteRenderer>();
                        if (sr != null) sr.DOFade(0, 0.5f);
                    });
                }
                catch
                {
                    // fallback: simple move if DOTween not available at runtime
                    coin.transform.position = Vector2.MoveTowards(coin.transform.position, transform.position, 5f * Time.deltaTime);
                }
            }
        }

    }

    private void OnDisable()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.OnHealthReceived -= HealthEarned;
    }
}

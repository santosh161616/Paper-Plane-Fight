using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private int _protectionAmount = 50;
    [SerializeField] private float _duration = 10f;
    [SerializeField] private float _cooldown = 30f;

    // Runtime state
    int _remainingProtection;
    bool _isActive;
    Coroutine _lifetimeRoutine;

    public int ProtectionAmount => _protectionAmount;
    public float Duration => _duration;
    public float Cooldown => _cooldown;
    public int RemainingProtection => _remainingProtection;
    public bool IsActive => _isActive;

    void Start()
    {
        // Auto-activate when the component is enabled / spawned on the player.
        Activate();
    }

    /// <summary>
    /// Activate shield: reset protection and start lifetime timer.
    /// </summary>
    public void Activate()
    {
        _remainingProtection = _protectionAmount;
        _isActive = true;

        if (_lifetimeRoutine != null)
            StopCoroutine(_lifetimeRoutine);

        _lifetimeRoutine = StartCoroutine(ShieldLifetime());
        // TODO: trigger VFX / audio here (e.g. enable child effect)
    }

    /// <summary>
    /// Deactivate shield (called when duration ends or protection exhausted).
    /// </summary>
    public void Deactivate()
    {
        _isActive = false;
        if (_lifetimeRoutine != null)
        {
            StopCoroutine(_lifetimeRoutine);
            _lifetimeRoutine = null;
        }

        // Optionally destroy the component or the GameObject if it's a temporary powerup object attached to the player
        // Destroy(this); // uncomment if you want the component removed when done
    }

    IEnumerator ShieldLifetime()
    {
        yield return new WaitForSeconds(_duration);
        Deactivate();
    }

    /// <summary>
    /// Try to absorb incoming damage. Returns leftover damage that should be applied to player health.
    /// Always call <paramref name="onHit"/> (e.g. projectile.Hit) in the caller after processing the returned leftover.
    /// </summary>
    public int AbsorbDamage(int damage)
    {
        if (!_isActive || _remainingProtection <= 0)
            return damage;

        int absorbed = Mathf.Min(_remainingProtection, damage);
        _remainingProtection -= absorbed;

        if (_remainingProtection <= 0)
        {
            // exhausted
            Deactivate();
        }

        return damage - absorbed;
    }
}
    
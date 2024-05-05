using System.Collections;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private BlackFade blackFade;
    [Header("Life")]
    public int maxHealth;
    public Cooldown damageCooldown;
    public event Action<int> OnHealthChanged;
    [SerializeField] private LookAtConstraint lookAtConstraint;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; OnHealthChanged?.Invoke(value); }
    }
    private int _currentHealth;
    private PlayerMovement _playerMovement;
    [Header("Death")]
    [SerializeField] private bool canDie;
    [SerializeField] private float deathDuration = 2f;
    private bool isDead;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private AnimationCurve damageCurve;
    private void Start()
    {
        CurrentHealth = maxHealth;
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public IEnumerator ResetHearts()
    {
        while (CurrentHealth < maxHealth)
        {
            CurrentHealth++;
            yield return new WaitForSeconds(1f);
        }
    }
    public void TakeDamage(int damage)
    {
        if (_playerMovement.isDashing || damageCooldown.IsCoolingDown) { Debug.Log("Invulnerable"); return; }
        int result = CurrentHealth += damage;
        if (result > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
        else if (result < 1)
        {
            if(!isDead) Die();
        }
        else
        {
            CurrentHealth = result;
        }
        StartCoroutine(DamageMaterialChange());
        damageCooldown.StartCooldown();
        GetComponent<PlayerAnimatorManager>().TriggerAnimation("hit");
    }

    private IEnumerator DamageMaterialChange()
    {
        float t = 0;
        while (t < damageCooldown.CooldownTime)
        {
            t += Time.deltaTime;
            float value = damageCurve.Evaluate(t / damageCooldown.CooldownTime);
            if (damageMaterial != null) damageMaterial.SetFloat("_DamageValue", value);
            yield return null;
        }
    }

    public void Die()
    {
        Debug.Log("You Dead");
        if (!canDie) return;
        animator.SetBool("death", true);
        isDead = true;
        StartCoroutine(DieSequence());
        lookAtConstraint.enabled = false;
    }
    private IEnumerator DieSequence()
    {
        CinemachineVirtualCamera deathCam = (CinemachineVirtualCamera)GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        float startOrthSize = deathCam.m_Lens.OrthographicSize;
        PlayerInputHandler.Instance.DisableInputs();
        float t = 0;
        while (t < deathDuration)
        {
            t += Time.unscaledDeltaTime;
            float value = t / deathDuration;
            Time.timeScale = Mathf.Clamp(1 - value, 0, 1);
            deathCam.m_Lens.OrthographicSize = Mathf.SmoothStep(startOrthSize, 2.5f, value);
            yield return null;
        }
        AudioManager.Instance.StopAllLoops(1f);
        StartCoroutine(DeathSequence());
    }
    private IEnumerator DeathSequence()
    {
        PlayerInputHandler.Instance.DisableInputs();
        Time.timeScale = 0;
        AudioManager.Instance.StopAllLoops(0.5f);
        yield return StartCoroutine(blackFade.FadeToBlack(0.5f));
        Time.timeScale = 1;
        CrossSceneInformation.CurrentTimerValue = LevelManager.Instance.elapsedTime;
        PlayerInputHandler.Instance.EnableInputs();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnDisable()
    {
        if (damageMaterial != null) damageMaterial.SetFloat("_DamageValue", 0f);
    }
}


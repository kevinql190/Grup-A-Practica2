using System.Collections;
using UnityEngine;
using System;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    private float currentSpeed;
    private Vector3 _direction;
    private float _currentVelocity; //Camera Damp
    private float targetAngle; //Rotació
    [Header("Dash")]
    [SerializeField] private Cooldown dashCooldown;
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashDistance = 0.3f;
    [SerializeField] private float dashLandRadius = 0.5f;
    [SerializeField] private int dashMaxCharges = 3;
    [SerializeField] private float dashChargeCooldown = 2;
    [HideInInspector] public bool isDashing;
    [SerializeField] private bool canDash = true; //Para debug
    public event Action<float> OnDashChargeChanged;
    public float CurrentDashCharges
    {
        get { return _dashCharges; }
        set { _dashCharges = value; OnDashChargeChanged?.Invoke(value); }
    }
    private float _dashCharges;
    private bool isDashCharging;
    [SerializeField] private VisualEffect dashParticle;
    [Header("Camera")]
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Header("Gizmos")]
    [SerializeField] private bool moveGizmo;
    [SerializeField] private bool dashGizmo;

    private NavMeshAgent agent;
    [SerializeField] private CapsuleCollider capsule;

    private void Start()
    {
        currentSpeed = moveSpeed;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DashRecharge());
    }

    private void Update()
    {
        HandleDash();
        ProjectInputVector();
        PlayerTranslate();
        if (_direction.sqrMagnitude == 0) return;
        PlayerRotate();
    }
    #region Player Run
    private void ProjectInputVector()
    {
        Vector3 cameraForward = Vector3.Scale(virtualCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        if (isDashing && PlayerInputHandler.MoveInput.sqrMagnitude == 0) return;
        _direction = PlayerInputHandler.MoveInput.x * virtualCamera.transform.right + PlayerInputHandler.MoveInput.y * cameraForward;
    }

    private void PlayerTranslate()
    {
        if (!GetComponent<NavMeshAgent>().enabled) return;
        agent.Move(currentSpeed * Time.deltaTime * _direction);
    }
    private void PlayerRotate()
    {
        //if (!isDashing)
        {
            targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        }
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    #endregion
    #region Player Dash
    private void HandleDash()
    {
        if (CurrentDashCharges < 1 || dashCooldown.IsCoolingDown || !PlayerInputHandler.DashJustPressed || !canDash) return;
        AudioManager.Instance.PlaySFXOnce("dash");
        CurrentDashCharges--;
        if(!isDashCharging) StartCoroutine(DashRecharge());
        StartCoroutine(Dash());
        dashCooldown.StartCooldown();
    }
    IEnumerator Dash()
    {
        isDashing = true;
        dashParticle.Play();
        Vector3 dashDirection = _direction.sqrMagnitude == 0 ? transform.forward : _direction; //Dash Forward if there is no move input
        Vector3 newPos = transform.position + dashDistance * dashDirection; //Calculate destination Position
        bool hitWall = false;
        Ray ray = new(transform.position, newPos-transform.position);
        if (Physics.Raycast(ray, out RaycastHit raycasthit, dashDistance)) //Raycast possible walls (colliders)
        {
            if (raycasthit.collider.CompareTag("Wall")) hitWall = true;
        }
        bool raycastHit = NavMesh.Raycast(transform.position, newPos, out _, NavMesh.AllAreas);
        bool dasheable = NavMesh.SamplePosition(newPos - Vector3.up * capsule.height / 2, out NavMeshHit navmeshhit, dashLandRadius, NavMesh.AllAreas);
        if (raycastHit && dasheable && !hitWall) //If no walls, destination has NavMesh and no lineal path to it, then Void Dash
        {
            Debug.Log("Void Dash");
            agent.enabled = false;
            newPos = new(navmeshhit.position.x, transform.position.y, navmeshhit.position.z);
            float t = 0f;
            float dashDistance2 = Vector3.Distance(newPos, transform.position);
            Vector3 originalPos = transform.position;
            while (t < dashDistance2 / dashSpeed)
            {
                t += Time.deltaTime;
                float value = t / (dashDistance2 / dashSpeed);
                transform.position = Vector3.Lerp(originalPos, newPos, value);
                yield return null;
            }
            agent.enabled = true;
        }
        else
        {
            currentSpeed = dashSpeed;
            float t = 0f;
            while (t < dashDistance/dashSpeed)
            {
                t += Time.deltaTime;
                _direction = _direction.sqrMagnitude == 0 ? transform.forward : _direction;
                yield return null;
            }
            _direction = _direction.sqrMagnitude == 0 ? Vector2.zero : _direction;
            currentSpeed = moveSpeed;
        }
        dashParticle.Stop();
        isDashing = false;
    }
    IEnumerator DashRecharge()
    {
        isDashCharging = true;
        while (CurrentDashCharges < dashMaxCharges)
        {
            CurrentDashCharges += Time.deltaTime / dashChargeCooldown;
            yield return null;
        }
        isDashCharging = false;
    }
    #endregion
    public IEnumerator StunPlayer(float time, float speedDecrease)
    {
        currentSpeed -= speedDecrease;
        canDash = false;
        yield return new WaitForSeconds(time);
        canDash = true;
        currentSpeed += speedDecrease;
    }
    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        // Draw a capsule at pointX with the same dimensions as the CapsuleCollider
        if (moveGizmo)
        {
            Gizmos.color = Color.blue;
            Vector3 newPos = transform.position + moveSpeed * transform.forward - Vector3.up * capsule.height / 2;
            Gizmos.DrawLine(transform.position - Vector3.up * capsule.height / 2, newPos);
            Gizmos.DrawWireSphere(newPos, capsule.radius);
        }
        if (dashGizmo)
        {
            Gizmos.color = Color.red;
            Vector3 newPos = transform.position + dashDistance * transform.forward - Vector3.up * capsule.height / 2;
            Gizmos.DrawLine(transform.position - Vector3.up * capsule.height / 2, newPos);
            Gizmos.DrawWireSphere(newPos, dashLandRadius);
        }
    }
    #endregion
}

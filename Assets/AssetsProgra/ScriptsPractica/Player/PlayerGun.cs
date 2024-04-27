using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Cooldown rechargeTime;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private int targetCount = 1;
    [SerializeField] Color shootingColor = Color.white; // The color of the line
    [SerializeField] private float shootingLineDuration;
    [SerializeField] private float defaultLineDistance;
    private LineRenderer lineRenderer;
    private IEnumerator vanishTrail;
    private void Awake()
    {
        vanishTrail = VanishTrail();
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = shootingColor;
        lineRenderer.endColor = shootingColor;
        lineRenderer.enabled = false;
    }
    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        if (!PlayerInputHandler.AttackJustPressed || rechargeTime.IsCoolingDown) return;
        Shoot();
    }
    private void Shoot()
    {
        Ray ray = new(transform.position,transform.forward);
        var hits = Physics.RaycastAll(ray, attackRange, _targetLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        int i = 0;
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(-attackDamage);
            if (i >= targetCount - 1) break;
            i++;
        }
        Debug.Log(i);
        if (hits.Length != 0)
        {
            DrawShootTrail(hits[i].transform.position);
            AudioManager.Instance.PlaySFXOnce("SoundEffect");
        }
        else
            DrawShootTrail(transform.position + transform.forward * defaultLineDistance);
    }

    private void DrawShootTrail(Vector3 lastHit)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, lastHit);
        lineRenderer.enabled = true;
        StopCoroutine(vanishTrail);
        vanishTrail = VanishTrail();
        StartCoroutine(vanishTrail);
    }
    private IEnumerator VanishTrail()
    {
        float t = 0;
        while(t< shootingLineDuration)
        {
            t += Time.deltaTime;
            float value = t / shootingLineDuration;
            lineRenderer.widthMultiplier = 1 - value;
            yield return null;
        }
    }
}

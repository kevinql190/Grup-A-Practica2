using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoAttack : Enemy
{
    //Animació provisional BORRAR quan tinguem les animacions
    [Header("Animació provisional :)")]
    public float attackScaleMultiplier = 1.5f;
    private Vector3 originalScale;

    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        originalScale = transform.localScale; // Borrar quan tinguem les animacions
    }
    public override void Attack()
    {
        StartCoroutine(DelayedAttack()); // Borrar quan tinguem les animacions
    }
 
    IEnumerator DelayedAttack()
    {
        // Posar a la funcio attack quan es tinguin les animacions
        FacePlayer();

        // ------------- Animació provisional BORRAR quan tinguem les animacions ---------------------
        // Animación provisional: pre-atac
        yield return StartCoroutine(ScaleOverTime(originalScale * attackScaleMultiplier, 0.025f));
        //Esperar 
        yield return new WaitForSeconds(0.025f);
        //Fi animación pre-atac
        yield return ScaleOverTime(originalScale, 0.025f);
        //------------------------------------------------------------------------------

        // Posar a la funcio attack quan es tinguin les animacions
        if (InRange())
        {
            Debug.Log("Tomato damage: " + damage);
            Damager();
        }

    }
    //Animació provisional: pre-atac (s'ha de borrar quan estiguin les animacions)
    IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        float elapsedTime = 0;
        Vector3 originalScale = transform.localScale;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }
    public override void FacePlayer()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}

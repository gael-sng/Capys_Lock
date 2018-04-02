using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBlock : MonoBehaviour {

    public float maxExplosionForce = 10f;
    public float maxExplosionDamage = 5f;
    public float explosionRange = 2f;
    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Start()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        AttackSchedule attack = gameManager.GetComponent<AttackSchedule>();
        if(attack != null)
            attack.addExplosive(gameObject);
        FlexAttack flexAttack = gameManager.GetComponent<FlexAttack>();
        if (flexAttack != null)
            flexAttack.addExplosive(gameObject);
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
    }

    private void OnDestroy()
    {
        Collider2D[] hits;
        hits = Physics2D.OverlapCircleAll(myTransform.position, explosionRange);
        foreach (Collider2D hit in hits)
        {
            print("Explodiu: " + hit.gameObject.name);
            if (hit.gameObject.tag.Equals("Destructible"))
            {
                Vector3 direction = hit.transform.position - myTransform.position;
                float distance = direction.magnitude;
                float realForce = maxExplosionForce - (maxExplosionForce / explosionRange) * distance;
                float realDamage = maxExplosionDamage - (maxExplosionDamage / explosionRange) * distance;

                Destructible dest = hit.GetComponent<Destructible>();
                if (dest != null)
                    dest.TakeDamage(realDamage);

                Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                    rb2d.AddForce(direction.normalized * realForce, ForceMode2D.Impulse);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Character")))
        {
            if (!playerAttack)
            {
                collision.gameObject.GetComponent<IDamageable>().Damage(damage, (collision.transform.position - transform.position).normalized * knockBack);
            }
            else
            {
                return;
            }
        }

        Hit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Sound soundOnShot;
    [SerializeField] Sound soundOnHit;
    [SerializeField] AudioSource ads;
    public float knockBack;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite playerBulletSprite;

    [HideInInspector] public bool playerAttack;
    [HideInInspector] public int damage;
    [SerializeField] Color playerStartColor;
    [SerializeField] int durability;
    [SerializeField] Animator a;
    public ParticleSystem ps;
    public Rigidbody2D rb;
    bool dead;

    public virtual void Start()
    {
        ads.PlayOneShot(soundOnShot.RandomSound());
    }

    public void ChangeToPlayerBullet()
    {
        playerAttack = true;
        sr.sprite = playerBulletSprite;

        if (ps)
        {
            var m = ps.main;
            m.startColor = new ParticleSystem.MinMaxGradient(playerStartColor);
        }

        gameObject.layer = 12;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < .35f)
        {
            // Play fade out animation
            DestroyProjectile(true);
        }
    }

    public virtual void DestroyProjectile(bool fadeOut)
    {
        if (dead) { return; }
        dead = true;

        if (a)
        {
            if (fadeOut)
            {
                a.Play("Fade_Out");
            }
            else
            {
                //ads.PlayOneShot(soundOnHit.sound.GetClip());
                a.Play("Bullet_Explode");
            }
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Emit()
    {
        ps.Emit(5);
    }

    public void Hit()
    {
        durability--;

        if (durability <= 0)
        {
            // Play explode animation
            DestroyProjectile(false);
        }
    }
}
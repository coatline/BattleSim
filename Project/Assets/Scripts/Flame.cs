using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public bool shooting;
    public int damage;
    Animator a;
    void Awake()
    {
        a = GetComponent<Animator>();
        timer = 1;
    }

    void Update()
    {
        if (shooting)
        {
            a.enabled = true;
        }
        else
        {
            a.enabled = false;
        }
    }

    float timer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Man"))
        {
            if (timer > 0)
            {
                timer = 0;
                //collision.gameObject.GetComponent<Character>().TakeDamage(damage);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
}

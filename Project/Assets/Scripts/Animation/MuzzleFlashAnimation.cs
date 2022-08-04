using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    bool flashing;
    float speed;
    float alpha;

    public void Flash(float speed, float size)
    {
        this.speed = speed;

        sr.transform.localScale = Vector3.one * size;
        flashing = true;
        alpha = 1;
    }

    void Update()
    {
        if (flashing)
        {
            sr.color = new Color(1, 1, 1, alpha);

            alpha -= Time.deltaTime * speed;

            if (alpha <= 0)
            {
                flashing = false;
            }
        }
    }
}

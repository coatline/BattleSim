using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgText : MonoBehaviour
{
    public int damage;
    TMP_Text myText;

    void Start()
    {
        myText = GetComponent<TMP_Text>();

        myText.text = $"{damage}";

        if(damage > 5 && damage < 10)
        {
            myText.color = Color.yellow;
        }
        else if(damage >= 10)
        {
            myText.color = Color.red;
        }

        y = transform.position.y;

        x = Random.Range(-.005f, .005f);

        alpha = 1;
    }

    float alpha = 1;
    float y;
    float x;

    void Update()
    {
        transform.position = new Vector3(transform.position.x + x, y);

        y += Time.deltaTime / 3;

        alpha -= Time.deltaTime;

        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}

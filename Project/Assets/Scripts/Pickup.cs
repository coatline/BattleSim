using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    ItemData item;

    public void Setup(ItemData item)
    {
        this.item = item;
        sr.sprite = item.sprite;
    }

    public ItemData Grab()
    {
        Destroy(gameObject);
        return item;
    }
}

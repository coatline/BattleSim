using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [HideInInspector] public bool pickedUpItem;
    [SerializeField] bool canPickupItem = true;

    [SerializeField] MuzzleFlashAnimation muzzleFlashAnimation;
    [SerializeField] AudioSource itemAudioSource;
    [SerializeField] Transform handTransform;
    [SerializeField] Pickup pickupPrefab;

    [SerializeField] SpriteRenderer muzzleFlash;
    [SerializeField] SpriteRenderer itemSprite;
    [SerializeField] ItemData defaultItem;
    [SerializeField] Transform handPivot;

    public event System.Action<int> Healed;

    Vector3 initialHandPosition;
    bool recovering;
    ItemData item;

    public virtual ItemData Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;

            if (Item)
            {
                muzzleFlash.transform.localScale = new Vector2(Item.weapon.gun.muzzleFlashSize, Item.weapon.gun.muzzleFlashSize);
            }
        }
    }

    private void Awake()
    {
        initialHandPosition = handTransform.localPosition;
    }

    public virtual void Start()
    {
        if (Item == null)
        {
            Item = defaultItem;
            UpdateItemVisual();
        }
    }

    private void Update()
    {
        if (recovering)
        {
            var dist = Vector2.Distance(initialHandPosition, handTransform.localPosition);
            if (dist < .005f)
            {
                recovering = false;
            }

            float rSpeed = 5;

            if (Item)
            {
                rSpeed = Item.weapon.recoverySpeed;
            }

            handTransform.localPosition = Vector3.Lerp(handTransform.localPosition, initialHandPosition, Time.deltaTime * rSpeed);
        }
    }

    Vector3 GetOffsetFromHand(Vector2 offset)
    {
        if (Item == null) { return Vector2.zero; }

        float bX = offset.x;
        float bY = offset.y;

        Vector2 bHoleX = new Vector2(bX, bX) * itemSprite.transform.right;
        Vector2 bHoleY = new Vector2(bY, bY) * itemSprite.transform.up;
        return bHoleX + bHoleY;
    }

    public void UpdateItemVisual()
    {
        if (!Item) { itemSprite.sprite = null; return; }

        itemSprite.sprite = Item.sprite;
    }

    bool canUse = true;

    IEnumerator ItemTimer()
    {
        if (!Item) { yield return null; }
        canUse = false;
        float useTime = Item.useRate;
        yield return new WaitForSeconds(useTime);
        canUse = true;
    }

    IEnumerator Burst()
    {
        float burstTime = Item.weapon.timeBetweenAttacks;
        int bullets = Item.weapon.attacksPerBurst;
        ItemData item = Item;

        for (int i = 0; i < bullets; i++)
        {
            if (Item != item) { break; }

            handTransform.Translate(GetOffsetFromHand(Item.weapon.pushBack), Space.World);
            StartCoroutine(StartRecovery());

            for (int k = 0; k < Item.weapon.attackCount; k++)
            {
                Shoot(k);
            }

            yield return new WaitForSeconds(burstTime);
        }
    }

    public void TryUseItem(float angle, Vector2 aimVariability)
    {
        if (canUse) { Aim(angle, aimVariability); UseItem(aimVariability); }
    }

    float Aim(float angle, Vector2 aimVariability)
    {
        angle += Random.Range((float)aimVariability.x, (float)aimVariability.y);

        float flip = 0;

        if (angle + 5 > 0 || angle - 5 < -180)
        {
            flip = 180;
        }

        handPivot.rotation = Quaternion.Euler(0, 0, angle + 90);
        itemSprite.transform.localRotation = Quaternion.Euler(flip, 0, itemSprite.transform.rotation.z);

        return angle;
    }

    IEnumerator StartRecovery()
    {
        recovering = false;
        StopCoroutine(StartRecovery());
        yield return new WaitForSeconds(Item.weapon.recoveryDelay);
        recovering = true;
    }

    void UseItem(Vector2 aimVariability)
    {
        //angle = Aim(angle, aimVariability);
        if (Item == null) { return; }

        if (Item.type == ItemType.Gun)
        {
            if (Item.weapon.burst)
            {
                StartCoroutine(Burst());
            }
            else
            {
                handTransform.Translate(GetOffsetFromHand(Item.weapon.pushBack), Space.World);
                StartCoroutine(StartRecovery());

                for (int i = 0; i < Item.weapon.attackCount; i++)
                {
                    Shoot(i);
                }
            }
        }
        else if (Item.type == ItemType.Melee)
        {
            if (Item.weapon.burst)
            {
                StartCoroutine(Burst());
            }
            else
            {
                handTransform.Translate(GetOffsetFromHand(Item.weapon.pushBack), Space.World);
                StartCoroutine(StartRecovery());

                for (int i = 0; i < Item.weapon.attackCount; i++)
                {
                    Shoot(i);
                }
            }
        }
        else if (Item.type == ItemType.Health)
        {
            Healed?.Invoke(Item.healAmount);

            itemAudioSource.PlayOneShot(Item.soundOnUse.RandomSound());

            Item = null;

            UpdateItemVisual();

            return;
        }

        if (canUse)
        {
            StartCoroutine(ItemTimer());
        }
    }

    void Shoot(int bulletIndex)
    {
        float randRot = 0;
        float xOffset = 0;

        if (!Item.weapon.parallelBullets)
        {
            randRot = (-(((float)Item.weapon.attackCount * (float)Item.weapon.attackSpacing) / 2f)) + Random.Range(-Item.weapon.spread, Item.weapon.spread) + ((float)bulletIndex * Item.weapon.attackSpacing);
        }
        else
        {
            xOffset = -((Item.weapon.attackCount * Item.weapon.attackSpacing) / 2) + (bulletIndex * Item.weapon.attackSpacing);
        }

        CreateProjectile(randRot, xOffset);
    }

    public virtual Projectile CreateProjectile(float randRot, float xOffset)
    {
        if (Item.weapon.gun.muzzleFlash)
        {
            muzzleFlashAnimation.Flash(Item.weapon.gun.muzzleFlashSpeed, Item.weapon.gun.muzzleFlashSize);
        }

        var bulletHole = GetOffsetFromHand(new Vector2(Item.weapon.attackOffsetX, Item.weapon.attackOffsetY));

        muzzleFlash.transform.position = bulletHole + itemSprite.transform.position;

        var newBullet = Instantiate(Item.weapon.projectilePrefab, itemSprite.transform.position, Quaternion.Euler(itemSprite.transform.eulerAngles - new Vector3(0, 0, 90 + randRot)));

        newBullet.transform.localPosition += new Vector3(bulletHole.x, bulletHole.y);

        newBullet.transform.Translate(newBullet.transform.right * xOffset, Space.World);

        newBullet.rb.velocity = newBullet.transform.up * Item.weapon.attackForce;
        newBullet.knockBack = Item.weapon.knockBack;
        newBullet.soundOnShot = Item.soundOnUse;
        newBullet.damage = Item.weapon.damage;
        return newBullet;
    }

    public void PickupItem(ItemData item)
    {
        if (!item) { return; }

        DropItem();

        if (this.Item == item) { return; }

        this.Item = item;

        UpdateItemVisual();

        if (Item != null)
        {
            StartCoroutine(PickupItemCooldown());
        }

        pickedUpItem = true;
    }

    public virtual void DropItem()
    {
        if (!Item) { return; }

        var p = Instantiate(pickupPrefab, transform.position, Quaternion.identity);

        p.Setup(Item);

        Item = null;

        UpdateItemVisual();
    }

    IEnumerator PickupItemCooldown()
    {
        canPickupItem = false;
        //yield return new WaitForSeconds(200);
        yield return null;
        //canPickupItem = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPickupItem && collision.gameObject.CompareTag("Pickup"))
        {
            ItemData item = collision.gameObject.GetComponent<Pickup>().Grab();

            PickupItem(item);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData
{
    public float muzzleFlashSpeed;
    public float muzzleFlashSize;
    public bool muzzleFlash;
    //public float bulletSpeed;
    //public int bulletCount = 1;
    //public float spread;
    //public float bulletHoleX;
    //public float bulletHoleY;

    //[Header("Burst")]
    //public bool burst;
    //public int shotsPerBurst;
    //public float timeBetweenBullets;

    //[Header("Multi Shot")]
    //public bool parallelBullets;
    //public float bulletSpacing = .3f;
}

[System.Serializable]
public class MeleeData
{
    public MeleeAttack meleePrefab;
    public Vector2 scale;

    //[Header("Multi Shot")]
    //public bool parallelBullets;
    //public float bulletSpacing = .3f;
}

[System.Serializable]
public class WeaponData
{
    //public MeleeData melee;
    public GunData gun;
    public Projectile projectilePrefab;
    public Vector2 projectileScale;

    public int damage;

    public float attackForce;
    public float attackCount;
    public float knockBack;
    public Vector2 pushBack;
    public float recoveryDelay;
    public float recoverySpeed;
    public float spread;

    public float attackOffsetX;
    public float attackOffsetY;

    [Header("Burst")]
    public bool burst;
    public int attacksPerBurst;
    public float timeBetweenAttacks;

    [Header("Multi Shot")]
    public bool parallelBullets;
    public float attackSpacing = .3f;
}

public enum ItemType
{
    Gun,
    Melee,
    Health
}

[CreateAssetMenu(fileName = "NEW ITEM JACK", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public WeaponData weapon;
    public Sprite sprite;
    public Sound soundOnUse;

    public bool autoUse;
    public float useRate;
    public int healAmount; 
}
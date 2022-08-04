using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    enum State
    {
        ducking, //Another way to dodge
        attack,
        retreat,
        moveRight,
        moveLeft
    }

    public static event System.Action<Character> Died;

    [SerializeField] ItemHolder itemHolder;
    [SerializeField] Rigidbody2D rb;
    List<Character> characters;

    int health;

    public void Setup(List<Character> initialCharacters)
    {
        characters = new List<Character>(initialCharacters);

        CharacterSpawner.CharacterCreated += CharacterCreated;
        Character.Died += CharacterDied;
    }

    void CharacterCreated(Character c)
    {
        characters.Add(c);
    }

    void CharacterDied(Character c)
    {
        if (c == this) return;

        characters.Remove(c);
    }

    void Die()
    {
        Died?.Invoke(this);
        Destroy(gameObject);
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    protected void Attack(Vector2Int direction)
    {
        //itemHolder.TryUseItem();
    }

    protected void Move(Vector2Int velocity)
    {
        rb.velocity = velocity;
    }

    private void OnDisable()
    {
        Character.Died -= CharacterDied;
        CharacterSpawner.CharacterCreated -= CharacterCreated;
    }
}

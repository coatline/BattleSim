using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSpawner : MonoBehaviour
{
    //public static int CharacterCount { get; private set; }
    public static event Action<Character> CharacterCreated;

    [SerializeField] Character characterPrefab;
    List<Character> characters;

    private void Awake()
    {
        characters = new List<Character>();
        Character.Died += CharacterDied;
    }

    void CharacterDied(Character character)
    {
        characters.Remove(character);
    }

    public void Spawn(Vector2 position)
    {
        Character character = Instantiate(characterPrefab, new Vector3(position.x, position.y), Quaternion.identity);

        CharacterCreated?.Invoke(character);

        character.Setup(characters);

        characters.Add(character);
    }
}

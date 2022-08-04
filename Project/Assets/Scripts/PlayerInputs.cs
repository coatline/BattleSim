using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] CharacterSpawner characterSpawner;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            characterSpawner.Spawn(cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}

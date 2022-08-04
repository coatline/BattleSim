using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes
{
    // Effects the proportion of legs to body
    public float Speed { get; private set; }
    // Effects the size of arms
    public float Accuracy { get; private set; }
    public float Endurance { get; private set; }
    // Size effects melee damage
    public float Size { get; private set; }
}

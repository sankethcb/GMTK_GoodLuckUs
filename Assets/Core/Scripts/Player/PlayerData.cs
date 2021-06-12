using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerData : ScriptableObject
{
    public int GroupCount = 3;
    public List<Color> ColorSet;

    public LayerMask LevelMask;
}

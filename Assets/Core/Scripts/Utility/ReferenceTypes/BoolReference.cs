using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolReference", menuName = "Utilities/Reference Data/Bool", order = 1)]
public class BoolReference : ScriptableObject
{
    public bool value = true;

    public void SetTrue () => value = true;

    public void SetFalse () => value = false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReset : MonoBehaviour
{

    public PlayerData data;
   
    void Start()
    {
        data.GroupCount = 0;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerData
{
    public float hp = 500;

    public void SaveHp(float newhp)
    {
        if (newhp < hp)
        {
            hp = newhp;
            DataAccountPlayer.SaveDataPlayerData();
        }
    }
}

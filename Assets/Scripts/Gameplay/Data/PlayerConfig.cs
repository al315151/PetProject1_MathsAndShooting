using System;
using UnityEngine;

[Serializable]
public class PlayerConfig : IPlayerConfig
{
    public int PlayerSpeed { get; set; }

    public PlayerConfig()
    {
        PlayerSpeed = 5;
    }
}

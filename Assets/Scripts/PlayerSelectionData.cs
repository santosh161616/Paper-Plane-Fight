using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerData", menuName ="ScriptableObject/PlayerSelectionData", order =1)]
public class PlayerSelectionData : ScriptableObject
{
    public List<PlayerData> playersData;
}

[Serializable]
public class PlayerData
{
    public Sprite playerImage;
    public string PlayerName;
    public int Health;
    public int Damage;
    public int Speed;
    public int totalAds;
    public int adAlreadyShown;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrickName", menuName = "Tricks/New Trick", order = 1)]
public class Trick : ScriptableObject
{
    public string trickName;
    public string animatorTrigger;
    public string animatorFailedTrigger = "Failed";
    public PlayerAbilities abilitiesRequired;
    public int points;
    public int bonusPoints;
    public float performTime = .1f;
}

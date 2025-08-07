using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Config 1", menuName = "Config/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int Bet = 0;
    public float[] SlotRewards;
    public PlinkoSlotType SlotRewardsType;
}

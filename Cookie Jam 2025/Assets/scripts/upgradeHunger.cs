using UnityEngine;

public class UpgradeHunger : Upgrade
{

    float hungerUpgrade;

    public UpgradeHunger(string name, int[] cost, int addition) : base(name, cost, addition)
    {

    }

    public override void Skill()
    {
        Debug.Log("upgrade food");
    }
}

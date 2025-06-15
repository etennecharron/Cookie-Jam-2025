using Unity.VisualScripting;
using UnityEngine;

public class UpgradeFood : Upgrade
{

    float hungerUpgrade;

    public UpgradeFood(string name, int[] cost, int addition, float hungerUpgrade) : base(name, cost, addition)
    {
        this.hungerUpgrade = hungerUpgrade;
    }

    public float getHungerUpgrade()
    {
        return this.hungerUpgrade;
    }


    public override void Skill()
    {
        Debug.Log("upgrade food");
    }


    public override void upgradeLvl()
    {
        base.upgradeLvl();
    }

}
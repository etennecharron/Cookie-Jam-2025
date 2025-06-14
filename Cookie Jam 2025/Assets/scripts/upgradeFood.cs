using Unity.VisualScripting;
using UnityEngine;

public class UpgradeFood : Upgrade
{
    

    public UpgradeFood(string name, int[] cost, int addition) :base(name, cost, addition)
    {

    }

    public override void Skill()
    {
        Debug.Log("upgrade food");
    }

}
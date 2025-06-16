using UnityEngine;

public class UpgradeFinal : Upgrade
{


    public UpgradeFinal(string name, int[] cost) : base(name, cost,0)
    {

    }

    public override void Skill()
    {
        Debug.Log("upgrade food");
    }

}

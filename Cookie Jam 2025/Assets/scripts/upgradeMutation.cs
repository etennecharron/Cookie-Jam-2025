
using UnityEngine;

public class UpgradeMutation : Upgrade
{

    int mult = 1;

    public UpgradeMutation(string name, int[] cost) : base(name, cost, 0)
    {
        
    }
    public override int GetMult()
    {
        return mult;
    }
    public override void Skill()
    {
        Debug.Log("upgrade mutation");
      
    }
 
    public override void upgradeLvl()
    {
        if (GetLvl() < getMaxLvl())
        {
            SetAddition(getAddition() * 2);
            SetLvl(GetLvl() + 1);
        }
    }
}

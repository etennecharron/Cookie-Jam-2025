using Unity.VisualScripting;

public class UpgradeFood : Upgrade
{
    

    public UpgradeFood(string name, int lvl, int[] cost, int mult):base(name, lvl, cost, mult)
    {

    }

    public new int Skill()
    {
        return getMult();
    }

}
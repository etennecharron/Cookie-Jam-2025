using UnityEngine;
using System.Diagnostics;
using System;
public class Upgrade
{
    private int mult;
    private string name;
    private int lvl;
    private int[] cost = new int[MAXLVL];
    public const int MAXLVL = 5;

    public Upgrade(string name, int lvl, int[] cost, int mult)
    {
        this.name = name;
        this.lvl = lvl;
        this.cost = cost;
        this.mult = mult;
    }

    public int getMaxLvl()
    {
        return MAXLVL;
    }
    public int Skill()
    {
        return mult;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetLvl()
    {
        return this.lvl;
    }
    public void SetLvl(int lvl)
    {
        this.lvl = lvl;
    }
    public int GetCost(int lvl)
    {
        return this.cost[lvl];
    }
    public int getMult()
    {
        return this.mult;
    }


    public int upgradeLvl()
    {
        SetLvl(GetLvl()+1);
        return Skill();
    }
}

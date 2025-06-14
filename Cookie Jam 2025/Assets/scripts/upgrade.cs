using UnityEngine;
using System;
public class Upgrade
{
    private int addition;
    private int mult;

    private string name;
    private int lvl;
    private int[] cost;
    public int maxLvl;

    public Upgrade(string name, int[] cost, int addition)
    {
        this.name = name;
        this.cost = cost;
        this.addition = addition;
        this.maxLvl = cost.Length;
    }

    public int getMaxLvl()
    {
        return maxLvl;
    }
    public virtual void Skill()
    {
        Debug.Log("upgrades family");
    }

    public virtual int GetMult()
    {
        return mult;
    }
    public void SetMult(int mult)
    {
        this.mult = mult;
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
    public int getAddition()
    {
        return this.addition;
    }

    public void SetAddition(int mult)
    {
        this.addition = mult;
    }
    public virtual void upgradeLvl()
    {
        if (GetLvl() < getMaxLvl())
        {
            SetAddition(getAddition() * 2);
            SetLvl(GetLvl() + 1);
        }
    }
}

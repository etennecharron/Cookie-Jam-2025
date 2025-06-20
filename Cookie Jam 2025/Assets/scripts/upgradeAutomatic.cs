using System.Collections;
using UnityEngine;

public class UpgradeAutomatic : Upgrade
{
    public int second; //= 3;
    public float[] time;

    public UpgradeAutomatic(string name, int[] cost, int addition, int second) : base(name, cost, addition)
    {
        /*
        this.time = new float[cost.Length];

        int j = time.Length+1;
        for(int i = 0; i < time.Length; i++)
        {
            time[i] = second * j / time.Length;
            j--;
        }
        */
        this.second = second;
    }
    
    public float GetTime(int index)
    {
        return this.time[index];
    }
    public int GetSecond()
    {
        return this.second;
    }

    public override void Skill()
    {
        Debug.Log("ITS WORKING");
    }
    public IEnumerator FeedInterval(float interval)
    {
        while (true)
        {
            Skill();
            yield return new WaitForSeconds(interval);
        }
    }

}

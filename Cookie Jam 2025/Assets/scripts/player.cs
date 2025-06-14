using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{

    public int currency;
    public int currenyAddition;
    public int currencyMult;

    public GameObject content;
    public GameObject upgradePrefab;
    public GameObject currencyGameObject;

    public UnityEngine.UI.Button monster;

    private Coroutine automaticFeeder;


    Upgrade[] upgrades = { new Upgrade("Better setup!", new[] {10,50,200,1000,5000}, 2), new UpgradeMutation("More mouth!", new[] { 2000, 15000, 1000000 }), new UpgradeAutomatic("Hire employee!", new[] {500, 2500,10000,100000}, 1), new Upgrade("Good vitamins!", new[] {250,5500,10000,15000}, 20), new UpgradeAutomatic("Feeding machines!",new[] {5000,15000,50000,100000},50)};
    GameObject[] upgradesBtns;
    public void loadUpgrades()
    {
        upgradesBtns = new GameObject[upgrades.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            int index = i;
            GameObject upgrade = Instantiate(upgradePrefab);

            //hardcoded
            upgrade.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lvl 0 : " + upgrades[i].GetCost(upgrades[i].GetLvl()).ToString() + " $";
            //hardcoded
            upgrade.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = upgrades[i].GetName().ToString();
            //hardcoded
            upgrade.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickOnUpgrade(index));

            upgradesBtns[i] = upgrade;
            upgrade.transform.SetParent(content.transform);
        }
    }

    public void GrowBigger()
    {
        float addition = 0.005f;
        monster.GetComponent<Transform>().localScale = new Vector3(monster.GetComponent<Transform>().localScale.x + addition, monster.GetComponent<Transform>().localScale.y + addition, monster.GetComponent<Transform>().localScale.z + addition);

    }

    public void ClickOnMonster()
    {
        currency += (currenyAddition * currencyMult);
        refreshMoney();
        GrowBigger();

        /*
         * 
         * PRATIQUE A AVOIR
        Debug.Log("Addon : " + currenyAddition + " & Mult : " + currencyMult);
        */

    }


    public void ClickOnUpgrade(int index)
    {

        if (upgrades[index].GetLvl() < upgrades[index].getMaxLvl())
        {
            if (currency >= upgrades[index].GetCost(upgrades[index].GetLvl()))
            {
                currency -= upgrades[index].GetCost(upgrades[index].GetLvl());

                //get addition before lvlup so it dosnt add double;
                currenyAddition += upgrades[index].getAddition();


                /*
                 * 
                 * BEGINING SPECIAL UPGRADE
                 * 
                 */

                //if multable 
                if (upgrades[index].getAddition() == 0)
                {
                    currencyMult += upgrades[index].GetMult();

                    upgrades[index].upgradeLvl();
                }
                else if (upgrades[index] is UpgradeAutomatic automatic)
                {
                    if(automatic.GetLvl() == 0)
                    {


                        automaticFeeder = StartCoroutine(FeedInterval(automatic.GetTime(automatic.GetLvl()), index));
                        automatic.upgradeLvl();
                    }
                    else
                    {
                       StopCoroutine(automaticFeeder);
        
                        StartCoroutine(FeedInterval(automatic.GetTime(automatic.GetLvl()), index));
                        automatic.upgradeLvl();
                    }
                }
                else
                {
                    upgrades[index].Skill();
                    upgrades[index].upgradeLvl();
                }
                /*
                * 
                * END SPECIAL UPGRADE
                * 
                */
                refreshMoney();

                if (upgrades[index].GetLvl() < upgrades[index].getMaxLvl())
                {
                    RefreshPriceUpgrade(index);
                }
                else
                {
                    upgradesBtns[index].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MAX";
                }

            }
        }

    }

    public IEnumerator FeedInterval(float interval,int index)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            // GETS THE CURRENT MULT SINCE IT HAS MULTIPLE MOUTH SO I THINK IT MAKES SENSE THEY ALSO GET THE MULT
            currency += (upgrades[index].getAddition() * currencyMult);
            refreshMoney();
            GrowBigger();

        }
    }
    public void refreshMoney()
    {
        currencyGameObject.GetComponent<TextMeshProUGUI>().text = writeNumber(currency);
    }
    public void RefreshPriceUpgrade(int index)
    {
        upgradesBtns[index].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lvl " + upgrades[index].GetLvl() + " : " + writeNumber(upgrades[index].GetCost(upgrades[index].GetLvl())) + " $";

    }

    public string writeNumber(int number)
    {
        String text = "";

        if (number < Math.Pow(10, 3))
        {
            text = number.ToString();
        }
        else if (number >= Math.Pow(10, 3) && number <= Math.Pow(10, 6))
        {
            text = Math.Round((number / Math.Pow(10, 3)), 2).ToString() + "K";
        }
        else if (number >= Math.Pow(10, 6) && number <= Math.Pow(10, 9))
        {
            text = Math.Round((number / Math.Pow(10, 6)), 2).ToString() + "M";
        }
        else if (number >= Math.Pow(10, 9) && number <= Math.Pow(10, 12))
        {
            text = Math.Round((number / Math.Pow(10, 9)), 2).ToString() + "T";
        }
        else if (number >= Math.Pow(10, 12) && number <= Math.Pow(10, 15))
        {
            text = Math.Round((number / Math.Pow(10, 12)), 2).ToString() + "Qa";
        }

        return text;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currency = 0;
        currenyAddition = 1;
        currencyMult = 1;
        loadUpgrades();


    }

    // Update is called once per frame
    void Update()
    {

    }
}

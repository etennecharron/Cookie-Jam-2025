using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{

    public int currency;
    public int currenyMult;

    public GameObject content;
    public GameObject upgradePrefab;
    public GameObject currencyGameObject;

    public UnityEngine.UI.Button monster;



    Upgrade[] upgrades = {new UpgradeFood("Better food!", 0, new[] {20,100,500,1000,2000},1), new UpgradeFood("BASDASDAS!", 0, new[] { 20, 100, 500, 1000, 2000 }, 1), new UpgradeFood("BeHBVCBCVood!", 0, new[] { 20, 100, 500, 1000, 2000 }, 1) };
    GameObject[] upgradesBtns;
    public void loadUpgrades()
    {
        upgradesBtns = new GameObject[upgrades.Length];

        for(int i = 0; i < upgrades.Length; i++)
        {
            int index = i;
           GameObject upgrade = Instantiate(upgradePrefab);

            //hardcoded
            upgrade.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = upgrades[i].GetCost(upgrades[i].GetLvl()).ToString() + "$";
            //hardcoded
             upgrade.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = upgrades[i].GetName().ToString();
            //hardcoded
            upgrade.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=>ClickOnUpgrade(index));

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
        currency += currenyMult;
        refreshMoney();
        GrowBigger();
    }

    
    public void ClickOnUpgrade(int index)
    {
        Debug.Log(index);
        
        if(currency >= upgrades[index].GetCost(upgrades[index].GetLvl()) && upgrades[index].GetLvl() < upgrades[index].getMaxLvl())
        {
            currency -= upgrades[index].GetCost(upgrades[index].GetLvl());

            int multToAdd = upgrades[index].upgradeLvl();
            currenyMult += multToAdd;

            refreshMoney();
            RefreshPriceUpgrade(index);
        }
    }

    public void refreshMoney()
    {
        currencyGameObject.GetComponent<TextMeshProUGUI>().text = currency.ToString();
    }
    public void RefreshPriceUpgrade(int index)
    {
        upgradesBtns[index].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = upgrades[index].GetCost(upgrades[index].GetLvl()).ToString() + "$";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currency = 0;
        currenyMult = 1;
        loadUpgrades();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

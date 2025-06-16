using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Android.AndroidGame;

public class player : MonoBehaviour
{

    public long currency;
    public int currencyAddition;
    public int currencyMult;

    public GameObject content;
    public GameObject upgradePrefab;
    public GameObject currencyGameObject;

    public UnityEngine.UI.Button monster;

    private Coroutine automaticFeeder;

    float sizeAddition = 0.01f;
    float sizeDiminution = 0.005f;

    float diminutionTime = 0.1f;

    public GameObject textBubbleContainer;
    public GameObject textBubble;

    public Sprite[] food;

    public UnityEngine.UI.Button startButton;
    public GameObject startScreen;
    public GameObject gameScreen;
    public GameObject endScreen;
    public GameObject deathScreen;

    public float[] sizes;
    public int sizeLvl;
    public bool upgradeableSize;
    public Sprite[] monstreEvolutions;
    private int growBtnIndex;

    private int upgradeFinalIndex;
    private bool finalUpgradeAvailable;
    private int finalSize = 6;

    Upgrade[] upgrades = { new UpgradeFood("Better food!", new[] {50,5000,2000,10000,50000,100000}, 1,0.001f),
        new UpgradeMutation("EVOLVE!", new[] {3,4,5}),
        new UpgradeFinal("GRAND OFFERING", new[]{1000000}),
        new Upgrade("Good humor!", new[]{150},1),
        new Upgrade("Buy new furnitures", new[]{1200},1),
        new UpgradeAutomatic("Hire a friend!",new[] {750},2, 3),
        new Upgrade("Good vitamins!",new[] {300}, 3),
        new Upgrade("Buy a cat!", new[]{1500},2),
        new UpgradeAutomatic("Feeding machines!",
        new[] {1},2,50)};


    GameObject[] upgradesBtns;
    public void loadUpgrades()
    {
        upgradesBtns = new GameObject[upgrades.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            int index = i;
            GameObject upgrade = Instantiate(upgradePrefab);

            //hardcoded
            upgrade.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lvl 0 : " + writeNumber(upgrades[i].GetCost(upgrades[i].GetLvl())) + " $";
            //hardcoded
            upgrade.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = upgrades[i].GetName().ToString();
            //hardcoded
            upgrade.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickOnUpgrade(index));

            upgradesBtns[i] = upgrade;
            upgrade.transform.SetParent(content.transform);

            if (upgrades[i] is UpgradeMutation mutation)
            {
                sizes = new float[mutation.GetCostLength()];
                for (int j = 0; j < mutation.GetCostLength(); j++)
                {
                    sizes[j] = mutation.GetCost(j);
                }
                upgrade.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Evol 1: GROW BIG";
                growBtnIndex = i;
            }
            if (upgrades[i] is UpgradeFinal final)
            {
                upgrade.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "GROW BIG + " +writeNumber(upgrades[i].GetCost(upgrades[i].GetLvl())) + " $";
                upgradeFinalIndex = i;
                upgrade.SetActive(false);
            }
        }
    }

    public void GrowBigger()
    {

        monster.GetComponent<Transform>().localScale = new Vector3(monster.GetComponent<Transform>().localScale.x + sizeAddition, monster.GetComponent<Transform>().localScale.y + sizeAddition, monster.GetComponent<Transform>().localScale.z + sizeAddition);

        if (monster.GetComponent<Transform>().localScale.x > 0.90f && monster.GetComponent<Transform>().localScale.x < 1f)
        {
            textBubbleContainer.SetActive(true);
            writeTextBubble("FEED ME");
        }
        else if (monster.GetComponent<Transform>().localScale.x > 1.5f)
        {
            textBubbleContainer.SetActive(false);
        }
        if (sizeLvl < sizes.Length)
        {
            if (monster.GetComponent<Transform>().localScale.x > sizes[sizeLvl])
            {
                upgradesBtns[growBtnIndex].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
                upgradeableSize = true;

            }
        }
        if (upgradesBtns[upgradeFinalIndex].activeInHierarchy == true && monster.GetComponent<Transform>().localScale.x > finalSize)
        {
            finalUpgradeAvailable = true;
            upgradesBtns[upgradeFinalIndex].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
        }

    }
    public void GrowSmaller()
    {
        if(monster.GetComponent<Transform>().localScale.x < 1.3f && monster.GetComponent<Transform>().localScale.x > 1f)
        {
            textBubbleContainer.SetActive(true);
            writeTextBubble("FEED ME");
        }

        if (monster.GetComponent<Transform>().localScale.x > 0.4f)
        {
            monster.GetComponent<Transform>().localScale = new Vector3(monster.GetComponent<Transform>().localScale.x - sizeDiminution, monster.GetComponent<Transform>().localScale.y - sizeDiminution, monster.GetComponent<Transform>().localScale.z - sizeDiminution);

            if (monster.GetComponent<Transform>().localScale.x < 0.75f && monster.GetComponent<Transform>().localScale.x > 0.50f)
            {
                writeTextBubble("D:");
            }
            else if (monster.GetComponent<Transform>().localScale.x < 0.50f)
            {
                writeTextBubble("X - X");
            }
        }
        else
        {
            deathScreen.SetActive(true);
            gameScreen.SetActive(false);

        }


    }

    public void ClickOnMonster()
    {
        currency += (currencyAddition * currencyMult);
        refreshMoney();
        GrowBigger();

        /*
         * 
         * PRATIQUE A AVOIR
        Debug.Log("Addon : " + currenyAddition + " & Mult : " + currencyMult);
        */

    }

    public void writeTextBubble(string message)
    {

        textBubble.GetComponent<TextMeshProUGUI>().text = message;

    }
    public void ClickOnUpgrade(int index)
    {

        if (upgrades[index].GetLvl() < upgrades[index].getMaxLvl())
        {
            if (currency >= upgrades[index].GetCost(upgrades[index].GetLvl()))
            {
                currency -= upgrades[index].GetCost(upgrades[index].GetLvl());

                /****************************************** BEGINING SPECIAL UPGRADE*****************************************************/


                //if multable 
                if (upgrades[index] is UpgradeMutation mutation)
                {
                    if (upgradeableSize == true)
                    {
                        upgradeableSize = false;
                        sizeLvl++;
                        if (sizeLvl < monstreEvolutions.Length)
                        {
                            monster.GetComponent<UnityEngine.UI.Image>().sprite = monstreEvolutions[sizeLvl];
                        }

                        currencyMult += upgrades[index].GetMult();
                        upgrades[index].upgradeLvl();

                        upgradesBtns[index].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Evol " + (sizeLvl + 1) + " : GROW BIGGER";

                        monster.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                        upgradesBtns[growBtnIndex].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.black;
                    }
                }
                // IF IS AN AUTOMATIC UPGRADE TYPE
                else if (upgrades[index] is UpgradeAutomatic automatic)
                {
                    if (automatic.GetLvl() == 0)
                    {
    
                        automaticFeeder = StartCoroutine(FeedInterval(automatic.GetSecond(), index));
                        automatic.upgradeLvl();
                    }
                    /*
                    else
                    {
                        StopCoroutine(automaticFeeder);

                        StartCoroutine(FeedInterval(automatic.GetTime(automatic.GetLvl()), index));
                        automatic.upgradeLvl();
                    }*/
                }
                //IF ITS A FOODUPGRADE
                else if (upgrades[index] is UpgradeFood foodUpgrader)
                {
                    //get addition before lvlup so it dosnt add double;
                    currencyAddition += upgrades[index].getAddition();

                    if (foodUpgrader.GetLvl() == 0)
                    {
                        upgradesBtns[index].transform.GetChild(3).gameObject.SetActive(true);
                    }
                    // Debug.Log(upgradesBtns[index].transform.GetChild(3).GetComponent<RawImage>());

                    upgradesBtns[index].transform.GetChild(3).GetComponent<UnityEngine.UI.Image>().sprite = food[foodUpgrader.GetLvl()];
                    sizeAddition += foodUpgrader.getHungerUpgrade();
                    upgrades[index].upgradeLvl();
                }
                else if (upgrades[index] is UpgradeFinal final)
                {
                    if (finalUpgradeAvailable == true)
                    {
                        gameScreen.SetActive(false);
                        endScreen.SetActive(true);

                    }
                }
                //IF ITS ANY BASIC UPGRADE
                else
                {
                    //get addition before lvlup so it dosnt add double;
                    currencyAddition += upgrades[index].getAddition();
                    upgrades[index].Skill();
                    upgrades[index].upgradeLvl();
                }
                /****************************************** END SPECIAL UPGRADE*****************************************************/

                refreshMoney();

                if (upgrades[index].GetLvl() < upgrades[index].getMaxLvl())
                {
                    if (!(upgrades[index] is UpgradeMutation))
                    {
                        RefreshPriceUpgrade(index);
                    }

                }
                else
                {
                    if (upgrades[index] is UpgradeMutation)
                    {
                        upgradesBtns[upgradeFinalIndex].SetActive(true);

                    }
                    upgradesBtns[index].gameObject.SetActive(false);
                   // upgradesBtns[index].transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MAX";
                }

            }
        }

    }


    public IEnumerator FeedInterval(float interval, int index)
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

    public IEnumerator LosingWeigth(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            GrowSmaller();

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

    public string writeNumber(long number)
    {
        String text = "";

        if (number < Math.Pow(10, 3))
        {
            text = number.ToString();
        }
        else if (number >= Math.Pow(10, 3) && number < Math.Pow(10, 6))
        {
            text = Math.Round((number / Math.Pow(10, 3)), 2).ToString() + "K";
        }
        else if (number >= Math.Pow(10, 6) && number < Math.Pow(10, 9))
        {
            text = Math.Round((number / Math.Pow(10, 6)), 2).ToString() + "M";
        }
        else if (number >= Math.Pow(10, 9) && number < Math.Pow(10, 12))
        {
            text = Math.Round((number / Math.Pow(10, 9)), 2).ToString() + "T";
        }
        else if (number >= Math.Pow(10, 12) && number < Math.Pow(10, 15))
        {
            text = Math.Round((number / Math.Pow(10, 12)), 2).ToString() + "Qa";
        }

        return text;
    }


    public void Initialise()
    {
        currency = 0;
        currencyAddition = 1;
        currencyMult = 1;
        sizeLvl = 0;
        loadUpgrades();
        upgradeableSize = false;

        writeTextBubble("FEED ME");

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].SetLvl(0);
        }

        monster.GetComponent<UnityEngine.UI.Image>().sprite = monstreEvolutions[0];

        StartCoroutine(LosingWeigth(diminutionTime));
        refreshMoney();
    }


    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Initialise();
            startScreen.SetActive(false);
            gameScreen.SetActive(true);
        });



    }

















    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


class Computer : MonoBehaviour
{
    [Header("Фон")]
    public Sprite backgr;
    [Header("ОУ на майнере")]
    public GameObject PU;
    public Text PU_text;

    Vector2 pointSt;
    public GameObject plusXP;

    public Text currencyText; //Деньги в валюте
    public Text progressText; //Текст процентного заполнения кликов
    public Text bonusText; //Текст стоимость улучшения
    public Text expText; //Текст количества опыта
    public GameObject partsContainer; //Контейнер, содержащий части компьютера
    public GameObject partPrefab;
    public GameObject infPref; //префаб бесконечной загрузки 
    public GameObject block; //блокировка майнинга, если не куплены все необходимые части

    public Image p1; //полоса прогресса заполнения кликов
    public Image p2; //полоса прогресса заполнения опыта
    public GameObject progressPanel; //панель для кликов
    public Button bonusButton; //кнопка покупки улучшения
    public Button buyComp; //кнопка покупки компьютера



    public Game g;

    public int progressCounter1 = 0;
    public int progressCounter2 = 0;

    public Currency cur;
    public float currency = 0;

    //процент от времени заполнения автомайнера при полном наборе видеокарт
    public int fullGPU = 40;

    public int cost = 1; // цена компьютера
    public string nameComp = "Ноутбук"; // Имя компьютера
    public bool isReady; // готов ли компьютер к работе (все ли компоненты куплены)
    public bool isFarm; // ферма?
    public bool isResearched = true; // открыт ли компьютер
    public float bonus = 0.001f; // доход за заполнение прогресс-бара
    public float bonusCost = 2; // цена улучшения (на которую поднимаем доход)
    float bonusD = 1.32f;
    float bonusCostD = 1.44f;
    public int maxClick = 10; // кол-во дробей прогресс-бара

    int expD = 50; // кол-во опыта за клик по кнопке
    int expU = 15; // кол-во опыта за улучшение доходности
    public int upgradeCost = 200; //максимальное число EXP. Начало с 200
    float upgradeCoef = 1.16f; // коеф. сложности upgradeCost, для ноута - 1,12. 
    public int upgradePoints = 0; // кол-во очков улучшений (ОУ)
    //удалить 
    public int efficiency = 100;
    public int progressCounter = 0; // счётчик заполнения прогрессбара

    /*-- появляется после покупки автомайна --*/
    public float timeUpgrade = 30; // Ускорение майна. Начинается с 30% и падает на 2,5% каждое улучшение
    float timeUpgradeD = 2.5f;
    public float profiteUpgrade = 35; // Увеличение доходности майнинга. Начинается с 35% и падает на 2.5% каждое улучшение
    float profiteUpgradeD = 2.5f;
    /*!-- появляется после покупки автомайна --*/

    public int level = 1; // Уровень развития компьютера. Повышается на +1 каждые 50 кликов. Не зависит от EXP 

    public AutoMiner autoMiner;

    /*Оффлайн автомайнер*/
    int offMinerCost = 150;
    public bool isBoughtOff;
    public float offProfit = 0.00001f;
    public float offProfitBonus = 50;
    float offProfitBonusD = 2.5f;
    /********************/

    public bool isBoughtAutoRepair = false; //Куплена ли автопочинка 
    public int autoRepairCost = 30;

    string jsonParts;
    public PartsOfComputer[] compParts;

    public GameObject _level;

    public void LevelUp()
    {
        _level.SetActive(true);
        _level.GetComponent<Animator>().SetBool("Level", true);
    }

    public GameObject clickParent;
    public GameObject clickTextPrefab;
    private clickObj[] clickTextPool = new clickObj[10];
    public GameObject Empty;
    private int ClickNum;
    Transform impr;

    public void Translator()
    {
        gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.game[0];
    }

    void Start()
    {
        //Translator();
        impr = g.improvementWin.transform.GetChild(1);
        clickParent.transform.localScale = new Vector2(1, 1);

        for (int i = 0; i < clickTextPool.Length; i++)
        {
            Empty = Instantiate(clickTextPrefab, clickParent.transform) as GameObject;
            clickTextPool[i] = Empty.GetComponent<clickObj>();
            //clickTextPool[i] = Instantiate(clickTextPrefab, clickParent.transform).ga
        }

        g.moneyText.text = "$" + g.money.ToString("0.#0");

        this.cur = g.currencies[0];

        Text t = transform.Find("ProgressPanel/currencyName").gameObject.GetComponent<Text>();
        t.text = cur.getName();
        this.currencyText = transform.Find("ProgressPanel/currencyText").gameObject.GetComponent<Text>();
        this.progressText = transform.Find("ProgressPanel/clickProgressbar/clickText").gameObject.GetComponent<Text>();
        this.bonusText = transform.Find("BonusPanel/BonusCost").gameObject.GetComponent<Text>();
        this.expText = transform.Find("ProgressPanel/xpProgressbar/xpText").gameObject.GetComponent<Text>();
        //this.improvementWin = transform.Find("ImprovementWin").gameObject;
        this.progressPanel = transform.Find("ProgressPanel").gameObject;
        this.p1 = transform.Find("ProgressPanel/clickProgressbar/Foreground").gameObject.GetComponent<Image>();
        this.p2 = transform.Find("ProgressPanel/xpProgressbar/Foreground").gameObject.GetComponent<Image>();
        this.bonusButton = transform.Find("BonusPanel/BonusButton").gameObject.GetComponent<Button>();
        this.buyComp = transform.Find("BuyComp").gameObject.GetComponent<Button>();
        this.partsContainer = g.improvementWin.transform.Find("Background/Parts").gameObject;
        buyComp.transform.Find("Text").gameObject.GetComponent<Text>().text = "$" + cost;
        bonusButton.onClick.AddListener(GetBonus);
        buyComp.gameObject.SetActive(!isResearched);
        buyComp.onClick.AddListener(ResearchComp);
        transform.Find("ProgressPanel/nameText").gameObject.GetComponent<Text>().text = nameComp;
        expText.text = progressCounter2.ToString() + "/" + upgradeCost.ToString() + "xp";
        p2.fillAmount = (float)progressCounter2 / (float)upgradeCost;
        bonusText.text = "$" + (bonusCost).ToString("0.#0");
        if (isReady)
        {
            if (autoMiner.isBoughtAuto)
            {
                StartCoroutine(BonusPerSec());
            }
            block.SetActive(false);
        }
        else
        {
            block.SetActive(true);
        }
        //autoMiner = new AutoMiner(1, "AutoMiner", 5, bonus);
        updateUp();
    }

    private string conversionFunction(float number)
    {
        string converted = number.ToString("#,##0");
        converted = converted.Replace(",", " ");
        return converted;
    }

    public void Update()
    {
        currencyText.text = currency.ToString("#0.###0");
        bonusButton.interactable = (g.money >= bonusCost);
        buyComp.interactable = (g.money >= cost);
        p1.fillAmount = (float)progressCounter1 / 100;
        progressText.text = progressCounter1.ToString() + "%";
        if (g.money > 1000000000 && g.money < 999999999999) // млрд
        {
            g.moneyText.text = "$" + ((g.money / 1000000000)).ToString("0.#0") + " " + LangSystem.lng.game[4];
        }
        else if (g.money > 1000000 && g.money < 999999999)// млн
        {
            g.moneyText.text = "$" + ((g.money / 1000000)).ToString("0.#0") + " " + LangSystem.lng.game[3];
        }
        else if (g.money > 10000 && g.money <= 999999) //тыс
        {
            g.moneyText.text = "$" + conversionFunction(g.money);
        }
        else if (g.money <= 99999)
        {
            g.moneyText.text = "$" + g.money.ToString("0.#0");
        }
    }

    public void updateUp()
    {
        if (upgradePoints > 0)
        {
            PU.SetActive(true);
            PU_text.text = LangSystem.lng.game[1] + ": " + upgradePoints.ToString();
        }
        else
        {
            PU.SetActive(false);
        }
    }

    public void OnClick()
    {

        g.clickCounter++;
        if (g.clickCounter == 100) g.gameObject.GetComponent<Achievment>().unlockAch(0);
        if (g.clickCounter == 10000) g.gameObject.GetComponent<Achievment>().unlockAch(6);
        if (g.clickCounter == 1000)
        {
            g.rateWin.SetActive(true);
        }
        g.exp += expD;
        checkEXP();
        progressCounter1 = progressCounter1 + (100 / maxClick);
        progressCounter2 += expD;
        if (progressCounter1 > 100)
        {
            progressCounter1 = 0;
            if (g.isProfitBoosterOn)
            {
                currency = currency + bonus * 5;
                this.cur.sum = this.cur.sum + bonus * 5;
            }
            else
            {
                currency += bonus;
                this.cur.sum += bonus;
            }

            if ((!g.gameObject.GetComponent<Achievment>().achievment[5].get) && cur.sum >= 1000) g.gameObject.GetComponent<Achievment>().unlockAch(5);
            progressCounter++;
            if (g.isAutoExchangerOn)
            {
                Exchange();
            }
        }
        if (progressCounter2 > upgradeCost)
        {
            level++;
            LevelUp();
            if (isFarm && level == 10 && (!g.gameObject.GetComponent<Achievment>().achievment[7].get)) g.gameObject.GetComponent<Achievment>().unlockAch(7);
            if (maxClick > 1) maxClick--;
            progressCounter2 = progressCounter2 % upgradeCost;
            upgradePoints++;
            upgradeCost = (int)(upgradeCost * upgradeCoef);
            updateUp();
        }
        expText.text = progressCounter2.ToString() + "/" + upgradeCost.ToString() + "xp";
        p2.fillAmount = (float)progressCounter2 / (float)upgradeCost;
        if (progressCounter > 5)
        {
            progressCounter = 0;
            foreach (PartsOfComputer p in compParts)
            {
                if (p.isBought)
                {
                    // p.isBought = rnd.Next(1, 100) > p.curReliability;
                    p.isBought = Random.Range(1, 100) > p.curReliability;
                    p.isBroken = !p.isBought;
                    if (p.isBroken && isBoughtAutoRepair && (g.money >= g.parts[p.id].costNew))
                    {
                        g.money -= g.parts[p.id].costNew;
                        p.curReliability = g.parts[p.id].reliabilityNew;
                        p.isBought = true;
                        p.isBroken = false;
                        g.moneyText.text = "$" + g.money.ToString("0.#0");
                        g.partCount++;
                        if (g.partCount == 100) g.gameObject.GetComponent<Achievment>().unlockAch(4);
                        checkInteractable();
                    }
                }
            }
            isReady = checkIsReady();
            block.SetActive(!isReady);
            if (!isReady)
            {
                infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 0;
                if (isFarm)
                {
                    g.farmCount--;
                    if (g.farmCount == 0)
                    {
                        unblockLast();
                    }
                }
                GameObject prevComp = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 1).gameObject;
                if ((isFarm && g.farmCount == 0 && !prevComp.GetComponent<Computer>().isFarm) || (!isFarm))
                {
                    prevComp.transform.Find("BlockMining").gameObject.SetActive(false);
                    Computer pr = prevComp.GetComponent<Computer>();
                    pr.isReady = true;
                    if (!pr.autoMiner.isBoughtAuto)
                    {
                        g.autoMinerButton.interactable = (g.money >= pr.autoMiner.autoCost);
                    }
                    else
                    {
                        StopCoroutine(pr.BonusPerSec());
                        StartCoroutine(pr.BonusPerSec());
                    }
                }
                pushBroken();
            }
            if (g.improvementWin.activeSelf && gameObject.transform.GetSiblingIndex() == g.compId)
            {
                changeBuyButtons();
            }
        }
        //Вылетающие клики        
        clickTextPool[ClickNum].StartMotion(expD);
        ClickNum = ClickNum == clickTextPool.Length - 1 ? 0 : ClickNum + 1;
        //pointSt = transform.GetChild(0).transform.GetChild(Random.Range(0, 3)).position;
        //Debug.Log(pointSt);
        //GameObject expPr = (GameObject)Instantiate(plusXP, pointSt, Quaternion.identity);
        //expPr.GetComponentInChildren<Text>().text = "+" + expD.ToString() + "xp";

    }

    private void unblockLast()
    {
        GameObject miners = GameObject.FindWithTag("Miners").gameObject;
        Computer last;
        int i = 0;
        while (!miners.transform.GetChild(i).GetComponent<Computer>().isFarm) i++;
        last = miners.transform.GetChild(i - 1).GetComponent<Computer>();
        last.isReady = true;
        last.unblock();
    }

    private bool checkLast()
    {
        GameObject miners = GameObject.FindWithTag("Miners").gameObject;
        Computer last;
        int i = 0;
        while (!miners.transform.GetChild(i).GetComponent<Computer>().isFarm) i++;
        last = miners.transform.GetChild(i - 1).GetComponent<Computer>();
        return last.isReady;
    }

    private void blockLast()
    {
        GameObject miners = GameObject.FindWithTag("Miners").gameObject;
        Computer last;
        int i = 0;
        while (!miners.transform.GetChild(i).GetComponent<Computer>().isFarm) i++;
        last = miners.transform.GetChild(i - 1).GetComponent<Computer>();
        last.isReady = false;
        infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 0;
        last.block.SetActive(true);
    }

    public void GetBonus()
    {
        g.exp += expU;
        checkEXP();
        progressCounter2 += expU;
        if (progressCounter2 > upgradeCost)
        {
            level++;
            if (maxClick > 1) maxClick--;
            progressCounter2 = progressCounter2 % upgradeCost;
            upgradePoints++;
            upgradeCost = (int)(upgradeCost * upgradeCoef);
            updateUp();
        }
        expText.text = progressCounter2.ToString() + "/" + upgradeCost.ToString() + "xp";
        p2.fillAmount = (float)progressCounter2 / (float)upgradeCost;
        g.money = g.money - bonusCost;
        g.moneyText.text = "$" + g.money.ToString("0.#0");
        bonusCost = bonusCost * bonusCostD;
        autoMiner.autoProfit = autoMiner.autoProfit - bonus;
        bonus = bonus * bonusD;
        autoMiner.autoProfit = autoMiner.autoProfit + bonus;
        bonusText.text = "$" + (bonusCost).ToString("0.#0");
        //Вылетающие бонусы
        pointSt = transform.GetChild(0).transform.GetChild(UnityEngine.Random.Range(0, 3)).position;
        GameObject expPr = (GameObject)Instantiate(plusXP, pointSt, Quaternion.identity);
        expPr.GetComponentInChildren<Text>().text = "+" + expU.ToString() + "xp";

    }
    public void Exchange()
    {
        g.money = (g.money + currency * cur.getExchRate());
        g.sumMoney = (g.sumMoney + currency * cur.getExchRate());
        if ((!g.gameObject.GetComponent<Achievment>().achievment[1].get) && g.sumMoney >= 5000f) g.gameObject.GetComponent<Achievment>().unlockAch(1);
        if ((!g.gameObject.GetComponent<Achievment>().achievment[1].get) && g.sumMoney >= 1000000000f) g.gameObject.GetComponent<Achievment>().unlockAch(8);
        currency = 0;
        g.moneyText.text = "$" + g.money.ToString("0.#0");
    }
    public void BuyAutoMiner()
    {
        g.money = g.money - autoMiner.autoCost;
        autoMiner.isBoughtAuto = true;
        g.autoMinerButton.interactable = false;
        autoMiner.autoTime = ((float)(200 - getEfficiency()) / 100) * autoMiner.maxTime;
        autoMiner.autoProfit = bonus;
        StartCoroutine(BonusPerSec());
        g.upgradeTimeButton.interactable = (upgradePoints > 0);
        g.upgradeProfitButton.interactable = (upgradePoints > 0);
        g.moneyText.text = "$" + g.money.ToString("0.#0");
        g.time.text = (autoMiner.autoTime - autoMiner.timeBonus).ToString() + " " + LangSystem.lng.improvementWins[1];
        g.sr_pr.text = ((autoMiner.autoProfit) / (autoMiner.autoTime - autoMiner.timeBonus)).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
        g.pribyl.text = cur.getName() + " " + (autoMiner.autoProfit).ToString("#0.###0");
        g.improvementWin.transform.Find("Background/AutoMiner/GameObject/checkmark").GetComponent<check>().setOn();
        g.improvementWin.transform.Find("Background/AutoMiner/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = "";
        g.offMinerButton.interactable = (g.money >= offMinerCost) && !isBoughtOff;
        changeBuyButtons();
    }

    //Покупка автопочинки
    public void BuyAutoRepair()
    {
        g.money = g.money - autoRepairCost;
        isBoughtAutoRepair = true;
        g.autoRepairButton.interactable = false;
        g.moneyText.text = "$" + g.money.ToString("0.#0");
        g.improvementWin.transform.Find("Background/AutoRepair/GameObject/checkmark").GetComponent<check>().setOn();
        g.improvementWin.transform.Find("Background/AutoRepair/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = "";
    }

    public void BuyOffMiner()
    {
        g.money = g.money - offMinerCost;
        isBoughtOff = true;
        g.offMinerButton.interactable = false;
        g.offMinerBonusButton.interactable = (upgradePoints > 0);
        g.moneyText.text = "$" + g.money.ToString("0.#0");
        g.improvementWin.transform.Find("Background/NightMiner/GameObject/checkmark").GetComponent<check>().setOn();
        g.improvementWin.transform.Find("Background/NightMiner/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = "";
        g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost) && !autoMiner.isBoughtAuto;
        changeBuyButtons();
    }

    public void checkInteractable()
    {
        g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost) && !autoMiner.isBoughtAuto && isReady;
        g.offMinerButton.interactable = (g.money >= offMinerCost) && !isBoughtOff && isReady;
        g.autoRepairButton.interactable = (g.money >= autoRepairCost) && !isBoughtAutoRepair && isReady;
    }

    IEnumerator BonusPerSec()
    {
        while (isReady)
        {
            infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 1;
            if (g.isTimeBoosterOn)
            {
                if (((autoMiner.autoTime - autoMiner.timeBonus) / 500) > 0.009f)
                {
                    progressCounter1++;
                }
                else
                {
                    int d = (int)((1 - ((autoMiner.autoTime - autoMiner.timeBonus) / 5)) * 10);
                    progressCounter1 = progressCounter1 + d;
                }
            }
            else
            {
                if (((autoMiner.autoTime - autoMiner.timeBonus) / 100) > 0.009f)
                {
                    progressCounter1++;
                }
                else
                {
                    int d = (int)((1 - (autoMiner.autoTime - autoMiner.timeBonus)) * 10);
                    progressCounter1 = progressCounter1 + d;
                }
            }
            if (progressCounter1 > 100)
            {
                progressCounter1 = 0;
                if (g.isProfitBoosterOn)
                {
                    currency = currency + autoMiner.autoProfit * 5;
                    this.cur.sum = this.cur.sum + autoMiner.autoProfit * 5;
                }
                else
                {
                    currency = currency + autoMiner.autoProfit;
                    this.cur.sum += autoMiner.autoProfit;
                }

                if ((!g.gameObject.GetComponent<Achievment>().achievment[5].get) && cur.sum >= 1000) g.gameObject.GetComponent<Achievment>().unlockAch(5);
                p1.fillAmount = (float)progressCounter1 / 100;
                progressText.text = progressCounter1.ToString() + "%";
                progressCounter++;
                if (g.isAutoExchangerOn)
                {
                    Exchange();
                }
            }
            if (progressCounter > 5)
            {
                progressCounter = 0;
                foreach (PartsOfComputer p in compParts)
                {
                    if (p.isBought)
                    {
                        System.Random rnd = new System.Random();
                        p.isBought = rnd.Next(1, 100) > p.curReliability;
                        p.isBroken = !p.isBought;
                        if (p.isBroken && isBoughtAutoRepair && (g.money >= g.parts[p.id].costNew))
                        {
                            g.money -= g.parts[p.id].costNew;
                            p.curReliability = g.parts[p.id].reliabilityNew;
                            p.isBought = true;
                            p.isBroken = false;
                            g.moneyText.text = "$" + g.money.ToString("0.#0");
                            g.partCount++;
                            if (g.partCount == 100) g.gameObject.GetComponent<Achievment>().unlockAch(4);
                            checkInteractable();
                        }
                    }
                }
                isReady = checkIsReady();
                block.SetActive(!isReady);
                if (!isReady)
                {
                    infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 0;
                    if (isFarm)
                    {
                        g.farmCount--;
                        if (g.farmCount == 0)
                        {
                            unblockLast();
                        }
                    }
                    GameObject prevComp = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 1).gameObject;
                    if ((isFarm && g.farmCount == 0 && !prevComp.GetComponent<Computer>().isFarm) || (!isFarm))
                    {
                        prevComp.transform.Find("BlockMining").gameObject.SetActive(false);
                        Computer pr = prevComp.GetComponent<Computer>();
                        pr.isReady = true;
                        if (!pr.autoMiner.isBoughtAuto)
                        {
                            g.autoMinerButton.interactable = (g.money >= pr.autoMiner.autoCost);
                        }
                        else
                        {
                            StopCoroutine(pr.BonusPerSec());
                            StartCoroutine(pr.BonusPerSec());
                        }
                    }
                    pushBroken();
                }
                if (g.improvementWin.activeSelf && gameObject.transform.GetSiblingIndex() == g.compId)
                {
                    changeBuyButtons();
                }
            }
            if ((autoMiner.autoTime - autoMiner.timeBonus) < 0.57f)
            {
                infPref.SetActive(true);
                progressText.text = LangSystem.lng.game[5] ;//"Скорость света";
            }
            else
            {
                infPref.SetActive(false);
            }

            if (g.isTimeBoosterOn)
            {
                if (((autoMiner.autoTime - autoMiner.timeBonus) / 500) > 0.01f)
                {
                    yield return new WaitForSecondsRealtime(((autoMiner.autoTime - autoMiner.timeBonus) / 500) - 0.01f);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(0);
                }
            }
            else
            {
                if (((autoMiner.autoTime - autoMiner.timeBonus) / 100) > 0.01f)
                {
                    yield return new WaitForSecondsRealtime(((autoMiner.autoTime - autoMiner.timeBonus) / 100) - 0.01f);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(0);
                }
            }
        }
    }
    public void BuyTimeUpgrade()
    {
        upgradePoints--;
        autoMiner.timeBonus = autoMiner.timeBonus + ((autoMiner.autoTime - autoMiner.timeBonus) / 100) * timeUpgrade;
        timeUpgrade = timeUpgrade - timeUpgradeD;
        checkUpgradeButtons();
        if (timeUpgrade > 0) g.improvementWin.transform.Find("Background/UpgradeGroup/TimeUpgrade/Text_count_tm").GetComponent<Text>().text = "-" + timeUpgrade.ToString() + "%";
        else g.improvementWin.transform.Find("Background/UpgradeGroup/TimeUpgrade/Text_count_tm").GetComponent<Text>().text = "0%";
        g.time.text = (autoMiner.autoTime - autoMiner.timeBonus).ToString("0.#0") + " " + LangSystem.lng.improvementWins[1];
        g.sr_pr.text = ((autoMiner.autoProfit) / (autoMiner.autoTime - autoMiner.timeBonus)).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
        updateUp();
    }
    public void BuyProfitUpgrade()
    {
        upgradePoints--;
        autoMiner.autoProfit = autoMiner.autoProfit + (autoMiner.autoProfit / 100) * profiteUpgrade;
        profiteUpgrade = profiteUpgrade - profiteUpgradeD;
        checkUpgradeButtons();
        g.improvementWin.transform.Find("Background/UpgradeGroup/ProfitUpgrade/Text_count_pr").GetComponent<Text>().text = "+" + profiteUpgrade.ToString() + "%";
        g.pribyl.text = cur.getName() + " " + (autoMiner.autoProfit).ToString("#0.###0");
        g.sr_pr.text = ((autoMiner.autoProfit) / (autoMiner.autoTime - autoMiner.timeBonus)).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
        updateUp();
    }

    public void BuyTimeOffUpgrade()
    {
        upgradePoints--;
        offProfit = offProfit + (offProfit / 100) * offProfitBonus;
        offProfitBonus = offProfitBonus - offProfitBonusD;
        checkUpgradeButtons();
        g.improvementWin.transform.Find("Background/UpgradeGroup/NightUpgrade/Text_count_n").GetComponent<Text>().text = "+" + offProfitBonus.ToString() + "%";
        updateUp();
    }

    public void checkUpgradeButtons()
    {
        g.upgradeProfitButton.interactable = ((upgradePoints > 0) && (profiteUpgrade > 0)) && autoMiner.isBoughtAuto;
        g.upgradeTimeButton.interactable = ((upgradePoints > 0) && (timeUpgrade > 0)) && autoMiner.isBoughtAuto;
        g.offMinerBonusButton.interactable = ((upgradePoints > 0) && (offProfitBonus > 0)) && isBoughtOff;
        g.upgradePointsText.text = LangSystem.lng.improvementWins[5] + ": " + upgradePoints.ToString();
    }

    public void checkEXP()
    {
        if (g.exp >= g.rewardCost)
        {
            g.money = g.money + g.reward;
            g.reward = (int)(g.reward * g.rewardD);
            g.rewardCost = (int)(g.rewardCost * g.rewardCostD);
        }
    }

    public void pushBroken()
    {
        g.push.transform.Find("Icon").GetComponent<Image>().sprite = transform.Find("MenuButton/Icon").GetComponent<Image>().sprite;
        g.push.transform.Find("Header").GetComponent<Text>().text = nameComp;
        g.push.transform.Find("Description").GetComponent<Text>().text = LangSystem.lng.push[4];// "Компьютер сломался! Необходимо заменить компоненты.";
        g.push.GetComponent<Animator>().SetTrigger("isShown");
    }

    public void pushIsNotReady()
    {
        g.push.transform.Find("Icon").GetComponent<Image>().sprite = transform.Find("MenuButton/Icon").GetComponent<Image>().sprite;
        g.push.transform.Find("Header").GetComponent<Text>().text = nameComp;
        g.push.transform.Find("Description").GetComponent<Text>().text = LangSystem.lng.push[5]; //"Компьютер не готов! Необходимо купить компоненты.";
        g.push.GetComponent<Animator>().SetTrigger("isShown");
    }

    public void checkIsBroken()
    {
        bool isBroken = false;
        if (!checkIsReady())
        {
            foreach (PartsOfComputer p in compParts)
            {
                isBroken = isBroken || p.isBroken;
            }
            if (isBroken)
            {
                pushBroken();
            }
            else
            {
                pushIsNotReady();
            }
        }
    }

    public void unblock()
    {
        GameObject prevComp = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 1).gameObject;
        GameObject prevComp1 = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 1).gameObject;
        bool wasReady = isReady;
        Computer pr1 = prevComp1.GetComponent<Computer>();
        if (checkIsReady() && isReady) isReady = true;
        else isReady = checkIsReady() && (pr1.isReady || (isFarm && g.farmCount != 0) || (isFarm && checkLast()));
        try
        {
            Computer next = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 1).gameObject.GetComponent<Computer>();
            if (isFarm || (next.isFarm && isReady))
            {
                if (next.isFarm && isReady && !isFarm)
                {
                    prevComp.transform.Find("BlockMining").gameObject.SetActive(true);
                    prevComp.GetComponent<Computer>().isReady = false;
                    prevComp.GetComponent<Computer>().infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 0;
                }
                next.unblock();
            }
            if (isReady)
            {
                if (next.checkIsReady() && (!isFarm)) next.unblock();
                else
                {
                    if (isFarm && !wasReady)
                    {
                        g.farmCount++;
                        blockLast();
                        if (g.farmCount == 1 && !g.gameObject.GetComponent<Achievment>().achievment[2].get)
                        {
                            g.gameObject.GetComponent<Achievment>().unlockAch(2);
                        }
                        if (g.farmCount == 3 && !g.gameObject.GetComponent<Achievment>().achievment[3].get)
                        {
                            g.gameObject.GetComponent<Achievment>().unlockAch(3);
                        }
                    }
                    block.SetActive(false);
                    efficiency = getEfficiency();
                    autoMiner.autoTime = autoMiner.maxTime * (200 - getEfficiency()) / 100;
                    if (!autoMiner.isBoughtAuto)
                    {
                        g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost);
                    }
                    else
                    {
                        StopCoroutine(BonusPerSec());
                        StartCoroutine(BonusPerSec());
                        g.time.text = (autoMiner.autoTime - autoMiner.timeBonus).ToString("0.#0") + " " + LangSystem.lng.improvementWins[1];
                        g.sr_pr.text = ((autoMiner.autoProfit) / (autoMiner.autoTime - autoMiner.timeBonus)).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
                        g.pribyl.text = cur.getName() + " " + (autoMiner.autoProfit).ToString("#0.###0");
                    }
                }
                if (!prevComp.GetComponent<Computer>().isFarm)
                {
                    prevComp.transform.Find("BlockMining").gameObject.SetActive(true);
                    prevComp.GetComponent<Computer>().isReady = false;
                    prevComp.GetComponent<Computer>().infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 0;
                }
            }
            else
            {
                block.SetActive(true);
                g.autoMinerButton.interactable = false;
            }
        }
        catch (System.Exception e)
        {
            if (isReady)
            {
                if (isFarm && !wasReady)
                {
                    g.farmCount++;
                    blockLast();
                    if (g.farmCount == 3 && !g.gameObject.GetComponent<Achievment>().achievment[3].get)
                    {
                        g.gameObject.GetComponent<Achievment>().unlockAch(3);
                    }
                }
                block.SetActive(false);
                efficiency = getEfficiency();
                autoMiner.autoTime = autoMiner.maxTime * (200 - getEfficiency()) / 100;
                if (!autoMiner.isBoughtAuto)
                {
                    g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost);
                }
                else
                {
                    StopCoroutine(BonusPerSec());
                    StartCoroutine(BonusPerSec());
                    g.time.text = (autoMiner.autoTime - autoMiner.timeBonus).ToString("0.#0") + " " + LangSystem.lng.improvementWins[1];
                    g.sr_pr.text = ((autoMiner.autoProfit) / (autoMiner.autoTime - autoMiner.timeBonus)).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
                    g.pribyl.text = cur.getName() + " " + (autoMiner.autoProfit).ToString("#0.###0");
                }
                if (!prevComp.GetComponent<Computer>().isFarm)
                {
                    prevComp.transform.Find("BlockMining").gameObject.SetActive(true);
                    prevComp.GetComponent<Computer>().isReady = false;
                    prevComp.GetComponent<Computer>().infPref.transform.Find("Stripes").GetComponent<Animator>().speed = 0;
                }
            }
            else
            {
                block.SetActive(true);
                g.autoMinerButton.interactable = false;
            }
        }
    }


    public void openCloseImprovementWin()
    {

        if (!autoMiner.isBoughtAuto)
        {
            g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost);
            g.time.text = "0" + " " + LangSystem.lng.improvementWins[1]; ;
            g.sr_pr.text = "0.0000 " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
            g.pribyl.text = cur.getName() + " " + bonus.ToString("#0.###0");
            g.upgradeTimeButton.interactable = false;
            g.upgradeProfitButton.interactable = false;
            g.improvementWin.transform.Find("Background/AutoMiner/GameObject/checkmark").GetComponent<check>().setOff();
            g.improvementWin.transform.Find("Background/AutoMiner/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = LangSystem.lng.improvementWins[12]; //"НАЖМИТЕ, ЧТОБЫ КУПИТЬ";
        }
        else
        {
            g.upgradeTimeButton.interactable = ((upgradePoints > 0) && (timeUpgrade > 0));
            g.upgradeProfitButton.interactable = ((upgradePoints > 0) && (profiteUpgrade > 0));
            float time = 0;
            float profit = 0;
            if (g.isTimeBoosterOn)
            {
                g.time.text = ((autoMiner.autoTime - autoMiner.timeBonus) / 5).ToString("0.#0") + " " + LangSystem.lng.improvementWins[1]; ;
                time = (autoMiner.autoTime - autoMiner.timeBonus) / 5;
            }
            else
            {
                g.time.text = (autoMiner.autoTime - autoMiner.timeBonus).ToString("0.#0") + " " + LangSystem.lng.improvementWins[1]; ;
                time = (autoMiner.autoTime - autoMiner.timeBonus);
            }

            if (g.isProfitBoosterOn)
            {
                g.pribyl.text = cur.getName() + " " + (autoMiner.autoProfit * 5).ToString("#0.###0");
                profit = autoMiner.autoProfit * 5;
            }
            else
            {
                g.pribyl.text = cur.getName() + " " + (autoMiner.autoProfit).ToString("#0.###0");
                profit = autoMiner.autoProfit;
            }
            g.sr_pr.text = (profit / time).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1]; ;
            g.improvementWin.transform.Find("Background/AutoMiner/GameObject/checkmark").GetComponent<check>().setOn();
            g.improvementWin.transform.Find("Background/AutoMiner/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = "";
            g.autoMinerButton.interactable = false;
        }

        if (!isBoughtOff)
        {
            g.offMinerButton.interactable = (g.money >= offMinerCost);
            g.offMinerBonusButton.interactable = false;
            g.improvementWin.transform.Find("Background/NightMiner/GameObject/checkmark").GetComponent<check>().setOff();
            g.improvementWin.transform.Find("Background/NightMiner/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = LangSystem.lng.improvementWins[12]; //"НАЖМИТЕ, ЧТОБЫ КУПИТЬ";
        }
        else
        {
            g.offMinerBonusButton.interactable = ((upgradePoints > 0) && (offProfitBonus > 0));
            g.improvementWin.transform.Find("Background/NightMiner/GameObject/checkmark").GetComponent<check>().setOn();
            g.improvementWin.transform.Find("Background/NightMiner/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = "";
            g.offMinerButton.interactable = false;
        }

        if (!isBoughtAutoRepair)
        {
            g.autoRepairButton.interactable = (g.money >= autoRepairCost);
            g.improvementWin.transform.Find("Background/AutoRepair/GameObject/checkmark").GetComponent<check>().setOff();
            g.improvementWin.transform.Find("Background/AutoRepair/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = LangSystem.lng.improvementWins[12]; //"НАЖМИТЕ, ЧТОБЫ КУПИТЬ";
        }
        else
        {
            g.improvementWin.transform.Find("Background/AutoRepair/GameObject/checkmark").GetComponent<check>().setOn();
            g.improvementWin.transform.Find("Background/AutoRepair/GameObject/BuyAuto/Text_buy").GetComponent<Text>().text = "";
            g.autoRepairButton.interactable = false;
        }

        if (!isReady)
        {
            g.autoMinerButton.interactable = false;
            g.offMinerButton.interactable = false;
            g.autoRepairButton.interactable = false;
        }
        g.levelText.text = LangSystem.lng.improvementWins[0] + ": " + level.ToString();
        g.nameText.text = nameComp;
        g.compId = gameObject.transform.GetSiblingIndex();

        g.fill.fillAmount = (float)progressCounter2 / (float)upgradeCost;
        g.prBarText.text = "XP " + progressCounter2.ToString() + " / " + upgradeCost.ToString();
        g.improvementWin.transform.Find("Background/UpgradeGroup/ProfitUpgrade/Text_count_pr").GetComponent<Text>().text = "+" + profiteUpgrade.ToString() + "%";
        if (timeUpgrade > 0) g.improvementWin.transform.Find("Background/UpgradeGroup/TimeUpgrade/Text_count_tm").GetComponent<Text>().text = "-" + timeUpgrade.ToString() + "%";
        else g.improvementWin.transform.Find("Background/UpgradeGroup/TimeUpgrade/Text_count_tm").GetComponent<Text>().text = "0%";
        if (offProfitBonus > 0) g.improvementWin.transform.Find("Background/UpgradeGroup/NightUpgrade/Text_count_n").GetComponent<Text>().text = "+" + offProfitBonus.ToString() + "%";
        else g.improvementWin.transform.Find("Background/UpgradeGroup/NightUpgrade/Text_count_n").GetComponent<Text>().text = "0%";

        g.upgradePointsText.text = LangSystem.lng.improvementWins[5] + ": " + upgradePoints.ToString();
        g.autoMinerButton.onClick.RemoveAllListeners();
        g.autoMinerButton.onClick.AddListener(BuyAutoMiner);
        g.upgradeTimeButton.onClick.RemoveAllListeners();
        g.upgradeProfitButton.onClick.RemoveAllListeners();
        g.upgradeTimeButton.onClick.AddListener(BuyTimeUpgrade);
        g.offMinerBonusButton.onClick.RemoveAllListeners();
        g.offMinerBonusButton.onClick.AddListener(BuyTimeOffUpgrade);
        g.upgradeProfitButton.onClick.AddListener(BuyProfitUpgrade);
        g.offMinerButton.onClick.RemoveAllListeners();
        g.offMinerButton.onClick.AddListener(BuyOffMiner);
        g.autoRepairButton.onClick.RemoveAllListeners();
        g.autoRepairButton.onClick.AddListener(BuyAutoRepair);


        g.improvementWin.transform.Find("Background/AutoMiner/GameObject/Icon_Autominer/Text").GetComponent<Text>().text = "$" + autoMiner.autoCost.ToString();
        g.improvementWin.transform.Find("Background/NightMiner/GameObject/Icon_Autominer/Text").GetComponent<Text>().text = "$" + offMinerCost.ToString();
        g.improvementWin.transform.Find("Background/AutoRepair/GameObject/Icon_Autominer/Text").GetComponent<Text>().text = "$" + autoRepairCost.ToString();

        g.improvementWin.transform.Find("Background/Header/icon").GetComponent<Image>().sprite = backgr;
        // g.improvementWin.transform.Find("Background/Header/icon").GetComponent<Image>().sprite = transform.Find("MenuButton").GetComponent<Image>().sprite;
        g.improvementWin.transform.Find("Background/Header/icon/Image").GetComponent<Image>().sprite = transform.Find("MenuButton/Icon").GetComponent<Image>().sprite;

        if (compParts.Length == 0)
        {
            g.improvementWin.transform.Find("Background/Description_parts/Text").GetComponent<Text>().text = "";
            g.autoRepairButton.gameObject.transform.parent.parent.gameObject.SetActive(false);
            g.improvementWin.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
        }
        else
        {
            g.autoRepairButton.gameObject.transform.parent.parent.gameObject.SetActive(true);
            g.improvementWin.transform.GetChild(1).GetChild(7).gameObject.SetActive(true);
            g.improvementWin.transform.Find("Background/Description_parts/Text").GetComponent<Text>().text = LangSystem.lng.improvementWins[18]; //"Компоненты компьютера";
        }
        for (int i = 0; i < compParts.Length; i++)
        {
            int temp = i;
            GameObject A = Instantiate(partPrefab, partPrefab.transform.position = new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            A.transform.SetParent(partsContainer.transform, false);
            A.transform.Find("nameText").GetComponent<Text>().text = g.parts[compParts[i].id].partName;
            A.transform.Find("Image").GetComponent<Image>().sprite = g.parts[compParts[i].id].type.image;
            A.transform.Find("BuyNew/Text").GetComponent<Text>().text = "$" + g.parts[compParts[i].id].costNew + " | " + g.parts[compParts[i].id].reliabilityNew + "%";
            A.transform.Find("BuyUsed/Text").GetComponent<Text>().text = "$" + g.parts[compParts[i].id].costUsed + " | " + g.parts[compParts[i].id].reliabilityUsed + "%";
            A.transform.Find("BuyNew").GetComponent<Button>().onClick.AddListener(delegate { BuyPart(temp, true, A); });
            A.transform.Find("BuyUsed").GetComponent<Button>().onClick.AddListener(delegate { BuyPart(temp, false, A); });
            A.transform.Find("TextUsed").GetComponent<Text>().text = LangSystem.lng.improvementWins[19];
            A.transform.Find("TextNew").GetComponent<Text>().text = LangSystem.lng.improvementWins[20];
        }
        //updateUp();//обновление очков улучшений
        changeBuyButtons();
        //ПЕРЕВОДЫ
        impr.GetChild(1).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[2]; // Прибыль
        impr.GetChild(1).GetChild(2).GetComponent<Text>().text = LangSystem.lng.improvementWins[3]; // Время
        impr.GetChild(1).GetChild(4).GetComponent<Text>().text = LangSystem.lng.improvementWins[4]; // Ср. прибыль
        //-------------------------------------------//
        g.upgradePointsText.text = LangSystem.lng.improvementWins[5] + ": " + upgradePoints.ToString();
        //-------------------------------------------//
        impr.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[6]; // Прибыль
        impr.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[7]; // Время
        impr.GetChild(3).GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[8]; // Время
        //-------------------------------------------//
        impr.GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[9]; //Автомайнер
        impr.GetChild(4).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[10]; // Программа
        impr.GetChild(4).GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text = LangSystem.lng.improvementWins[11]; //Описание
        //-------------------------------------------//
        impr.GetChild(5).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[13]; //Ночной майнер
        impr.GetChild(5).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[14]; // Автономная добыча
        impr.GetChild(5).GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>().text = LangSystem.lng.improvementWins[15]; //Описание
        //-------------------------------------------//
        impr.GetChild(6).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.improvementWins[16]; //авторемонт
        impr.GetChild(6).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>().text = LangSystem.lng.improvementWins[17];
        impr.GetChild(6).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = LangSystem.lng.improvementWins[12]; //Описание
        //-------------------------------------------//
        g.improvementWin.SetActive(!g.improvementWin.activeSelf);
    }

    //мощность компьютера в процентах
    public int getEfficiency()
    {
        int eff = 100;
        int[] masIsBought;
        int[] masAmount;
        masIsBought = new int[g.typesOfParts.Length];
        masAmount = new int[g.typesOfParts.Length];
        for (int i = 0; i < masIsBought.Length; i++)
        {
            masIsBought[i] = 0;
            masAmount[i] = 0;
        }
        if (compParts.Length != 0)
        {
            foreach (PartsOfComputer p in compParts)
            {
                int tId = g.parts[p.id].type.id;
                masIsBought[tId] = p.isBought ? (masIsBought[tId] + 1) : masIsBought[tId];
                masAmount[tId]++;
            }
            for (int i = 0; i < masIsBought.Length; i++)
            {
                if (i == 1)
                {
                    int delta = (int)((((float)masIsBought[1] / (float)masAmount[1]) * 100) * fullGPU / 100);
                    eff = eff - (fullGPU - delta);
                }
                else
                {
                    eff -= (5 - (int)(((masIsBought[i] / masAmount[i]) * 100) * 0.05f));
                }
            }
        }
        return eff;
    }


    public void ResearchComp()
    {
        if (gameObject.transform.GetSiblingIndex() != 0)
        {
            GameObject prevComp = gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() - 1).gameObject;
            Computer pr = prevComp.GetComponent<Computer>();
            if (pr.isReady)
            {
                g.money -= cost;
                buyComp.gameObject.SetActive(false);
                g.moneyText.text = "$" + g.money.ToString("0.#0");
                isResearched = true;
            }
            else
            {
                g.push.transform.Find("Icon").GetComponent<Image>().sprite = transform.Find("MenuButton/Icon").GetComponent<Image>().sprite;
                g.push.transform.Find("Header").GetComponent<Text>().text = nameComp;
                g.push.transform.Find("Description").GetComponent<Text>().text = LangSystem.lng.push[6] + " " + pr.nameComp + " " + LangSystem.lng.push[7];
                g.push.GetComponent<Animator>().SetTrigger("isShown");
            }
        }
        else
        {
            g.money -= cost;
            buyComp.gameObject.SetActive(false);
            g.moneyText.text = "$" + g.money.ToString("0.#0");
            isResearched = true;
            //if (isFarm) g.farmCount++;
        }
    }

    //id - индекс в массиве компонентов данного компьютера, а не id компонента
    public void BuyPart(int id, bool isNew, GameObject part)
    {
        if (isNew)
        {
            g.money -= g.parts[compParts[id].id].costNew;
            compParts[id].curReliability = g.parts[compParts[id].id].reliabilityNew;
        }
        else
        {
            g.money -= g.parts[compParts[id].id].costUsed;
            compParts[id].curReliability = g.parts[compParts[id].id].reliabilityUsed;
        }
        compParts[id].isBought = true;
        compParts[id].isBroken = false;
        part.transform.Find("BuyNew").gameObject.GetComponent<Button>().interactable = false;
        part.transform.Find("BuyUsed").gameObject.GetComponent<Button>().interactable = false;
        g.moneyText.text = "$" + g.money.ToString("0.#0");
        changeBuyButtons();

        if (autoMiner.isBoughtAuto)
        {
            efficiency = getEfficiency();
            autoMiner.autoTime = autoMiner.maxTime * (200 - getEfficiency()) / 100;
            g.time.text = (autoMiner.autoTime - autoMiner.timeBonus).ToString("0.#0") + " " + LangSystem.lng.improvementWins[1];
            g.sr_pr.text = ((autoMiner.autoProfit) / (autoMiner.autoTime - autoMiner.timeBonus)).ToString("#0.###0") + " " + cur.getName() + " / " + LangSystem.lng.improvementWins[1];
        }

        g.partCount++;
        if (g.partCount == 100) g.gameObject.GetComponent<Achievment>().unlockAch(4);
        if (!isReady) unblock();
        checkInteractable();
    }

    //Проверка, куплены ли все необходимые части
    public bool checkIsReady()
    {
        bool res = true;
        bool[] masIsBought;
        masIsBought = new bool[g.typesOfParts.Length];
        if (compParts.Length != 0)
        {
            for (int i = 0; i < masIsBought.Length; i++)
            {
                masIsBought[i] = false;
            }
            foreach (PartsOfComputer p in compParts)
            {
                int tId = g.parts[p.id].type.id;
                masIsBought[tId] = masIsBought[tId] || (p.isBought && !p.isBroken);
            }
            for (int i = 0; i < masIsBought.Length; i++)
            {
                res = res && masIsBought[i];
            }
        }
        return res;
    }

    public void changeBuyButtons()
    {
        int childs = compParts.Length;
        for (int i = 0; i < childs; i++)
        {
            if (!compParts[i].isBought)
            {
                partsContainer.transform.GetChild(i).Find("BuyNew").GetComponent<Button>().interactable = (g.parts[compParts[i].id].costNew <= g.money);
                partsContainer.transform.GetChild(i).Find("BuyUsed").GetComponent<Button>().interactable = (g.parts[compParts[i].id].costUsed <= g.money);
                partsContainer.transform.GetChild(i).Find("Ckecmark").GetComponent<check>().setOff();
                if (compParts[i].isBroken)
                {
                    partsContainer.transform.GetChild(i).Find("Burned").GetComponent<check>().setOn();
                }
            }
            else
            {
                partsContainer.transform.GetChild(i).Find("BuyNew").GetComponent<Button>().interactable = false;
                partsContainer.transform.GetChild(i).Find("BuyUsed").GetComponent<Button>().interactable = false;
                partsContainer.transform.GetChild(i).Find("Ckecmark").GetComponent<check>().setOn();
                //partsContainer.transform.GetChild(i).Find("Burned").GetComponent<check>().setOff();
                partsContainer.transform.GetChild(i).Find("Burned").GetComponent<check>().Disable();
            }
        }
    }
}

[System.Serializable]
class AutoMiner
{
    public int autoCost; // Цена покупки автомайнера
    public string autoName; // Имя автомайнера
    public bool isBoughtAuto; // Куплен ли автомайнер?
    public float autoTime; // Время заполенения прогресс-бара. Уменьшается за счет timeUpgrade
    public float maxTime; // Время заполнения прогресс бара при полном комполекте зачастей
    public float autoProfit; // Доход автомайнера. Изначально = bonus. В дальнейшем autoProfit = bonus + profitUpgrade
    public float timeBonus;

    public AutoMiner(int cost, string name, float time, float profit)
    {
        this.autoCost = cost;
        this.autoName = name;
        this.maxTime = time;
        this.autoProfit = profit;
        this.isBoughtAuto = false;
        timeBonus = 0;
    }
}




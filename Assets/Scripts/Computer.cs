using UnityEngine;
using UnityEngine.UI;
using System.Collections;


class Computer : MonoBehaviour
{
    public Text currencyText; //Деньги в валюте
    public Text progressText; //Текст процентного заполнения кликов
    public Text bonusText; //Текст стоимость улучшения
    public Text expText; //Текст количества опыта
    public GameObject partsContainer; //Контейнер, содержащий части компьютера
    public GameObject partPrefab;
    public GameObject block; //болкировка майнинга, если не куплены все необходимые части

    public Image p1; //полоса прогресса заполнения кликов
    public Image p2; //полоса прогресса заполнения опыта
    public GameObject progressPanel; //панель для кликов
    public Button bonusButton; //кнопка покупки улучшения
    public Button buyComp; //кнопка покупки компьютера

    public Game g;
    
    int progressCounter1 = 0;
    int progressCounter2 = 0;

    Currency cur;
    float currency = 0;

    //процент от времени заполнения автомайнера при полном наборе видеокарт
    public int fullGPU=40;

    public int cost = 1; // цена компьютера
    public string nameComp = "Ноутбук"; // Имя компьютера
    bool isReady; // готов ли компьютер к работе (все ли компоненты куплены)
    bool isFarm; // ферма?
    public bool isResearched = true; // открыт ли компьютер
    float bonus = 0.001f; // доход за заполнение прогресс-бара
    float bonusCost = 2; // цена улучшения (на которую поднимаем доход)
    float bonusD = 1.32f;
    float bonusCostD = 1.44f;
    int maxClick=10; // кол-во дробей прогресс-бара
    int exp = 0;
    int expD = 2; // кол-во опыта за клик по кнопке
    int expU = 15; // кол-во опыта за улучшение доходности
    int upgradeCost=200; //максимальное число EXP. Начало с 200
    float upgradeCoef = 1.16f; // коеф. сложности upgradeCost, для ноута - 1,12. 
    int upgradePoints=0; // кол-во очков улучшений (ОУ)
    //удалить 
    public int efficiency = 100;

    /*-- появляется после покупки автомайна --*/
    float timeUpgrade = 30; // Ускорение майна. Начинается с 30% и падает на 2,5% каждое улучшение
    float timeUpgradeD = 2.5f; 
    float profiteUpgrade = 35; // Увеличение доходности майнинга. Начинается с 35% и падает на 2.5% каждое улучшение
    float profiteUpgradeD = 2.5f;
    /*!-- появляется после покупки автомайна --*/

    int clickCounter = 0; // Счетчик кликов по кнопке
    int level = 0; // Уровень развития компьютера. Повышается на +1 каждые 50 кликов. Не зависит от EXP 

    public AutoMiner autoMiner;
    string jsonParts;
    public PartsOfComputer[] compParts;

    /*public Computer(string name, int cost, float startBonus, float bonusCost, int maxClick, float upgradeCoef, Currency cur)
    {
        nameComp = name;
        this.cost = cost;
        bonus = startBonus;
        this.bonusCost = bonusCost;
        this.maxClick = maxClick;
        this.upgradeCoef = upgradeCoef;
        this.cur = cur;
    }*/

    void Start()
    {
        Currency c = new Currency("ETH", 30000f);
        this.cur = c;

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
        buyComp.transform.Find("Text").gameObject.GetComponent<Text>().text = "$"+cost;
        bonusButton.onClick.AddListener(GetBonus);
        buyComp.gameObject.SetActive(!isResearched);
        buyComp.onClick.AddListener(ResearchComp);
        transform.Find("ProgressPanel/nameText").gameObject.GetComponent<Text>().text = nameComp;
        autoMiner = new AutoMiner(1, "AutoMiner", 5, bonus);
        autoMiner.autoTime = ((200-getEfficiency())/100)* autoMiner.maxTime;
    }

    public void Update()
    {
        currencyText.text = currency.ToString("#0.###0");
        bonusButton.interactable = (g.money >= bonusCost);
        buyComp.interactable = (g.money >= cost);
        p1.fillAmount = (float)progressCounter1 / (float)maxClick;
        progressText.text = ((int)(((float)progressCounter1 / (float)maxClick) * 100)).ToString() + "%";
        
    }

    public void OnClick()
    {
        clickCounter++;
        exp += expD;
        progressCounter1 ++;
        progressCounter2+= expD;
        if (progressCounter1 > maxClick)
        {
            progressCounter1 = 0;
            currency += bonus;
        }
        if (progressCounter2 > upgradeCost)
        {
            level++;
            
            if (maxClick > 1) maxClick--;
            progressCounter2 = progressCounter2 % upgradeCost;
            upgradePoints++;
            upgradeCost = (int)(upgradeCost * upgradeCoef);
        }
        expText.text = progressCounter2.ToString() + "/" + upgradeCost.ToString()+"xp";
        p2.fillAmount = (float)progressCounter2 / (float)upgradeCost;
    }
    public void GetBonus()
    {
        exp += expU;
        expText.text = progressCounter2.ToString() + "/" + upgradeCost.ToString();
        g.money = g.money - bonusCost;
        g.moneyText.text = "$"+g.money.ToString();
        bonusCost = bonusCost * bonusCostD;
        bonus = bonus * bonusD;
        bonusText.text = "$"+(bonusCost).ToString("0.#0");
    }
    public void Exchange()
    {
        g.money = (g.money + currency * cur.getExchRate());
        currency = 0;
        g.moneyText.text = "$"+ g.money.ToString();
    }
    public void BuyAutoMiner()
    {
        g.money = g.money - autoMiner.autoCost;
        autoMiner.isBoughtAuto = true;
        g.autoMinerButton.interactable = false;
        StartCoroutine(BonusPerSec());
        g.upgradeTimeButton.interactable = (upgradePoints > 0);
        g.upgradeProfitButton.interactable = (upgradePoints > 0);
        g.moneyText.text = "$" + g.money.ToString();
    }
    IEnumerator BonusPerSec()
    {
        while (true)
        {
            clickCounter++;
            progressCounter1++;
            if (progressCounter1 > maxClick)
            {
                progressCounter1 = 0;
                currency = currency + bonus + autoMiner.autoProfit;
                p1.fillAmount = (float)progressCounter1 / (float)maxClick;
                progressText.text = ((int)(((float)progressCounter1 / (float)maxClick) * 100)).ToString() + "%";
            }
            yield return new WaitForSeconds((autoMiner.autoTime - autoMiner.timeBonus) / (float)maxClick);
        }
    }
    public void BuyTimeUpgrade()
    {
        upgradePoints--;
        autoMiner.timeBonus = autoMiner.timeBonus + ((autoMiner.autoTime - autoMiner.timeBonus) / 100) * timeUpgrade;
        timeUpgrade = timeUpgrade - timeUpgradeD;
        g.upgradeTimeButton.interactable = (upgradePoints > 0);
        g.upgradeProfitButton.interactable = (upgradePoints > 0);
        g.upgradePointsText.text = "Очки улучшений: " + upgradePoints.ToString();
    }
    public void BuyProfitUpgrade()
    {
        upgradePoints--;
        autoMiner.autoProfit = autoMiner.autoProfit + (autoMiner.autoProfit / 100) * profiteUpgrade;
        profiteUpgrade = profiteUpgrade - profiteUpgradeD;
        g.upgradeProfitButton.interactable = (upgradePoints > 0);
        g.upgradeTimeButton.interactable = (upgradePoints > 0);
        g.upgradePointsText.text = "Очки улучшений: " + upgradePoints.ToString();
    }
    public void openCloseImprovementWin()
    {
        if (!autoMiner.isBoughtAuto)
        {
            g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost);
            g.time.text = "0" + " СЕК";
            g.sr_pr.text = "0 " + cur.getName() + " / СЕК";
        }
        else
        {
            g.upgradeTimeButton.interactable = (upgradePoints > 0);
            g.upgradeProfitButton.interactable = (upgradePoints > 0);
            g.time.text = autoMiner.autoTime.ToString() + " СЕК";
            g.sr_pr.text = (bonus / autoMiner.autoTime).ToString("#0.###0") + " " + cur.getName() + " / СЕК";
        }
        if (!isReady)
        {
            g.autoMinerButton.interactable = false;
        }
        g.levelText.text = "Уровень: " + level.ToString();
        g.nameText.text = nameComp;
        g.pribyl.text = cur.getName() + " " + bonus.ToString("#0.###0");
        g.fill.fillAmount = (float)progressCounter2 / (float)upgradeCost;
        g.prBarText.text = "XP " + progressCounter2.ToString() + " / " + upgradeCost.ToString();

        g.upgradePointsText.text = "Очки улучшений: " + upgradePoints.ToString();
        g.autoMinerButton.onClick.RemoveAllListeners();
        g.autoMinerButton.onClick.AddListener(BuyAutoMiner);
        g.upgradeTimeButton.onClick.RemoveAllListeners();
        g.upgradeProfitButton.onClick.RemoveAllListeners();
        g.upgradeTimeButton.onClick.AddListener(BuyTimeUpgrade);
        g.upgradeProfitButton.onClick.AddListener(BuyProfitUpgrade);

        for (int i = 0; i < compParts.Length; i++)
        {
            int temp = i;
            GameObject A = Instantiate(partPrefab, partPrefab.transform.position = new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            A.transform.SetParent(partsContainer.transform, false);
            A.transform.Find("nameText").GetComponent<Text>().text = g.parts[compParts[i].id].partName;
            A.transform.Find("BuyNew/Text").GetComponent<Text>().text = "$" + g.parts[compParts[i].id].costNew + " | " + g.parts[compParts[i].id].reliabilityNew + "%";
            A.transform.Find("BuyUsed/Text").GetComponent<Text>().text = "$" + g.parts[compParts[i].id].costUsed + " | " + g.parts[compParts[i].id].reliabilityUsed + "%";
            A.transform.Find("BuyNew").GetComponent<Button>().onClick.AddListener(delegate { BuyPart(temp, true, A); });
            A.transform.Find("BuyUsed").GetComponent<Button>().onClick.AddListener(delegate { BuyPart(temp, false, A); });
        }
        changeBuyButtons();
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
        return eff;
    }


    public void ResearchComp()
    {
        g.money -= cost; 
        buyComp.gameObject.SetActive(false);
        g.moneyText.text = "$" + g.money.ToString();
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
        g.moneyText.text = "$" + g.money.ToString();
        changeBuyButtons();
        isReady = checkIsReady();
        block.SetActive(!isReady);
        efficiency = getEfficiency();
        autoMiner.autoTime = autoMiner.maxTime * (200 - getEfficiency()) / 100;
        if (!isReady)
        {
            g.autoMinerButton.interactable = false;
            StopCoroutine(BonusPerSec());
        }
        else
        {
            if (!autoMiner.isBoughtAuto) g.autoMinerButton.interactable = (g.money >= autoMiner.autoCost);
        }
    }

    //Проверка, куплены ли все необходимые части
    public bool checkIsReady()
    {
        bool res = true;
        bool[] masIsBought;
        masIsBought = new bool[g.typesOfParts.Length];
        for (int i=0; i < masIsBought.Length; i++)
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
            }
            else
            {
                partsContainer.transform.GetChild(i).Find("BuyNew").GetComponent<Button>().interactable = false;
                partsContainer.transform.GetChild(i).Find("BuyUsed").GetComponent<Button>().interactable = false;
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

[System.Serializable]
class PartsOfComputer
{
    /*-- JSON-сериализация --*/
    public int id; // Номер компонента в списке улучшений
    public bool isBought; // Куплен ли компонент?
    public bool isBroken; // Сломан ли компонент?
    public float curReliability; // Вероятность поломки компонента в зависимости от новизны
    /*!-- JSON-сериализация --*/
}

//Класс Криптовалюты
class Currency
{
    private string name;
    private float exchRate;
    public Currency(string name, float exchRate)
    {
        this.name = name;
        this.exchRate = exchRate;
    }
    public string getName()
    {
        return this.name;
    }
    public float getExchRate()
    {
        return this.exchRate;
    }
}
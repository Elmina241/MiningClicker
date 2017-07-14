using UnityEngine;
using UnityEngine.UI;
using System.Collections;


class Computer : MonoBehaviour
{

    public Text moneyText; //Деньги в $
    public Text currencyText; //Деньги в валюте
    public Text progressText; //Текст процентного заполнения кликов
    public Text bonusText; //Текст стоимость улучшения
    public Text expText; //Текст количества опыта
    public Text upgradePointsText; //Текст очки улучшения
    public Text levelText; //Текст номер уровня
    public Image p1; //полоса прогресса заполнения кликов
    public Image p2; //полоса прогресса заполнения опыта
    public GameObject improvementWin; //окно улучшений и запчастей
    public Button bonusButton; //кнопка покупки улучшения
    public Button autoMinerButton; //кнопка покупки автомайнера
    public Button upgradeTimeButton; //кнопка покупки улучшения времени
    public Button upgradeProfitButton; //кнопка покупки улучшения дохода
    private float money = 0;
    int progressCounter1 = 0;
    int progressCounter2 = 0;

    float currency = 0;
    float exchRate = 200;
    string currencyName = "ETH";


    int cost; // цена компьютера
    string nameComp; // Имя компьютера
    bool isReady; // готов ли компьютер к работе (все ли компоненты куплены)
    bool isResearched; // открыт ли компьютер
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

    /*-- появляется после покупки автомайна --*/
    float timeUpgrade = 30; // Ускорение майна. Начинается с 30% и падает на 2,5% каждое улучшение
    float timeUpgradeD = 2.5f; 
    float profiteUpgrade = 35; // Увеличение доходности майнинга. Начинается с 35% и падает на 2.5% каждое улучшение
    float profiteUpgradeD = 2.5f;
    /*!-- появляется после покупки автомайна --*/

    int clickCounter = 0; // Счетчик кликов по кнопке
    int level = 0; // Уровень развития компьютера. Повышается на +1 каждые 50 кликов. Не зависит от EXP 
    int levelCost; // Цена уровня (кол-во кликов) УДАЛИТЬ

    public AutoMiner autoMiner;
    string jsonParts;

    public Computer()
    {
        
    }

    void Start()
    {
        Text t = transform.Find("ProgressPanel/currencyName").gameObject.GetComponent<Text>();
        t.text = currencyName;
        this.currencyText = transform.Find("ProgressPanel/currencyText").gameObject.GetComponent<Text>();
        this.progressText = transform.Find("ProgressPanel/clickProgressbar/clickText").gameObject.GetComponent<Text>();
        this.bonusText = transform.Find("BonusPanel/BonusCost").gameObject.GetComponent<Text>();
        this.expText = transform.Find("ProgressPanel/xpProgressbar/xpText").gameObject.GetComponent<Text>();
        this.improvementWin = transform.Find("ImprovementWin").gameObject;
        this.upgradePointsText = improvementWin.transform.Find("UpgradePoints").gameObject.GetComponent<Text>();
        this.levelText = improvementWin.transform.Find("LevelText").gameObject.GetComponent<Text>();
        this.p1 = transform.Find("ProgressPanel/clickProgressbar/Foreground").gameObject.GetComponent<Image>();
        this.p2 = transform.Find("ProgressPanel/xpProgressbar/Foreground").gameObject.GetComponent<Image>();
        this.bonusButton = transform.Find("BonusPanel/BonusButton").gameObject.GetComponent<Button>();
        this.autoMinerButton = improvementWin.transform.Find("AutoMiner/BuyAuto").gameObject.GetComponent<Button>();
        this.upgradeTimeButton = improvementWin.transform.Find("TimeUpgrade").gameObject.GetComponent<Button>();
        this.upgradeProfitButton = improvementWin.transform.Find("ProfitUpgrade").gameObject.GetComponent<Button>();
        bonusButton.onClick.AddListener(GetBonus);
        autoMinerButton.onClick.AddListener(BuyAutoMiner);
        upgradeTimeButton.onClick.AddListener(BuyTimeUpgrade);
        upgradeProfitButton.onClick.AddListener(BuyProfitUpgrade);
        autoMiner = new AutoMiner(1, "AutoMiner", 5, bonus);
    }

    public void Update()
    {
        currencyText.text = currency.ToString("#0.###0");
        bonusButton.interactable = (money >= bonusCost);
        if (!autoMiner.isBoughtAuto)
        {
            autoMinerButton.interactable = (money >= autoMiner.autoCost);
        }
        else
        {
            upgradeTimeButton.interactable = (upgradePoints > 0); ;
            upgradeProfitButton.interactable = (upgradePoints > 0); ;
        }
        p1.fillAmount = (float)progressCounter1 / (float)maxClick;
        progressText.text = ((int)(((float)progressCounter1 / (float)maxClick) * 100)).ToString() + "%";
        upgradePointsText.text = "Очки улучшений: " + upgradePoints.ToString();
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
            levelText.text = "Уровень: " + level.ToString();
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
        money = money - bonusCost;
        moneyText.text = money.ToString() + "$";
        bonusCost = bonusCost * bonusCostD;
        bonus = bonus * bonusD;
        bonusText.text = (bonusCost).ToString("0.#0") + "$";
    }
    public void openCloseImprovementWin()
    {
        improvementWin.SetActive(!improvementWin.activeSelf);
    }
    public void Exchange()
    {
        money = (money + currency * exchRate);
        currency = 0;
        moneyText.text = money.ToString() + "$";
    }
    public void BuyAutoMiner()
    {
        money = money - autoMiner.autoCost;
        autoMiner.isBoughtAuto = true;
        autoMinerButton.interactable = false;
        StartCoroutine(BonusPerSec());
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
            yield return new WaitForSeconds(autoMiner.autoTime / (float)maxClick);
        }
    }
    public void BuyTimeUpgrade()
    {
        upgradePoints--;
        autoMiner.autoTime = autoMiner.autoTime + (autoMiner.autoTime / 100) * timeUpgrade;
        timeUpgrade = timeUpgrade - timeUpgradeD;
    }
    public void BuyProfitUpgrade()
    {
        upgradePoints--;
        autoMiner.autoProfit = autoMiner.autoProfit + (autoMiner.autoProfit / 100) * profiteUpgrade;
        profiteUpgrade = profiteUpgrade - profiteUpgradeD;
    }
}

class AutoMiner
{
    public int autoCost; // Цена покупки автомайнера
    public string autoName; // Имя автомайнера
    public bool isBoughtAuto; // Куплен ли автомайнер?
    public float autoTime; // Время заполенения прогресс-бара. Уменьшается за счет timeUpgrade
    public float autoProfit; // Доход автомайнера. Изначально = bonus. В дальнейшем autoProfit = bonus + profitUpgrade

    public AutoMiner(int cost, string name, float time, float profit)
    {
        this.autoCost = cost;
        this.autoName = name;
        this.autoTime = time;
        this.autoProfit = profit;
        this.isBoughtAuto = false;
    }
}

class PartsOfComputer
{
    /*-- JSON-сериализация --*/
    int id; // Номер компонента в списке улучшений
    string partsName; // Имя компонента (AMD RADEON RX480 4GB)
    bool isBought; // Куплен ли компонент?
    bool isBroken; // Сломан ли компонент?
    float reliability; // Вероятность поломки каждые 100 заполнений прогресс-бара. 0-10% для Новых, 0-25% для Б/У
    int costNew; // Цена нового компонента
    int costUsed; // Цена Б/У компонента

    /*!-- JSON-сериализация --*/
}

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
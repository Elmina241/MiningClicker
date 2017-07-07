using UnityEngine;
using UnityEngine.UI;
using System.Collections;


class Computer : MonoBehaviour
{

    public Text moneyText;
    public Text progressText;
    public Text bonusText;
    public Text expText;
    public Image p1;
    public Image p2;
    public GameObject improvementWin;
    private int money = 0;
    int progressCounter1 = 0;
    int progressCounter2 = 0;

    int cost; // цена компьютера
    string nameComp; // Имя компьютера
    bool isReady; // готов ли компьютер к работе (все ли компоненты куплены)
    bool isResearched; // открыт ли компьютер
    int bonus = 1; // доход за заполнение прогресс-бара
    int bonusCost = 2; // цена улучшения (на которую поднимаем доход)
    int maxClick=10; // кол-во дробей прогресс-бара
    int exp = 0;
    int expD = 2; // кол-во опыта за клик по кнопке
    int expU = 15; // кол-во опыта за улучшение доходности
    int upgradeCost=200; //максимальное число EXP. Начало с 200
    float upgradeCoef = 1.12f; // коеф. сложности upgradeCost, для ноута - 1,12. 
    int upgradePoints=0; // кол-во очков улучшений (ОУ)

    /*-- появляется после покупки автомайна --*/
    float timeUpgrade; // Ускорение майна. Начинается с 30% и падает на 2,5% каждое улучшение
    float profiteUpgrade; // Увеличение доходности майнинга. Начинается с 35% и падает на 2.5% каждое улучшение
                          /*!-- появляется после покупки автомайна --*/

    int clickCounter = 0; // Счетчик кликов по кнопке
    int level; // Уровень развития компьютера. Повышается на +1 каждые 50 кликов. Не зависит от EXP 
    int levelCost; // Цена уровня (кол-во кликов)

    AutoMiner autoMiner;
    string jsonParts;

    public void OnClick()
    {
        clickCounter++;
        exp += expD;
        progressCounter1 ++;
        progressCounter2+= expD;
        if (progressCounter1 > maxClick)
        {
            progressCounter1 = 0;
            money += bonus;
            moneyText.text = money.ToString();
        }
        if (progressCounter2 > upgradeCost)
        {
            progressCounter2 = progressCounter2 % upgradeCost;
            upgradePoints++;
        }
        expText.text = exp.ToString() + "/" + upgradeCost.ToString()+"xp";
        p1.fillAmount = (float)progressCounter1 / (float)maxClick;
        p2.fillAmount = (float)progressCounter2 / (float)upgradeCost;
        progressText.text = ((int)(((float)progressCounter1 / (float)maxClick) * 100)).ToString() + "%";
    }
    public void GetBonus()
    {
        exp += expU;
        expText.text = exp.ToString() + "/" + upgradeCost.ToString();
        money = money - bonusCost;
        bonus = bonus + bonusCost;
        moneyText.text = money.ToString();
        bonusText.text = bonus.ToString() + "$";
    }
    public void openCloseImprovementWin()
    {
        improvementWin.SetActive(!improvementWin.activeSelf);
    }
}

class AutoMiner
{
    int autoCost; // Цена покупки автомайнера
    string autoName; // Имя автомайнера
    bool isBoughtAuto; // Куплен ли автомайнер?
    float autoTime; // Время заполенения прогресс-бара. Уменьшается за счет timeUpgrade
    float autoProfit; // Доход автомайнера. Изначально = bonus. В дальнейшем autoProfit = bonus + profitUpgrade
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

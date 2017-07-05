using UnityEngine;


class Computer : MonoBehavior
{

    int cost; // цена компьютера
    string nameComp; // Имя компьютера
    bool isReady; // готов ли компьютер к работе (все ли компоненты куплены)
    bool isResearched; // исследован ли компьютер
    int bonus; // доход за заполнение прогресс-бара
    int bonusCost; // цена улучшения (на которую поднимаем доход)
    int maxClick; // кол-во дробей прогресс-бара
    int exp = 2; // кол-во опыта за клик по кнопке
    int expU = 15; // кол-во опыта за улучшение доходности
    int upgradeCost; //максимальное число EXP. Начало с 200
    float upgradeCoef = 1.12; // коеф. сложности upgradeCost, для ноута - 1,12. 
    int upgradePoints; // кол-во очков улучшений (ОУ)

    /*-- появляется после покупки автомайна --*/
    float timeUpgrade; // Ускорение майна. Начинается с 30% и падает на 2,5% каждое улучшение
    float profiteUpgrade; // Увеличение доходности майнинга. Начинается с 35% и падает на 2.5% каждое улучшение
                          /*!-- появляется после покупки автомайна --*/

    int clickCounter; // Счетчик кликов по кнопке
    int level; // Уровень развития компьютера. Повышается на +1 каждые 50 кликов. Не зависит от EXP 
    int levelCost; // Цена уровня (кол-во кликов)

    AutoMiner autoMiner;
    string jsonParts;
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

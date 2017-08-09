using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Sprite[] background;
    
    public Text moneyText; //Деньги в $
    public float money = 0;
    public GameObject improvementWin; //окно улучшений и запчастей
    public Button autoMinerButton; //кнопка покупки автомайнера
    public Button upgradeTimeButton; //кнопка покупки улучшения времени
    public Button upgradeProfitButton; //кнопка покупки улучшения дохода
    public Text upgradePointsText; //Текст очки улучшения
    public Text levelText; //Текст номер уровня
    public Text nameText; //Текст названия компьютера
    public Text pribyl; //Прибыль на данный момент
    public Text time; //Время заполнения автомайнером 
    public Text sr_pr; //средняя прибыль
    public Text prBarText; //количество опыта 
    public Image fill; //заполнение прогрессбара

    public Part[] parts;
    public typeOfPart[] typesOfParts;

    void Start()
    {
        money = 80f;
        /*Объявление типов компонентов. ID в массиве должно совпадать с Id типа!!!!! НЕ МЕНЯТЬ ID GPU!!!!!*/
        typesOfParts = new typeOfPart[3];
        typesOfParts[0] = new typeOfPart(0, "CPU");
        typesOfParts[1] = new typeOfPart(1, "GPU");
        typesOfParts[2] = new typeOfPart(2, "Power");

        /*Объявление компонентов*/
        parts = new Part[4];
        parts[1] = new Part(1, "Intel Core i5-6600K 3.5 GHz", typesOfParts[0], 8, 15, 215, 167);
        parts[2] = new Part(2, "Электропитание", typesOfParts[2], 25, 35, 847, 540);
        parts[0] = new Part(0, "AMD Radeon RX 470", typesOfParts[1], 10, 25, 2, 10);
        parts[3] = new Part(3, "AMD Radeon RX 470", typesOfParts[1], 10, 25, 2, 10);
       

        this.autoMinerButton = improvementWin.transform.Find("Background/AutoMiner/GameObject/BuyAuto").gameObject.GetComponent<Button>();
        this.upgradeTimeButton = improvementWin.transform.Find("Background/UpgradeGroup/TimeUpgrade").gameObject.GetComponent<Button>();
        this.upgradeProfitButton = improvementWin.transform.Find("Background/UpgradeGroup/ProfitUpgrade").gameObject.GetComponent<Button>();
        //this.upgradePointsText = improvementWin.transform.Find("ScrollContent/UpgradePoints").gameObject.GetComponent<Text>();
        this.upgradePointsText = improvementWin.transform.Find("Background/PointGroup/UpgradePoints").gameObject.GetComponent<Text>();
        this.levelText = improvementWin.transform.Find("Background/Header/LevelText").gameObject.GetComponent<Text>();
    }

    public void openCloseImprovementWin()
    {
        GameObject partsContainer = improvementWin.transform.Find("Background/Parts").gameObject;
        int childs = partsContainer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(partsContainer.transform.GetChild(i).gameObject);
        }
        improvementWin.SetActive(!improvementWin.activeSelf);
    }

}

[System.Serializable]
public class Part
{
    /*-- JSON-сериализация --*/
    public int id; // Номер компонента в списке улучшений
    public typeOfPart type; // Тип компонента
    public string partName; // Имя компонента (AMD RADEON RX480 4GB)
    public float reliabilityNew; // Вероятность поломки каждые 100 заполнений прогресс-бара. 0-10% для Новых, 0-25% для Б/У
    public float reliabilityUsed;
    public int costNew; // Цена нового компонента
    public int costUsed; // Цена Б/У компонента
    public string image; // картиночка
    /*!-- JSON-сериализация --*/

    public Part(int id, string name, typeOfPart type, float reliabilityNew, float reliabilityUsed, int costNew, int costUsed, string image = "")
    {
        this.id = id;
        partName = name;
        this.type = type;
        this.reliabilityNew = reliabilityNew;
        this.reliabilityUsed = reliabilityUsed;
        this.costNew = costNew;
        this.costUsed = costUsed;
        this.image = image;
    }
}

[System.Serializable]
public class typeOfPart
{
    public int id; // Номер типа
    public string name; // Имя типа
    public typeOfPart(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
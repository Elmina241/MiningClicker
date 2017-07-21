using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Text moneyText; //Деньги в $
    public float money = 0;
    public GameObject improvementWin; //окно улучшений и запчастей
    public Button autoMinerButton; //кнопка покупки автомайнера
    public Button upgradeTimeButton; //кнопка покупки улучшения времени
    public Button upgradeProfitButton; //кнопка покупки улучшения дохода
    public Text upgradePointsText; //Текст очки улучшения
    public Text levelText; //Текст номер уровня

    void Start()
    {
        this.autoMinerButton = improvementWin.transform.Find("AutoMiner/BuyAuto").gameObject.GetComponent<Button>();
        this.upgradeTimeButton = improvementWin.transform.Find("TimeUpgrade").gameObject.GetComponent<Button>();
        this.upgradeProfitButton = improvementWin.transform.Find("ProfitUpgrade").gameObject.GetComponent<Button>();
        this.upgradePointsText = improvementWin.transform.Find("UpgradePoints").gameObject.GetComponent<Text>();
        this.levelText = improvementWin.transform.Find("LevelText").gameObject.GetComponent<Text>();
    }

    public void openCloseImprovementWin()
    {
        improvementWin.SetActive(!improvementWin.activeSelf);
    }

}

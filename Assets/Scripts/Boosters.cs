using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boosters : MonoBehaviour {

    System.DateTime lastStartProfit;
    System.DateTime lastStartTime;
    public Game g;
    public Achievment a;
    public int rechargeTimeP = 5;
    public int rechargeTimeT = 5;
    public int workTimeP = 30;
    public int workTimeT = 15;
    public GameObject ProfitBoosterButton;
    public GameObject TimeBoosterButton;
    
    // Use this for initialization
    void Start () {

        rechargeTimeP = 5;
        rechargeTimeT = 5;
        lastStartProfit = new System.DateTime(System.DateTime.Now.Year, 1, 1);
        lastStartTime = new System.DateTime(System.DateTime.Now.Year, 1, 1);

    }
	
	// Update is called once per frame
	void Update () {

        if (a._store[0].isBought)
        {
            if (g.isProfitBoosterOn) g.isProfitBoosterOn = (System.DateTime.Now - lastStartProfit).Seconds <= workTimeP;
            if ((System.DateTime.Now - lastStartProfit).Days > 7)
            {
                ProfitBoosterButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                ProfitBoosterButton.GetComponent<Button>().interactable = ((System.DateTime.Now - lastStartProfit).Seconds > (rechargeTimeP + workTimeP));
            }
            if (!ProfitBoosterButton.GetComponent<Button>().interactable)
            {
                ProfitBoosterButton.transform.GetChild(0).GetComponent<Text>().text = "00:" + ((rechargeTimeP + workTimeP) - (System.DateTime.Now - lastStartProfit).Seconds).ToString("0#");
                ProfitBoosterButton.GetComponent<Image>().sprite = GetComponent<Achievment>().BuyBtn_gray;
            }
            else
            {
                ProfitBoosterButton.transform.GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[2]; //"Включить";
                ProfitBoosterButton.GetComponent<Image>().sprite = GetComponent<Achievment>().Btn_violet;
            }
        }

        if (a._store[1].isBought)
        {
            if (g.isTimeBoosterOn) g.isTimeBoosterOn = (System.DateTime.Now - lastStartTime).Seconds <= workTimeT;
            if ((System.DateTime.Now - lastStartTime).Days > 7)
            {
                TimeBoosterButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                TimeBoosterButton.GetComponent<Button>().interactable = ((System.DateTime.Now - lastStartTime).Seconds > (rechargeTimeT + workTimeT));
            }
            if (!TimeBoosterButton.GetComponent<Button>().interactable)
            {
                TimeBoosterButton.transform.GetChild(0).GetComponent<Text>().text = "00:" + ((rechargeTimeT + workTimeT) - (System.DateTime.Now - lastStartTime).Seconds).ToString("0#");
                TimeBoosterButton.GetComponent<Image>().sprite = GetComponent<Achievment>().BuyBtn_gray;
            }
            else
            {
                TimeBoosterButton.transform.GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[2]; //"Включить";
                TimeBoosterButton.GetComponent<Image>().sprite = GetComponent<Achievment>().Btn_violet;
            }
        }
    }

    public bool checkProfitBooster()
    {
        if ((System.DateTime.Now - lastStartProfit).Days > 7) return true;
        else return (System.DateTime.Now - lastStartProfit).Seconds > (rechargeTimeP + workTimeP);
    }
    public bool checkTimeBooster()
    {
        if ((System.DateTime.Now - lastStartTime).Days > 7) return true;
        else return (System.DateTime.Now - lastStartTime).Seconds > (rechargeTimeT + workTimeT);
    }

    public void startProfitBooster()
    {
        if (checkProfitBooster())
        {
            lastStartProfit = System.DateTime.Now;
            g.isProfitBoosterOn = true;
        }
    }

    public void startTimeBooster()
    {
        if (checkTimeBooster())
        {
            lastStartTime = System.DateTime.Now;
            g.isTimeBoosterOn = true;
        }
    }
}

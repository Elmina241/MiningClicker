using UnityEngine;
using System.Collections;

public class Boosters : MonoBehaviour {

    System.DateTime lastStartProfit;
    System.DateTime lastStartTime;
    public Game g;
    public int rechargeTimeP = 5;
    public int rechargeTimeT = 5;
    public int workTimeP = 30;
    public int workTimeT = 15;
    /*public bool isProfitBoosterReady;
    public bool isTimeBoosterReady;*/

    // Use this for initialization
    void Start () {

        rechargeTimeP = 5;
        rechargeTimeT = 5;
        lastStartProfit = System.DateTime.Now;
        lastStartTime = System.DateTime.Now;

    }
	
	// Update is called once per frame
	void Update () {

        if (g.isProfitBoosterOn) g.isProfitBoosterOn = (System.DateTime.Now - lastStartProfit).Seconds <= workTimeP;
        if (g.isTimeBoosterOn) g.isTimeBoosterOn = (System.DateTime.Now - lastStartTime).Seconds <= workTimeT;

    }

    public bool checkProfitBooster()
    {
        return (System.DateTime.Now - lastStartProfit).Seconds > (rechargeTimeP + workTimeP);
    }
    public bool checkTimeBooster()
    {
        return (System.DateTime.Now - lastStartTime).Seconds > (rechargeTimeT + workTimeT);
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

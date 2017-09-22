using UnityEngine;
using System.Collections;

public class Boosters : MonoBehaviour {

    System.DateTime lastStartProfit;
    System.DateTime lastStartTime;
    public Game g;
    public int rechargeTimeP = 120;
    public int rechargeTimeT = 120;
    public int workTimeP = 30;
    public int workTimeT = 30;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator countProfitWork()
    {
        yield return new WaitForSecondsRealtime(workTimeP);
    }

    public bool checkProfitBooster()
    {
        return (System.DateTime.Now - lastStartProfit).Seconds > rechargeTimeP;
    }
    public bool checkTimeBooster()
    {
        return (System.DateTime.Now - lastStartTime).Seconds > rechargeTimeT;
    }

    public void startProfitBooster()
    {
        if (checkProfitBooster()) g.isProfitBoosterOn = true;
    }

    public void startTimeBooster()
    {
        if (checkTimeBooster()) g.isTimeBoosterOn = true;
    }
}

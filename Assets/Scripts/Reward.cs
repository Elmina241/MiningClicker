using UnityEngine;
using System;
using UnityEngine.UI;

public class Reward : MonoBehaviour {

    public float _hours = 0.0f;
    public float _minutes = 0.0f;
    public float _sec = 0.0f;
    public GameObject g;
    public Game gObj;
    private ulong lastChestOpen;

    private void Start()
    {
        lastChestOpen = ulong.Parse(PlayerPrefs.GetString("LastChestOpen"));

        if (!IsChestReady())
        {
            g.SetActive(false); 
        }      
    }

    private void Update()
    {
        if (!g.activeSelf)
        {            
            if (IsChestReady())
            {               
                g.SetActive(true);
            }       
        }
    }

    public void ChestClick()
    {
        //награда
        int rew = 0;
        switch (gObj.getCurrent())
        {
            case 0:
                rew = UnityEngine.Random.Range(9, 15);
                break;
            case 1:
                UnityEngine.Random.Range(20, 30);
                break;
            case 2:
                UnityEngine.Random.Range(40, 80);
                break;
            case 3:
                UnityEngine.Random.Range(100, 170);
                break;
            case 4:
                UnityEngine.Random.Range(200, 300);
                break;
            case 5:
                UnityEngine.Random.Range(310, 500);
                break;
            case 6:
                UnityEngine.Random.Range(550, 1000);
                break;
            case 7:
                UnityEngine.Random.Range(1200, 2000);
                break;
            case 8:
                UnityEngine.Random.Range(800, 1500);
                break;
            case 9:
                UnityEngine.Random.Range(370, 1700);
                break;
            case 10:
                UnityEngine.Random.Range(350, 840);
                break;
            case 11:
                UnityEngine.Random.Range(310, 2000);
                break;
            default:
                break;
        }
        gObj.money += rew;
        gObj.moneyText.text = "$" + gObj.money.ToString("0.#0");
        //g.GetComponent<Game>().push.transform.Find("Icon").GetComponent<Image>().sprite = transform.Find("MenuButton/Icon").GetComponent<Image>().sprite;
        gObj.push.transform.Find("Header").GetComponent<Text>().text = "Ежедневная награда!";
        gObj.push.transform.Find("Description").GetComponent<Text>().text = "Получено $" + rew.ToString();
        gObj.push.GetComponent<Animator>().SetTrigger("isShown");
        lastChestOpen = (ulong)DateTime.Now.Ticks; // текущее время (прошлое)
        PlayerPrefs.SetString("LastChestOpen", lastChestOpen.ToString());
        g.SetActive(false);
    }

    private bool IsChestReady()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastChestOpen); // разница между текущим и прошлым текущим
        ulong m = diff / TimeSpan.TicksPerMillisecond; // перевод в миллисекунды

        float secondsLeft = ((((_hours*3600.0f)+(_minutes*60.0f)+_sec)*1000.0f) - m) / 1000.0f; // перевод в секунды
        Debug.Log(secondsLeft);
        return (secondsLeft < 0);
    }
}

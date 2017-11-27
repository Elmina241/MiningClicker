using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{

    public float _hours = 0.0f;
    public float _minutes = 0.0f;
    public float _sec = 0.0f;
    public GameObject g;
    public Game gObj;
    public Sprite spr;
    private ulong lastChestOpen;
    public string[] items;
    public ulong unixTime; //текущее время
   

    private void Start()
    {
      
        lastChestOpen = ulong.Parse(PlayerPrefs.GetString("LastChestOpen"));
        print(lastChestOpen);
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
    //// КОД НИЖЕ ПАРСИТ В UNIX-ФОРМАТЕ ВРЕМЯ С СЕРВЕРА
    public IEnumerator GetTime()
    {
        WWW itemsData = new WWW("http://worldclockapi.com/api/json/est/now");
        yield return itemsData;
        string result = itemsData.text;
        items = result.Split(',');
        //Debug.Log(GetValueTime(items[6], "\"currentFileTime\":"));
        unixTime = ulong.Parse(GetValueTime(items[6], "\"currentFileTime\":"));
    }

    string GetValueTime(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        return value;
    }

    public void ChestClick()
    {
        //награда
        int rew = 0;
        switch (gObj.getCurrent())
        {
            case 0:
                rew = Random.Range(9, 15);
                break;
            case 1:
                rew = Random.Range(20, 30);
                break;
            case 2:
                rew = Random.Range(40, 80);
                break;
            case 3:
                rew = Random.Range(100, 170);
                break;
            case 4:
                rew = Random.Range(200, 300);
                break;
            case 5:
                rew = Random.Range(310, 500);
                break;
            case 6:
                rew = Random.Range(550, 1000);
                break;
            case 7:
                rew = Random.Range(1200, 2000);
                break;
            case 8:
                rew = Random.Range(800, 1500);
                break;
            case 9:
                rew = Random.Range(370, 1700);
                break;
            case 10:
                rew = Random.Range(350, 840);
                break;
            case 11:
                rew = Random.Range(310, 2000);
                break;
            default:
                break;
        }
        gObj.money += rew;
        gObj.moneyText.text = "$" + gObj.money.ToString("0.#0");
        gObj.GetComponent<Game>().push.transform.Find("Icon").GetComponent<Image>().sprite = spr;
        gObj.push.transform.Find("Header").GetComponent<Text>().text = LangSystem.lng.push[0]; //"Ежедневная награда!";
        gObj.push.transform.Find("Description").GetComponent<Text>().text = LangSystem.lng.push[1] + " $" + rew.ToString();
        gObj.push.GetComponent<Animator>().SetTrigger("isShown");
        StartCoroutine(GetTime());
        //GetTime();
        lastChestOpen = unixTime;
        PlayerPrefs.SetString("LastChestOpen", lastChestOpen.ToString());
        g.SetActive(false);
    }


    private bool IsChestReady()
    {
        StartCoroutine(GetTime());
        //GetTime();
        ulong diff = (unixTime - lastChestOpen); // разница между текущим и прошлым текущимы
        ulong m = diff / 10000000; // перевод в секунды
        float secondsLeft = ((_hours * 3600.0f) + (_minutes * 60.0f) + _sec) - m; // секунд осталось
        //print(secondsLeft);
        return (secondsLeft < 0);
    }
}

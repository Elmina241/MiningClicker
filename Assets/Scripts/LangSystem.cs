using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;

public class LangSystem : MonoBehaviour
{
    private string json = "";
    public static lang lng = new lang();
    private string[] langArray = { "ru_RU", "en_US" };

    bool s;
    public Text t;

    private void LangLoad()
    {

        //string path = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerPrefs.GetString("Language") + ".json");
        //WWW reader = new WWW(path);
        //while (!reader.isDone){          
        //}
        //string sr = Application.streamingAssetsPath + "/sees.json";
        //string json3 = JsonUtility.ToJson(lng);
        //File.WriteAllText(sr, json3);

        //json = reader.text;
        //lng = JsonUtility.FromJson<lang>(json);

#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerPrefs.GetString("Language") + ".json");
        WWW reader = new WWW(path);
        while (!reader.isDone) { }
        json = reader.text;
#else
        json = File.ReadAllText(Application.streamingAssetsPath + "/Languages/" + PlayerPrefs.GetString("Language") + ".json");
#endif
        lng = JsonUtility.FromJson<lang>(json);

    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
                PlayerPrefs.SetString("Language", "ru_RU");
            else PlayerPrefs.SetString("Language", "en_US");
        }
        LangLoad();

    }



    public void swBtn(int index)
    {
        PlayerPrefs.SetString("Language", langArray[index - 1]);
        LangLoad();
    }

}

[System.Serializable]
public class lang
{
    public string[] panelName = new string[5];
    public string[] settingsParam = new string[6];
    public string[] achievementText = new string[6];
    public string[] achArrHeader = new string[9];
    public string[] achArrDescrip = new string[9];
    public string[] storeArrHeader = new string[6];
    public string[] storeArrDescription = new string[6];
    public string[] storeDetails = new string[5];
    public string[] casino = new string[3];
    public string[] improvementWins = new string[21];
    public string[] push = new string[8];
    public string[] game = new string[6];
    public string[] windows = new string[5];
    public string[] learning = new string[10];
}
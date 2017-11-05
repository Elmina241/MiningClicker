using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class LangSystem : MonoBehaviour
{
    private string json = "";
    public static lang lng = new lang();
    private string[] langArray = { "ru_RU", "en_US" };


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

    private void LangLoad()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
                string path = Path.Combine(Application.streamingAssetsPath, "/Languages/" + PlayerPrefs.GetString("Language") + ".json");
                WWW reader = new WWW(path);
                while (!reader.isDone) { }
                json = reader.text;
#endif
#if UNITY_EDITOR
        json = File.ReadAllText(Application.streamingAssetsPath + "/Languages/" + PlayerPrefs.GetString("Language") + ".json");
        lng = JsonUtility.FromJson<lang>(json);
    }
#endif


    public void SwitchButton(int index)
    {
        PlayerPrefs.SetString("Language", langArray[index - 1]);
        LangLoad();
    }
}

public class lang
{
    public string[] panelName = new string[4];
    public string[] settingsParam = new string[3];
    public string[] achievementText = new string[6];
    public string[] achArrHeader = new string[9];
    public string[] achArrDescrip = new string[9];
    public string[] storeArrHeader = new string[10];
    public string[] storeArrDescription = new string[6];
    public string[] casino = new string[3];
    public string[] improvementWins = new string[19];
    public string[] push = new string[4];
    public string[] game = new string[3];
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RateMe : MonoBehaviour
{
    public Game g;
    public bool getRate = false;
    public int c;
    private void Start()
    {
        gameObject.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = LangSystem.lng.windows[2];
        gameObject.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.windows[3];
        gameObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.windows[4];
        if (!PlayerPrefs.HasKey("getrate"))
        {
            PlayerPrefs.SetInt("getRate", 0);
        }
        else
        {
            c = PlayerPrefs.GetInt("getRate");
            if (c == 0)
            {
                getRate = false;
            }
            else
                getRate = true;
        }        
    }

    public void RateOff()
    {
        bool exit = gameObject.GetComponent<Animator>().GetBool("exit");
        if (exit)
        {
            gameObject.SetActive(false);
            if (!getRate)
            {
                g.money += 200;
                getRate = true;
                c = 1;
                PlayerPrefs.SetInt("getRate", c);
            }
           
        }

    }

    public void OpenGP()
    {
        Application.OpenURL("market://details?id=com.miningclicker.ds");
    }

    public void RateMePleaseBitchExit()
    {
        //Сюда код перед закрытием (ссылка на ГП)
        gameObject.GetComponent<Animator>().SetBool("exit", true);
    }
}

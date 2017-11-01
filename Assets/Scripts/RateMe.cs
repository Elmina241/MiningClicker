using UnityEngine;
using System.Collections;

public class RateMe : MonoBehaviour
{
    public void RateOff()
    {
        bool exit = gameObject.GetComponent<Animator>().GetBool("exit");
        if (exit) gameObject.SetActive(false);
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

using UnityEngine;
using System;
using UnityEngine.UI;

public class Reward : MonoBehaviour {

    public float _hours = 0.0f;
    public float _minutes = 0.0f;
    public float _sec = 0.0f;
    public GameObject g;
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

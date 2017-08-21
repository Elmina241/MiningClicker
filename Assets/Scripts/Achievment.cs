using UnityEngine;
using System.Collections;

public class Achievment : MonoBehaviour {

    public Color yes;
    public Color not;
    public Sprite _received;
    public Sprite _N_received;
    public Transform Parent_ach;
    public Achievment_sys[] achievment;

    void Start()
    {
        Ach();
    }

    public void Ach()
    {
        for (int i = 0; i < achievment.Length; i++)
        {
            Parent_ach.GetChild(i).GetComponent<Ach_pref>().icon.sprite = achievment[i].icon;
            Parent_ach.GetChild(i).GetComponent<Ach_pref>()._header.text = achievment[i]._header.ToUpper();
            Parent_ach.GetChild(i).GetComponent<Ach_pref>()._description.text = achievment[i]._description.ToUpper();
            Parent_ach.GetChild(i).GetComponent<Ach_pref>().award.text = "+" + conversionFunction(achievment[i].award);
            if (achievment[i].get)
            {
                Parent_ach.GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get;
                Parent_ach.GetChild(i).GetComponent<Ach_pref>().check.sprite = _received;
                Parent_ach.GetChild(i).GetComponent<Ach_pref>().check.color = yes;
            }
            else
            {
                Parent_ach.GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get;
                Parent_ach.GetChild(i).GetComponent<Ach_pref>().check.sprite = _N_received;
                Parent_ach.GetChild(i).GetComponent<Ach_pref>().check.color = not;
            }

        }
    }

    private string conversionFunction(int number)
    {
        string converted = number.ToString("#,##0");
        converted = converted.Replace(",", " ");
        return converted;
    }
}

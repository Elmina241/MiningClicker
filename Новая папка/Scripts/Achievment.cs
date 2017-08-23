using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievment : MonoBehaviour {

    public Sprite Gradient_store, Gradient_achievment;
    public GameObject Header;
    public Color yes;
    public Color not;
    public Sprite _received;
    public Sprite _N_received;
    public Transform Parent_ach;
    public GameObject store_prefab;
    public GameObject achievment_prefab;
    public ScrollRect scr;
    public RectTransform store, achiev;
    public Achievment_sys[] achievment;


    //public void openClose()
    //{
    //    GameObject partsContainer = improvementWin.transform.Find("Background/Parts").gameObject;
    //    int childs = partsContainer.transform.childCount;
    //    for (int i = childs - 1; i >= 0; i--)
    //    {
    //        GameObject.Destroy(partsContainer.transform.GetChild(i).gameObject);
    //    }
    //    improvementWin.SetActive(!improvementWin.activeSelf);
    //}

    public void Store_set()
    {
       
        Header.GetComponent<Image>().sprite = Gradient_store;
        Header.transform.GetChild(1).GetComponent<Text>().text = "МАГАЗИН";
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Store_btn/Icon_Bg/Icon").GetComponent<Image>().sprite;
        
        Instantiate(store_prefab, Parent_ach, false);
        scr.content = store;
    }

    public void Achievment_set()
    {
        Header.GetComponent<Image>().sprite = Gradient_achievment;
        Header.transform.GetChild(1).GetComponent<Text>().text = "ЗАДАНИЯ";
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Achievment_btn/Icon_Bg/Icon").GetComponent<Image>().sprite;
       // Parent_ach.GetComponent<ScrollRect>().content = 
        Instantiate(achievment_prefab, Parent_ach, false);
        for (int i = 0; i < achievment.Length; i++)
        {
            Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().icon.sprite = achievment[i].icon;
            Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>()._header.text = achievment[i]._header.ToUpper();
            Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>()._description.text = achievment[i]._description.ToUpper();
            Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().award.text = "+" + conversionFunction(achievment[i].award);
            if (achievment[i].get)
            {
                Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get;
                Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.sprite = _received;
                Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.color = yes;
            }
            else
            {
                Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get;
                Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.sprite = _N_received;
                Parent_ach.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.color = not;
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

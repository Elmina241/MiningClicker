using UnityEngine;
using UnityEngine.UI;

public class Ach_pref : MonoBehaviour {

    public Image icon;
    public Text _header;
    public Text _description;
    public Text award;
    public int _award;
    public Image check;   
    public bool get;

    void Start()
    {
        award.text = "+" + conversionFunction(_award);
    }

    // ОЧЕНЬ СТРАШНЫЙ КОД, КОТОРЫЙ ДЕЛАЕТ ЛЮБУЮ ЦИФРУ КРАСИВОЙ
    private string conversionFunction(int number)
    {
        string converted = number.ToString("#,##0");
        converted = converted.Replace(",", " ");
        return converted;
    }
}

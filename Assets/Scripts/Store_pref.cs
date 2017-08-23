using UnityEngine;
using UnityEngine.UI;

public class Store_pref : MonoBehaviour {

    public Image icon, Btn;
    public Text _header;
    public Text _description;
    public Text price;
    public bool isBought, isReal;
    public float _priceReal;
    public int _priceGame;    

    void Start(){
                
        if (isReal)
        {
            price.text = _priceReal.ToString()+"р";
        }
        else
        {            
            price.text = "$"+(_priceGame/1000).ToString()+"k";
        }
        if (isBought)
        {
            gameObject.transform.GetChild(6).GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.transform.GetChild(6).GetComponent<Button>().interactable = true;
        }
    }
}

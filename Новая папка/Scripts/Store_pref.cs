using UnityEngine;
using UnityEngine.UI;

public class Store_pref : MonoBehaviour {
   
    public Text price;
    public int _price;

    void Start(){
                
        if (_price > 1000)
        {
            _price = _price / 1000;
            price.text = "$" + _price.ToString()+"k";
        }
    }
}

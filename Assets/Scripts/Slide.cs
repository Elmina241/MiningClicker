using UnityEngine;
using UnityEngine.UI;


public class Slide : MonoBehaviour {

    public Slider sld;
	

	// Update is called once per frame
	public void Sisya() {
        gameObject.GetComponent<Image>().fillAmount = sld.value;
	}
}

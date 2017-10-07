using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour {

    [SerializeField]
    private bool _sound = true;
    public Text textBtn;
	
    public void SoundChange()
    {
        if (_sound)
        {
            _sound = false;
            textBtn.text = "ВЫКЛ";
            //Отключаем AudioSourse 
        }
        else
        {
            _sound = true;
            textBtn.text = "ВКЛ";
        }
            
    }
	
}

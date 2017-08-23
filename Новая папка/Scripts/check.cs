using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class check : MonoBehaviour {

    public Color grey, green;
    public Image checkmark;
    Color transparent;
    public void setOn()
    {
        transparent = checkmark.color;
        transparent.a = 1f;
        checkmark.color = transparent;
        checkmark.color = green;       
            
    }
    public void setOff()
    {
        transparent = checkmark.color;
        transparent.a = 1f;
        checkmark.color = transparent;       
        checkmark.color = grey;  
       
    }
    public void Disable()
    {
        transparent = checkmark.color;
        transparent.a = 0f;
        checkmark.color = transparent;
    }


}

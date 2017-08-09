using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class check : MonoBehaviour {

    public Color grey, green;
    public Image checkmark;
    public void setOn()
    {
            checkmark.color = green;
    }
    public void setOff()
    {
            checkmark.color = grey;
    }


}

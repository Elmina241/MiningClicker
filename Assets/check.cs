using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class check : MonoBehaviour {

    public Color grey, green;
    public Image checkmark;
    void Start()
    {
        checkmark.color = grey;
    }
    public void click()
    {
        if (checkmark.color != green)
            checkmark.color = green;
        else
            checkmark.color = grey;

    }

	
}

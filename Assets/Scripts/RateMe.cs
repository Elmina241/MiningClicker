using UnityEngine;
using System.Collections;

public class RateMe : MonoBehaviour {

    
    public void RateOn()
    {
       
    }

    public void RateOff()
    {
        
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            RateOn();
        }
    }
}

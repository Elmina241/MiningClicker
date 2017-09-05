using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

    // Use this for initialization
    void AnimationComplete()
    {
        gameObject.SetActive(false);
    }
   
}

using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

    // Use this for initialization
    void AnimationComplete()
    {
        float temp = gameObject.GetComponent<Animator>().playbackTime;
        if (gameObject.activeSelf && gameObject.GetComponent<Animator>().playbackTime==-1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
            
    }
   
}

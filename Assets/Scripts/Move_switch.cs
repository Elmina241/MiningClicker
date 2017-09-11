using UnityEngine;
using System.Collections;

public class Move_switch : MonoBehaviour {

    public bool move;
    public void Move_back()
    {
        if (!move)
        {
            gameObject.GetComponent<Animator>().SetBool("sw", true);
            move = true;
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("sw", false);
            move = false;
        }

    }

    public void Move_to()
    {
      gameObject.GetComponent<Animator>().SetBool("sw", false);
    }
    
}

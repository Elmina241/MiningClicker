using UnityEngine;
using System.Collections;

public class Move_switch : MonoBehaviour {

    public bool move;

    public void Move_back()
    {
        if (!move)
        {
            gameObject.GetComponent<Animator>().SetBool("switch", true);
            move = true;
            if(gameObject.tag == "Music")
            {
                print("Музыка отключена");
            }
            else if(gameObject.tag == "Sound")
            {
                print("Звуки отключены");
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("switch", false);
            move = false;
            if (gameObject.tag == "Music")
            {
                print("Музыка включена"); // сюда вставить AudioSource с музыкой 
            }
            else if (gameObject.tag == "Sound")
            {
                print("Звуки включены"); // сюда вставить AudioSource со звуками
            }
        }

    }

    public void Move_to()
    {
      gameObject.GetComponent<Animator>().SetBool("sw", false);
    }
    
}

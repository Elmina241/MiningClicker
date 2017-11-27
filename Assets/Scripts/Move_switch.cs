using UnityEngine;
using System.Collections;

public class Move_switch : MonoBehaviour {

    public bool move;
    public int sound = 0;
    public int music = 0;
    public AudioSource g;
    public AudioSource c;


    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
            gameObject.GetComponent<Animator>().SetBool("switch", true);
        }
        else
        {
            music = PlayerPrefs.GetInt("Music");
            if (gameObject.tag == "Music" && music == 0)
            {
                gameObject.GetComponent<Animator>().SetBool("switch", false);
            }
               
        }
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
            gameObject.GetComponent<Animator>().SetBool("switch", true);
        }
        else
        {
            sound = PlayerPrefs.GetInt("Sound");
            if(gameObject.tag == "Sound" && sound == 0)
            {
                gameObject.GetComponent<Animator>().SetBool("switch", false);
            }
        }
    }

    public void Move_back()
    {
        if (!move)
        {
            gameObject.GetComponent<Animator>().SetBool("switch", true);
            move = true;
            if(gameObject.tag == "Music")
            {
                c.mute = false;
                print("Музыка включена");
                PlayerPrefs.SetInt("Music", 1);
            }
            else if(gameObject.tag == "Sound")
            {
                g.mute = false;
                print("Звуки включены");
                PlayerPrefs.SetInt("Sound", 1);
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("switch", false);
            move = false;
            if (gameObject.tag == "Music")
            {
                c.mute = true;
                print("Музыка выключена");
                PlayerPrefs.SetInt("Music", 0);// сюда вставить AudioSource с музыкой 
            }
            else if (gameObject.tag == "Sound")
            {
                g.mute = true;
                print("Звуки выключены");
                PlayerPrefs.SetInt("Sound", 0); // сюда вставить AudioSource со звуками
            }
        }

    }

    public void Move_to()
    {
      gameObject.GetComponent<Animator>().SetBool("sw", false);
    }
    
}

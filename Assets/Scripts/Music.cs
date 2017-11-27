using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{

    public AudioSource g;
    public int music;
    public bool move;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {

            music = 1;
            PlayerPrefs.SetInt("Music", music);

            g.mute = false;

            gameObject.GetComponent<Animator>().SetBool("switch", true);


        }
        else
        {
            music = PlayerPrefs.GetInt("Music");
            if (music == 1)
            {
                g.mute = false;
                g.Play();
                gameObject.GetComponent<Animator>().SetBool("switch", true);
            }
            else
            {
                g.mute = true;
                gameObject.GetComponent<Animator>().SetBool("switch", false);
                g.Stop();
            }
        }
    }

    public void Switch()
    {
        if (music == 0)
        {
            music = 1;
            gameObject.GetComponent<Animator>().SetBool("switch", true);
            PlayerPrefs.SetInt("Music", music);
            g.mute = false;
            g.Play();
        }
        else
        {
            music = 0;
            gameObject.GetComponent<Animator>().SetBool("switch", false);
            PlayerPrefs.SetInt("Music", music);
            g.mute = true;
            g.Stop();
        }
    }
}

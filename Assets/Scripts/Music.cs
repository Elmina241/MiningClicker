using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    /* Нормальный код */
    private bool clickMusic = false; // булл на проверку клика
    private bool clickSounds = false; // булл на проверку клика

    private bool currentStateMusic = true; // булл на проверку вкл/выкл музыки
    private bool currentStateSounds = true; // булл на проверку вкл/выкл звуков

    Vector3 startPos; // начальное положение маркера 
    Vector3 endPos; // конечное положение маркера

    void Start()
    {
        if (currentStateMusic.GetHashCode() == 1)
        {
            PlayerPrefs.SetInt("MusicState", currentStateMusic.GetHashCode());

        }
        else
        {
            PlayerPrefs.SetInt("MusicState", currentStateMusic.GetHashCode());
        }
        if (currentStateSounds.GetHashCode() == 1)
        {
            PlayerPrefs.SetInt("SoundState", currentStateSounds.GetHashCode());
        }
        else
        {
            PlayerPrefs.SetInt("SoundState", currentStateSounds.GetHashCode());
        }
    }

    void SetPosition(string component, int state)
    {
        if(component == "music")
        {

        }
    }

    /* Нормальный код */
    public Game g;
    //Vector3 startPos, endPos;
    int music;
    //public bool click = false;
    float time = 0.02f;
    float currentTime = 0f;
    public Color On;
    public Color Off;
    
    private float startTime;
    private float distance;
    public float currentDist = 0f;

    void setUp()
    {
        music = PlayerPrefs.GetInt("Music");
        //music = 0;
        if (music != 0)
        {
            startPos = new Vector3(43.8f, 0f, 0f);
            endPos = new Vector3(-43.8f, 0f, 0f);//startPos+(2*startPos);          
        }
        else
        {
            startPos = new Vector3(-43.8f, 0f, 0f);
            endPos = new Vector3(43.8f, 0f, 0f); // startPos + (2 * startPos);
        }
        transform.GetChild(0).transform.localPosition = startPos;
        distance = (endPos - startPos).magnitude;
        print(distance);
    }
    //private void Start()
    //{
    //    setUp();
    //}

    //public void Switch()
    //{
    //    setUp();
    //    click = true;
    //    StartCoroutine(Move());
    //}

    //IEnumerator Move()
    //{
    //    while (click)
    //    {
    //        yield return new WaitForSeconds(time);
    //        currentTime += time;
    //        if (currentDist >= distance)
    //        {
    //            click = false;
    //            StopCoroutine(Move());
    //        }
    //        currentDist = 400f * currentTime;
    //        float Perc = currentDist / distance;
    //        transform.GetChild(0).transform.localPosition = Vector3.Lerp(startPos, endPos, Perc);

    //    }        
    //}

}

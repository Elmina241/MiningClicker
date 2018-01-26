using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public Game g;
    Vector3 startPos, endPos;
    int music;
    public bool click = false;
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
    private void Start()
    {
        setUp();
    }

    public void Switch()
    {
        setUp();
        click = true;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (click)
        {
            yield return new WaitForSeconds(time);
            currentTime += time;
            if (currentDist >= distance)
            {
                click = false;
                StopCoroutine(Move());
            }
            currentDist = 400f * currentTime;
            float Perc = currentDist / distance;
            transform.GetChild(0).transform.localPosition = Vector3.Lerp(startPos, endPos, Perc);

        }        
    }

   
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class clickObj : MonoBehaviour
{

    private bool move;
    private Vector2 randomVector;

    void Update()
    {
        if (!move) return;
        transform.Translate(randomVector * Time.deltaTime * 0.35f);
    }

    public void StartMotion(int scoreIncrease)
    {
        transform.localPosition = Vector2.zero;
        GetComponent<Text>().text = "+" + scoreIncrease;
        randomVector = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        move = true;
        GetComponent<Animation>().Play();
    }

    public void StopMotion()
    {
        move = false;
    }
}

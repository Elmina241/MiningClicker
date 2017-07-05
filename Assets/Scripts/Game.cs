using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour {

    private int money = 0;
    private int score = 0;
    private int bonus = 1;
    public Text scoreText;
    public int maxScore = 10;
    public Text bonusText;
    public Image p;
    // Use this for initialization

    public void OnClick () {
        money += bonus;
        score ++;
        scoreText.text = money.ToString() + "$";
        if (score > maxScore)
        {
            score = 0;
        }
        p.fillAmount = (float)score/(float)maxScore;
    }
    public void GetBonus()
    {
        money = money - bonus;
        bonus = bonus + 2;
        scoreText.text = money.ToString() + "$";
        bonusText.text = bonus.ToString() + "$";
    }
	
	
}

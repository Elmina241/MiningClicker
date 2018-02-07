using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Learning : MonoBehaviour {

    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;
    public GameObject stage5;
    public GameObject stage6;
    public GameObject panelLearn;
    public int stage = 1;
    public GameObject m1;
    public GameObject button;
    // Use this for initialization
    void Start () {
        if (stage < 7) showStage();
	}

    public void startLearning()
    {
        stage = 1;
        showStage();
    }
	
    public void showStage()
    {
        stage1.SetActive(false);
        stage2.SetActive(false);
        stage3.SetActive(false);
        stage4.SetActive(false);
        stage5.SetActive(false);
        stage6.SetActive(false);
        panelLearn.SetActive(false);
        switch (stage)
        {
            case 1:
                stage1.SetActive(true);
                break;
            case 2:
                stage2.SetActive(true);
                break;
            case 3:
                stage3.SetActive(true);
                panelLearn.SetActive(true);
                break;
            case 4:
                stage4.SetActive(true);
                break;
            case 5:
                if (!m1.GetComponent<Computer>().g.improvementWin.activeSelf) m1.GetComponent<Computer>().openCloseImprovementWin();
                stage5.SetActive(true);
                break;
            case 6:
                stage6.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void resetLearning()
    {
        stage = 8;
        showStage();
    }

    public void incStage()
    {
        stage++;
        showStage();
    }

    public void blockButton()
    {
        Button b = button.GetComponent<Button>();
        b.interactable = !(stage == 7);
        
    }

}

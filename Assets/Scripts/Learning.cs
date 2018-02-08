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
    public Button button;
    public GameObject block;
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
                stage1.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[3];
                stage1.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[0];
                stage1.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[1];
                stage1.transform.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[2];
                break;
            case 2:
                stage2.SetActive(true);
                stage2.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[4];
                break;
            case 3:
                stage3.SetActive(true);
                panelLearn.SetActive(true);
                stage3.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[5];
                break;
            case 4:
                stage4.SetActive(true);
                stage4.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[6];
                break;
            case 5:
                if (!m1.GetComponent<Computer>().g.improvementWin.activeSelf) m1.GetComponent<Computer>().openCloseImprovementWin();
                stage5.SetActive(true);
                stage5.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[7];
                break;
            case 6:
                stage6.SetActive(true);
                stage6.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[8];
                stage6.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.learning[9];
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
        button.interactable = !(stage == 7);
        block.SetActive(stage == 7);
    }

}

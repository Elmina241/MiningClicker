using UnityEngine;
using UnityEngine.UI;


public class CoinFlip : MonoBehaviour {

    public Text _betText;
    public Text _resultPlay;
    public Text _resultText;
    public Slider sld;
    public Button btn;
    

    public float _bet;

    private int _choose;        

    public void SetDefault()
    {
        _resultText.text = "";
        _resultPlay.text = "";
        sld.value = 0;
        btn.interactable = true;
    }

    public void BetRange()
    {        
        _betText.text = "$"+(conversionFunction(gameObject.GetComponent<Game>().money * sld.value)).ToString();
    }

    public void ChooseSet(int _choose)
    {
        this._choose = _choose;
    }

    private string conversionFunction(float number)
    {
        string converted = number.ToString("##,###,##0.#0");
        converted = converted.Replace(",", " ");
        return converted;
    }

    private void Param(int rand, string text)
    {
        _resultPlay.text = rand.ToString();
        _resultText.text = text;
    }

    private void Result(int ch, int rand)
    {
        if (ch != rand)
        {
            _choose = 0;
            Param(rand, "Вы проиграли\n" + "$"+conversionFunction(_bet));
            gameObject.GetComponent<Game>().money -= (gameObject.GetComponent<Game>().money * sld.value);
            gameObject.GetComponent<Game>().saveGame();
        }
        else
        {
            _choose = 0;
            Param(rand, "Вы выиграли\n" + "$" + conversionFunction(_bet * 2));
            gameObject.GetComponent<Game>().money += ((gameObject.GetComponent<Game>().money * sld.value) * 2);
            gameObject.GetComponent<Game>().saveGame();
        }
    }

    public void Play()
    {
        _bet = (gameObject.GetComponent<Game>().money * sld.value);
        
        Debug.Log(_bet);
        if (_bet != 0.0f && _choose!=0)
        {
            btn.interactable = false;
            int rnd = Random.Range(1, 3);
            Debug.Log(rnd);
            switch (rnd)
            {
                case 1:
                    {
                        Result(_choose, rnd);
                        break;
                    }
                case 2:
                    {
                        Result(_choose, rnd);
                        break;
                    }
            }
            BetRange();
            btn.interactable = true;
        }       
    }

}

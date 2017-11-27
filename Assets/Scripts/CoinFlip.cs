using UnityEngine;
using UnityEngine.UI;


public class CoinFlip : MonoBehaviour
{

    public Text _betText;
    // public Text _resultPlay;
    public Text _resultText;
    public Image WinIcon;
    public Sprite btc, dollar;
    public Slider sld;
    public Button btn;
    public GameObject btcAnim;
    public GameObject dollarAnim;
    public GameObject Layer;



    public float _bet;

    private int _choose;

    public void SetDefault()
    {
        _resultText.text = string.Empty;
        // _resultPlay.text = "";
        sld.value = 0;
        btn.interactable = true;
        Layer.GetComponent<Animator>().SetBool("win", false);
    }


    public void BetRange()
    {
        _betText.text = "$" + (conversionFunction(gameObject.GetComponent<Game>().money * sld.value)).ToString();
    }

    public void ChooseSet(int _choose)
    {
        if (_choose == 1)
        {
            dollarAnim.GetComponent<Animator>().SetBool("Set", false);
            btcAnim.GetComponent<Animator>().SetBool("Set", true);

            Layer.GetComponent<Animator>().SetBool("win", false);
        }
        else
        {
            btcAnim.GetComponent<Animator>().SetBool("Set", false);
            dollarAnim.GetComponent<Animator>().SetBool("Set", true);
            Layer.GetComponent<Animator>().SetBool("win", false);
        }
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
        //_resultPlay.text = rand.ToString();
        _resultText.text = text;
    }

    private void Result(int ch, int rand)
    {
        if (ch != rand)
        {
            _choose = 0;
            Param(rand, LangSystem.lng.casino[1] + "\n" + "$" + conversionFunction(_bet));
            gameObject.GetComponent<Game>().money -= (gameObject.GetComponent<Game>().money * sld.value);
            gameObject.GetComponent<Game>().saveGame();
            btcAnim.GetComponent<Animator>().SetBool("Set", false);
            dollarAnim.GetComponent<Animator>().SetBool("Set", false);
            if (rand == 1)
            {
                WinIcon.sprite = btc;
                Layer.GetComponent<Animator>().SetBool("win", true);
            }
            else
            {
                WinIcon.sprite = dollar;
                Layer.GetComponent<Animator>().SetBool("win", true);
            }

        }
        else
        {
            _choose = 0;
            Param(rand, LangSystem.lng.casino[2] + "\n" + "$" + conversionFunction(_bet * 2));
            gameObject.GetComponent<Game>().money += ((gameObject.GetComponent<Game>().money * sld.value) * 2);
            gameObject.GetComponent<Game>().saveGame();
            btcAnim.GetComponent<Animator>().SetBool("Set", false);
            dollarAnim.GetComponent<Animator>().SetBool("Set", false);
            if (rand == 1)
            {
                WinIcon.sprite = btc;
                Layer.GetComponent<Animator>().SetBool("win", true);
            }
            else
            {
                WinIcon.sprite = dollar;
                Layer.GetComponent<Animator>().SetBool("win", true);
            }
        }
    }

    public void Play()
    {
        _bet = (gameObject.GetComponent<Game>().money * sld.value);

        Debug.Log(_bet);
        if (_bet != 0.0f && _choose != 0)
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

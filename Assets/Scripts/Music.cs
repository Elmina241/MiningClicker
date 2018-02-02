using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Music : MonoBehaviour
{
    //КОД ВИСИТ НА ОБЪЕКТЕ SETTINGS в Menu_down. Я подписал объект с кодом
    /* Нормальный код */
    [SerializeField]
    private bool clickMusic = false; // булл на проверку клика
    [SerializeField]
    private bool clickSounds = false; // булл на проверку клика
    [SerializeField]
    private bool currentStateMusic = true; // булл на проверку вкл/выкл музыки
    [SerializeField]
    private bool currentStateSounds = true; // булл на проверку вкл/выкл звуков

    public GameObject switchMusic;
    public GameObject switchSounds;
    [SerializeField]
    Vector3 startPosMusic, startPosSounds;
    [SerializeField]// начальное положение маркера 
    Vector3 endPosMusic, endPosSounds; // конечное положение маркера

    public Color switchOn; // цвет включенного маркера
    public Color switchOff; // цвет выключенного маркера

    //Для анимации перемещения свитча
    private float currentTime = 0f; // "пройденное" время перемещения текущее
    private float startTime = 0f;
    private float distance = 0f; // магнитуда между двумя точками
    public float currentDist = 0f;
    private float time = 0.02f;

    void Start()
    {
        DownloadState();
        distance = (new Vector3(43.8f, 0f, 0f) - new Vector3(-43.8f, 0f, 0f)).magnitude;
    }

    //В функции ниже попеременно заданы стартовые позиции маркеров и их конечные пункты для перемещения. Пока без анимаций.
    public void DownloadState()
    {

        if (!PlayerPrefs.HasKey("MusicState") && !PlayerPrefs.HasKey("SoundState")) // Если не существует ключ (первый запуск), то устанавливаем все по дефолту и сохраняем
        {
            PlayerPrefs.SetInt("MusicState", 1); //1, включен
            switchMusic.transform.localPosition = new Vector3(43.8f, 0f, 0f);
            currentStateMusic = true;
            switchMusic.GetComponent<Image>().color = switchOn;

            PlayerPrefs.SetInt("SoundState", 1); //1, включен
            switchSounds.transform.localPosition = new Vector3(43.8f, 0f, 0f);
            currentStateSounds = true;
            switchSounds.GetComponent<Image>().color = switchOn;
        }
        else
        {
            currentStateMusic = System.Convert.ToBoolean(PlayerPrefs.GetInt("MusicState")); // выгрузка состояния музыки
            currentStateSounds = System.Convert.ToBoolean(PlayerPrefs.GetInt("SoundState")); // выгрузка состояния звуков

            if (currentStateMusic.GetHashCode() == 1)
            {
                PlayerPrefs.SetInt("MusicState", currentStateMusic.GetHashCode()); //1, включен
                startPosMusic = new Vector3(43.8f, 0f, 0f);
                endPosMusic = new Vector3(-43.8f, 0f, 0f);
                switchMusic.transform.localPosition = startPosMusic;
                switchMusic.GetComponent<Image>().color = switchOn;
            }
            else
            {
                PlayerPrefs.SetInt("MusicState", currentStateMusic.GetHashCode()); //0, выключен
                startPosMusic = new Vector3(-43.8f, 0f, 0f);
                endPosMusic = new Vector3(43.8f, 0f, 0f);
                switchMusic.transform.localPosition = startPosMusic;
                switchMusic.GetComponent<Image>().color = switchOff;
            }
            if (currentStateSounds.GetHashCode() == 1)
            {
                PlayerPrefs.SetInt("SoundState", currentStateSounds.GetHashCode()); //1, включен
                startPosSounds = new Vector3(43.8f, 0f, 0f);
                endPosSounds = new Vector3(-43.8f, 0f, 0f);
                switchSounds.transform.localPosition = startPosSounds;
                switchSounds.GetComponent<Image>().color = switchOn;
            }
            else
            {
                PlayerPrefs.SetInt("SoundState", currentStateSounds.GetHashCode()); //0, выключен
                startPosSounds = new Vector3(-43.8f, 0f, 0f);
                endPosSounds = new Vector3(43.8f, 0f, 0f);
                switchSounds.transform.localPosition = startPosSounds;
                switchSounds.GetComponent<Image>().color = switchOff;
            }
        }
    }
    //Действия по нажатию на свитч (нужно сделать все в одной функции) 
    // НИФИГА НЕ СМОГ СДЕЛАТЬ:((((
    public void Click(string nameButton)
    {
        if (nameButton == "Music")
        {
            if (currentStateMusic) // если включена музыка
            {
               
                currentStateMusic = false; // выключить
                PlayerPrefs.SetInt("MusicState", currentStateMusic.GetHashCode());
                startPosMusic = new Vector3(43.8f, 0f, 0f);
                endPosMusic = new Vector3(-43.8f, 0f, 0f);
                switchMusic.transform.localPosition = startPosMusic;
                switchMusic.GetComponent<Image>().color = switchOff;
                clickMusic = true;
                //передача параметров для перемещения маркера
            }
            else
            {
                
                currentStateMusic = true;
                PlayerPrefs.SetInt("MusicState", currentStateMusic.GetHashCode());
                startPosMusic = new Vector3(-43.8f, 0f, 0f);
                endPosMusic = new Vector3(43.8f, 0f, 0f);
                switchMusic.transform.localPosition = startPosMusic;
                switchMusic.GetComponent<Image>().color = switchOn;
                clickMusic = true;
                //передача параметров для перемещения маркера
            }
        }
        else if (nameButton == "Sounds")
        {
            if (currentStateSounds) //если включены звуки
            {
                clickSounds = true;
                currentStateSounds = false; // выключить
                PlayerPrefs.SetInt("SoundState", currentStateSounds.GetHashCode());
                startPosSounds = new Vector3(-43.8f, 0f, 0f);
                endPosSounds = new Vector3(43.8f, 0f, 0f);
                switchSounds.transform.localPosition = startPosSounds;
                switchSounds.GetComponent<Image>().color = switchOff;
                //передача параметров для перемещения маркера
            }
            else
            {
                clickSounds = true;
                currentStateSounds = true;
                PlayerPrefs.SetInt("SoundState", currentStateSounds.GetHashCode());
                startPosSounds = new Vector3(43.8f, 0f, 0f);
                endPosSounds = new Vector3(-43.8f, 0f, 0f);
                switchSounds.transform.localPosition = startPosSounds;
                switchSounds.GetComponent<Image>().color = switchOn;
                //передача параметров
            }
        }
    }
    //Само перемещение объекта
    private void Update()
    {
        if (clickMusic)
        {
            Move(switchMusic, startPosMusic, endPosMusic);
        }
    }
    private void Move(GameObject _transAnim, Vector3 _startPos, Vector3 _endPos)
    {
        currentTime += time;
        if (currentDist >= distance)
        {
            clickMusic = false;
            StopCoroutine("animationMove");

        }
        currentDist = 400f * currentTime;
        float Perc = currentDist / distance;
        _transAnim.transform.localPosition = Vector3.Lerp(_startPos, _endPos, Perc);
    }

        //while (clickMusic || clickSounds)
        //{
        //    yield return new WaitForSeconds(time);
        //    currentTime += time;
        //    if (currentDist >= distance)
        //    {
        //        clickMusic = false;
        //        clickSounds = false;
        //        StopCoroutine("animationMove");              
        //    }
        //    currentDist = 400f * currentTime;
        //    float Perc = currentDist / distance;
        //    _transAnim.transform.localPosition = Vector3.Lerp(_startPos, _endPos, Perc);
        //}
    
    /* Нормальный код */
}

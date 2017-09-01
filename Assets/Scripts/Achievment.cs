using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievment : MonoBehaviour {

    // Овердохера переменных
    public Sprite Gradient_store, Gradient_achievment, Gradient_settings; //Градиенты шапок для окон, шапка одна, а окон много)
    public GameObject Header; // шапка
    public Color yes; // сложные цветовые решения,
    public Color not; // на которые я сам не знаю ответа
    public Sprite _received; // зеленый галочка
    public Sprite _N_received; // серый крестик
    public GameObject Store; // окно магаза
    public GameObject Achiev; // окно достижений и задач
    public GameObject _settings; // окно достижений и задач
    public Achievment_sys[] achievment; // менеджер по задачам
    public Sprite BuyBtn_green, BuyBtn_gray; // это кнопки "купить"/"не купить"
    public Store_manager[] _store; // менеджер магазина
    public Game g;
    public Text expCount;

    ////////////////////////////////////////////////
    ///                                          ///
    ///                                          ///
    ///                                          /// 
    ///          НЕ МЕНЯЙ ЦИФРЫ В ДЕТЯХ          /// 
    ///                                          ///
    ///                                          ///
    ///                                          /// 
    ////////////////////////////////////////////////
    public void Settings() // вызываем по кнопке магазина внизу 
    {
        _settings.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Gradient_settings;
        _settings.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Settings_btn/Icon_Bg/Icon").GetComponent<Image>().sprite; // тырим иконку
        Header.SetActive(false);     
        Achiev.SetActive(false); // отключаем панель ачивок
        Store.SetActive(false);
        _settings.SetActive(true);// подрубаем панель магаза      

    }

    public void Store_set() // вызываем по кнопке магазина внизу 
    {
        Header.SetActive(true);
        Header.GetComponent<Image>().sprite = Gradient_store; // задаем шапку
        Header.transform.GetChild(1).GetComponent<Text>().text = "МАГАЗИН"; // левый текст
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Store_btn/Icon_Bg/Icon").GetComponent<Image>().sprite; // тырим иконку
        Achiev.SetActive(false); // отключаем панель ачивок
        Store.SetActive(true); // подрубаем панель магаза
        _settings.SetActive(false);

        for (int i = 0; i < _store.Length; i++) // чекаем детей
        {
            Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>().icon.sprite = _store[i].icon; // устанавливаем иконку
            Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>()._header.text = _store[i]._header.ToUpper(); // заголовок в верхнем регистре
            Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>()._description.text = _store[i]._description.ToUpper(); // описание в верхнем регистре
            Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>().isReal = _store[i].isReal; // передаем переменную, отвечающую за реальные бабосики
            Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>()._priceGame = _store[i].priceGame; // передаем игровую цену
            Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>()._priceReal = _store[i].priceReal; // передаем цену за рублики
            if (_store[i].isBought) // если товар куплен
            {
                Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>().isBought = _store[i].isBought; // передаем параметр покупки true
                Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>().Btn.sprite = BuyBtn_gray; // меняем кнопку на серую
                Store.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(6).GetChild(0).GetComponent<Text>().text = "КУПЛЕНО"; // пишем что-то              
            }
            else
            {
                Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>().isBought = _store[i].isBought; // все то же самое, но наоборот
                Store.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Store_pref>().Btn.sprite = BuyBtn_green;
                Store.transform.GetChild(0).GetChild(0).GetChild(i).GetChild(6).GetChild(0).GetComponent<Text>().text = "КУПИТЬ";                 
            }

        }
    }
    //АНАЛОГИЧНО ВНИЗУ ДЛЯ АЧИВОК
    public void Achievment_set()
    {
        Header.SetActive(true);
        Store.SetActive(false);
        Achiev.SetActive(true);
        _settings.SetActive(false);
        Header.GetComponent<Image>().sprite = Gradient_achievment; 
        Header.transform.GetChild(1).GetComponent<Text>().text = "ЗАДАНИЯ";
        
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Achievment_btn/Icon_Bg/Icon").GetComponent<Image>().sprite;
       // Parent_ach.GetComponent<ScrollRect>().content = 
       
        for (int i = 0; i < achievment.Length; i++)
        {
            Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().icon.sprite = achievment[i].icon;// устанавливаем иконку
            Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>()._header.text = achievment[i]._header.ToUpper();// заголовок в верхнем регистре
            Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>()._description.text = achievment[i]._description.ToUpper();// описание в верхнем регистре
            Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>()._award = achievment[i].award; //передаем награду в префаб
            if (achievment[i].get)
            {
                Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get; // передаем параметр ПОЛУЧЕНО true 
                Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.sprite = _received; // устанавливаем спрайт галочки
                Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.color = yes; // делаем ее зеленой
            }
            else
            {
                Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get;
                Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.sprite = _N_received; // устанавливаем спрайт крестика
                Achiev.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Ach_pref>().check.color = not; // делаем его серым
            }

        }

        expCount.text = g.exp.ToString();
    }

    //Открытие достижения
    public void unlockAch(int id)
    {
        achievment[id].get = true;
        g.exp += achievment[id].award;
        g.push.transform.Find("Icon").GetComponent<Image>().sprite = achievment[id].icon;
        g.push.transform.Find("Header").GetComponent<Text>().text = achievment[id]._header;
        g.push.transform.Find("Description").GetComponent<Text>().text = "Поздравляем! Награда получена!";
        g.push.GetComponent<Animator>().SetTrigger("isShown");
    }

}

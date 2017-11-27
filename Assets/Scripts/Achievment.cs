using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Achievment : MonoBehaviour
{

    // Овердохера переменных
    public Sprite Gradient_store, Gradient_achievment, Gradient_settings, Gradient_casino; //Градиенты шапок для окон, шапка одна, а окон много)
    public GameObject Header; // шапка
    public Color yes; // сложные цветовые решения,
    public Color not; // на которые я сам не знаю ответа
    public Sprite _received; // зеленый галочка
    public Sprite _N_received; // серый крестик
    public GameObject Menu_down;

    public GameObject Casino; // окно казино
    public GameObject Store; // окно магаза
    public GameObject Achiev; // окно достижений и задач
    public GameObject _settings; // окно настроек
    public Achievment_sys[] achievment; // менеджер по задачам
    public Sprite BuyBtn_green, BuyBtn_gray, Btn_violet; // это кнопки "купить"/"не купить"
    public Store_manager[] _store; // менеджер магазина
    public Game g;
    public Text expCount;
    public Text reward; //награда
    public Text rewardCost; //стоимость награды
    public GameObject _currency;
    public bool _curS;
    AnimatorRecorderMode recorderMode;
    public GameObject m;
    Transform achiev, set, store;
    ////////////////////////////////////////////////
    ///                                          ///
    ///                                          ///
    ///                                          /// 
    ///          НЕ МЕНЯЙ ЦИФРЫ В ДЕТЯХ          /// 
    ///                                          ///
    ///                                          ///
    ///                                          /// 
    ////////////////////////////////////////////////
    private void Start()
    {
        achiev = Achiev.transform.GetChild(0);
        set = _settings.transform.GetChild(0);
        store = Store.transform.GetChild(0);
    }


    void AnimationComplete()
    {
        _currency.SetActive(false);
    }


    
    //Ниже пойдет вызов кучи анимаций
    //Все завязано на Menu_down. Он управляет вызовом всех панелек
    public void Settings() // вызываем по кнопке магазина внизу 
    {
        gameObject.GetComponent<CoinFlip>().SetDefault();
        _settings.SetActive(true);
        Header.SetActive(false);
        Casino.SetActive(false);
        Store.SetActive(false);
        Achiev.SetActive(false);
        //ЛОКАЛИЗАЦИЯ
        set.GetChild(0).GetChild(1).GetComponent<Text>().text = LangSystem.lng.panelName[3];
        set.GetChild(1).GetChild(1).GetComponent<Text>().text = LangSystem.lng.settingsParam[0];//перевод звуков
        set.GetChild(2).GetChild(1).GetComponent<Text>().text = LangSystem.lng.settingsParam[1];//перевод музыки
        set.GetChild(3).GetChild(1).GetComponent<Text>().text = LangSystem.lng.settingsParam[2];//перевод языка
        set.GetChild(4).GetChild(1).GetComponent<Text>().text = LangSystem.lng.windows[3]; // Оценить игру
        set.GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text = LangSystem.lng.settingsParam[3]; // Мы в вк



        set.GetChild(0).GetComponent<Image>().sprite = Gradient_settings;
        set.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Settings_btn/Icon_Bg/Icon").GetComponent<Image>().sprite; // тырим иконку

    }

    public void Store_set() // вызываем по кнопке магазина внизу 
    {
        gameObject.GetComponent<CoinFlip>().SetDefault();
        Header.SetActive(true);
        Casino.SetActive(false);
        Store.SetActive(true);
        Achiev.SetActive(false);
        _settings.SetActive(false);
        Header.GetComponent<Image>().sprite = Gradient_store; // задаем шапку
        Header.transform.GetChild(1).GetComponent<Text>().text = LangSystem.lng.panelName[1];//"МАГАЗИН"; // левый текст
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Store_btn/Icon_Bg/Icon").GetComponent<Image>().sprite; // тырим иконку


        for (int i = 0; i < _store.Length; i++) // чекаем детей
        {
            store.GetChild(0).GetChild(i).GetComponent<Store_pref>().icon.sprite = _store[i].icon; // устанавливаем иконку
            store.GetChild(0).GetChild(i).GetComponent<Store_pref>()._header.text = LangSystem.lng.storeArrHeader[i]; //_store[i]._header.ToUpper(); // заголовок в верхнем регистре
            store.GetChild(0).GetChild(i).GetComponent<Store_pref>()._description.text = LangSystem.lng.storeArrDescription[i]; //_store[i]._description.ToUpper(); // описание в верхнем регистре
            store.GetChild(0).GetChild(i).GetComponent<Store_pref>().isReal = _store[i].isReal; // передаем переменную, отвечающую за реальные бабосики
            store.GetChild(0).GetChild(i).GetComponent<Store_pref>()._priceGame = _store[i].priceGame; // передаем игровую цену
            store.GetChild(0).GetChild(i).GetComponent<Store_pref>()._priceReal = _store[i].priceReal; // передаем цену за рублики
            store.GetChild(0).GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[4];
            if (_store[i].isBought) // если товар куплен
            {
                store.GetChild(0).GetChild(i).GetComponent<Store_pref>().isBought = _store[i].isBought; // передаем параметр покупки true
                store.GetChild(0).GetChild(i).GetComponent<Store_pref>().Btn.sprite = BuyBtn_gray; // меняем кнопку на серую
                store.GetChild(0).GetChild(i).transform.Find("BuyBtn").GetComponent<Button>().interactable = false;// меняем кнопку на серую
                store.GetChild(0).GetChild(i).GetChild(6).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[1];//"КУПЛЕНО"; // пишем что-то              
            }
            else
            {
                store.GetChild(0).GetChild(i).GetComponent<Store_pref>().isBought = _store[i].isBought; // все то же самое, но наоборот
                store.GetChild(0).GetChild(i).GetComponent<Store_pref>().Btn.sprite = BuyBtn_green;
                store.GetChild(0).GetChild(i).GetChild(6).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[0];//"КУПИТЬ";
            }

        }
        store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Button>().interactable = _store[2].isBought;
        // store.GetChild(0).GetChild(2).GetChild(8).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[3];
        if (_store[2].isBought)
        {
            store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Image>().sprite = Btn_violet;
            store.GetChild(0).GetChild(2).GetChild(8).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[3];
        }
        else
        {
            store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Image>().color = new Color32(182, 182, 182, 255);
            store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Image>().sprite = BuyBtn_gray;
            store.GetChild(0).GetChild(2).GetChild(8).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[2];
        }
    }
    //АНАЛОГИЧНО ВНИЗУ ДЛЯ АЧИВОК
    public void Achievment_set()
    {
        gameObject.GetComponent<CoinFlip>().SetDefault();
        Header.SetActive(true);
        Casino.SetActive(false);
        Store.SetActive(false);
        Achiev.SetActive(true);
        _settings.SetActive(false);

        Header.GetComponent<Image>().sprite = Gradient_achievment;
        Header.transform.GetChild(1).GetComponent<Text>().text = LangSystem.lng.panelName[2]; //"ЗАДАНИЯ";

        //  CurrencyOff();
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Achievment_btn/Icon_Bg/Icon").GetComponent<Image>().sprite;
        //ПЕРЕВОДЫ ПАНЕЛИ АЧИВОК
        achiev.GetChild(0).GetChild(1).GetComponent<Text>().text = LangSystem.lng.achievementText[0]; // ТОП-100 лучших
        achiev.GetChild(1).GetChild(0).GetComponent<Text>().text = LangSystem.lng.achievementText[1]; // описание
        achiev.GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.achievementText[2]; // ваши очки
        achiev.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.achievementText[3]; //след.награда
        achiev.GetChild(2).GetChild(4).GetComponent<Text>().text = LangSystem.lng.achievementText[4]; // требуется
        for (int i = 0; i < achievment.Length; i++)
        {
            achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().icon.sprite = achievment[i].icon;// устанавливаем иконку achArrHeader
            achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>()._header.text = LangSystem.lng.achArrHeader[i].ToUpper(); //achievment[i]._header.ToUpper();// заголовок в верхнем регистре
            achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>()._description.text = LangSystem.lng.achArrDescrip[i].ToUpper();//achievment[i]._description.ToUpper();// описание в верхнем регистре
            achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>()._award = achievment[i].award; //передаем награду в префаб
            achiev.GetChild(3).GetChild(i).GetChild(4).GetComponent<Text>().text = LangSystem.lng.achievementText[5]; // награда
            if (achievment[i].get)
            {
                achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get; // передаем параметр ПОЛУЧЕНО true 
                achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().check.sprite = _received; // устанавливаем спрайт галочки
                achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().check.color = yes; // делаем ее зеленой
            }
            else
            {
                achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().get = achievment[i].get;
                achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().check.sprite = _N_received; // устанавливаем спрайт крестика
                achiev.GetChild(3).GetChild(i).GetComponent<Ach_pref>().check.color = not; // делаем его серым
            }
        }
        expCount.text = g.exp.ToString();
        reward.text = "$ " + g.reward.ToString();
        rewardCost.text = g.rewardCost.ToString();
    }

    public void CasinoPlay()
    {
        Header.SetActive(true);
        Casino.SetActive(true);
        Store.SetActive(false);
        Achiev.SetActive(false);
        _settings.SetActive(false);
        Header.GetComponent<Image>().sprite = Gradient_casino;
        Header.transform.GetChild(1).GetComponent<Text>().text = LangSystem.lng.panelName[0];//"Coin Flip";       
        Header.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameObject.Find("/Canvas/Panel_Down/Jackpot/Icon_Bg/Icon").GetComponent<Image>().sprite;
        Casino.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = LangSystem.lng.casino[0];
    }
    //Открытие достижения
    public void unlockAch(int id)
    {
        achievment[id].get = true;
        g.exp += achievment[id].award;
        g.push.transform.Find("Icon").GetComponent<Image>().sprite = achievment[id].icon;
        g.push.transform.Find("Header").GetComponent<Text>().text = LangSystem.lng.achArrHeader[id];// achievment[id]._header;//
        g.push.transform.Find("Description").GetComponent<Text>().text = LangSystem.lng.push[3]; // "Поздравляем! Награда получена!";
        g.push.GetComponent<Animator>().SetTrigger("isShown");
        if (g.exp >= g.rewardCost)
        {
            g.money = g.money + g.reward;
            g.reward = (int)(g.reward * g.rewardD);
            g.rewardCost = (int)(g.rewardCost * g.rewardCostD);
        }
    }

    //Покупка автообменника
    public void buyAutoExchanger()
    {
        _store[2].isBought = true;
        g.money -= _store[2].priceGame;
        g.isAutoExchangerOn = true;
        store.GetChild(0).GetChild(2).transform.Find("BuyBtn").GetComponent<Button>().interactable = false;
        store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Button>().interactable = true;
        store.GetChild(0).GetChild(2).transform.Find("OffBtn").GetComponent<Image>().sprite = Btn_violet;
        store.GetChild(0).GetChild(2).GetChild(8).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[3];
        store.GetChild(0).GetChild(2).GetComponent<Store_pref>().Btn.sprite = BuyBtn_gray; // меняем кнопку на серую
    }

    //Включение-выключение автообменника
    public void switchAutoExchanger()
    {
        g.isAutoExchangerOn = !g.isAutoExchangerOn;
        if (g.isAutoExchangerOn)
        {
            store.GetChild(0).GetChild(2).GetChild(8).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[3];//"Выключить";
            store.GetChild(0).GetChild(2).GetChild(8).GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        }
        else
        {
            store.GetChild(0).GetChild(2).GetChild(8).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[2];
            store.GetChild(0).GetChild(2).GetChild(8).GetComponent<Image>().color = new Color32(182, 182, 182, 255); //"Включить"; //OffBtn
        }
    }

    //Покупка бустера по доходности
    public void buyProfitBooster()
    {
        _store[0].isBought = true;
        g.money -= _store[0].priceGame;
        g.isProfitBoosterOn = true;
        Store.transform.GetChild(0).GetChild(0).GetChild(0).transform.Find("BuyBtn").GetComponent<Button>().interactable = false;
        Store.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Store_pref>().Btn.sprite = BuyBtn_gray; // меняем кнопку на серую
        Store.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[1];//"КУПЛЕНО";
    }

    //Покупка бустера по времени
    public void buyTimeBooster()
    {
        _store[1].isBought = true;
        g.money -= _store[1].priceGame;
        g.isTimeBoosterOn = true;
        Store.transform.GetChild(0).GetChild(0).GetChild(1).transform.Find("BuyBtn").GetComponent<Button>().interactable = false;
        Store.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Store_pref>().Btn.sprite = BuyBtn_gray; // меняем кнопку на серую
        Store.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(6).GetChild(0).GetComponent<Text>().text = LangSystem.lng.storeDetails[1];// "КУПЛЕНО";
    }

    public void buy8000()
    {
        g.money += 8000;
    }

}

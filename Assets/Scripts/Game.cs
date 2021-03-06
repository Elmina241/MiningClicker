﻿using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.IO;

public class Game : MonoBehaviour
{
    public int music = 0;
    public AudioSource audM;
    public GameObject musicSwitch;
   
    public const string leaderboard = "CgkI-9fq-ZEaEAIQAQ";

    public Sprite[] background;
    Rewards rew;
    public Learning learn;

    public Text moneyText; //Деньги в $
    public float money = 0;
    public int clickCounter = 0; // Счетчик кликов по кнопке
    public float sumMoney = 0; // Доход за всё время
    public int farmCount = 0; //счётчик работающих ферм
    public int partCount = 0; //счётчик купленных частей
    public int exp = 0;

    public int reward = 100;
    public int rewardCost = 100;
    public float rewardD = 1.5f;
    public float rewardCostD = 1.7f;

    public GameObject improvementWin; //окно улучшений и запчастей
    public GameObject rateWin; //окно оценки приложения
    public int compId; // ID компьютера, чьё окно улучшений сейчас отображается 
    public GameObject push; //панель уведомлений
    public Button autoMinerButton; //кнопка покупки автомайнера
    public Button offMinerButton; //кнопка покупки оффлайнового майнера
    public Button offMinerBonusButton; //кнопка покупки улучшения оффрайнового манера
    public Button autoRepairButton; //кнопка покупки автопочинки компонентов
    public Button upgradeTimeButton; //кнопка покупки улучшения времени
    public Button upgradeProfitButton; //кнопка покупки улучшения дохода
    public Text upgradePointsText; //Текст очки улучшения
    public Text levelText; //Текст номер уровня
    public Text nameText; //Текст названия компьютера
    public Text pribyl; //Прибыль на данный момент
    public Text time; //Время заполнения автомайнером 
    public Text sr_pr; //средняя прибыль
    public Text prBarText; //количество опыта 
    public Image fill; //заполнение прогрессбара
    public Sprite[] partImages; //картинки компонентов
    public bool isAutoExchangerOn = false; //Работает ли автообменник
    public bool isProfitBoosterOn = false; //Работает ли бустер доходности
    public bool isTimeBoosterOn = false; // Работает ли бустер времени
    public GameObject nightResult; // Окошко заработанных денег за ночь

    Save sv = new Save();

    public Part[] parts;
    public typeOfPart[] typesOfParts;
    public Currency[] currencies;

    

    [Header("Имена компонентов")]
    [Space(4)]
    public string[] NameOfCPU;

    public string[] NameOfGPU;

    public string[] NameOfMotherboard;

    public string[] NameOfPower;

    public string[] NameOfSSD;

    public string[] NameOfCooling;

    public string[] NameOfRAM;

    [Space(4)]
    [Header("Цены компонентов")]

    public float[] PriceOfCPU; // в $

    public float[] PriceOfGPU;

    public float[] PriceOfMotherboard;

    public float[] PriceOfPower;

    public float[] PriceOfSSD;

    public float[] PriceOfCooling;

    public float[] PriceOfRAM;

    public GameObject ArrayMiners;

    public GameObject RateMe;

    public void Translator()
    {
        int count = ArrayMiners.transform.childCount;        
        for (int i = 0; i < count; i++)
        {
            ArrayMiners.transform.GetChild(i).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.game[0];
            ArrayMiners.transform.GetChild(i).GetComponent<Computer>().updateUp();
            //ArrayMiners.transform.GetChild(i).GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>().text = LangSystem.lng.game[1];
        }
        nightResult.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = LangSystem.lng.windows[0];
        nightResult.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = LangSystem.lng.windows[1];

        RateMe.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = LangSystem.lng.windows[2];
        RateMe.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.windows[3];
        RateMe.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = LangSystem.lng.windows[4];


    }

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        //Music();
        ArrayMiners = GameObject.Find("/Canvas/BG/ScrollRect/Miners");
        rew = gameObject.GetComponent<Rewards>();
        learn = gameObject.GetComponent<Learning>();
        money = 20000f;

        NameOfCooling = new string[7] { "Arctic Cooling Alpine 64 PLUS", "Xilence M403", "CoolerMaster Hyper 103",
        "ID-Cooling SE-204K", "Scythe Mugen 5",  "Thermaltake Pacific RL140", "CoolerMaster Master V8" };

        NameOfCPU = new string[7] { "Intel® Pentium G4560 3500 GHz", "Intel® Core™i3-7100 3900 GHz", "Intel® Core™i5-7500 3400 GHz",
        "Intel® Core™i5-7500 3400 GHz", "Intel® Core™i7-6900K 3200 GHz", "Intel® Core™i7-6700K 4000 GHz", "Intel® Xeon E5-2650v4 2200 GHz"};

        NameOfGPU = new string[7] { "NVidia GF GTX 1050 Ti", "NVidia GF GTX 1050 Ti", "Palit GF GTX 1060 STORMX",
        "Asus GF GTX 1070 TURBO", "NVidia GF GTX 1080 Ti" , "NVidia GF GTX 1080 Ti", "NVidia GF GTX 1080 Ti" };

        NameOfMotherboard = new string[7] { "ASUS H110M-A/M.2", "ASRock H270 PRO4", "MSI B250M PRO-VD", "MSI H270 GAMING M3",
        "MSI X99A SLI PLUS", "MSI Z170M MORTAR", "ASUS Z10PE-D16 WS"};

        NameOfPower = new string[7] { "Thermaltake TR2 S 350W", "Cougar A 400W", "Zalman TX 500W", "Seasonic Prime 650W",
        "Corsair AX860 860W", "CoolerMaster V1000 1000W", "Corsair AXi 1200W" };

        NameOfSSD = new string[7] { "SiliconPower Slim S60 60 Gb", "SiliconPower Slim S70 120 Gb", "Kingston HyperX SAVAGE 240 Gb",
        "Crucial Micron 5100 MAX 480 Gb", "Transcend SSD370s 512 Gb", "Intel 750 Series 800 Gb", "Corsair Neutron XT 960 Gb" };

        NameOfCooling = new string[7] { "Arctic Cooling Alpine 64 PLUS", "Xilence M403", "CoolerMaster Hyper 103",
        "ID-Cooling SE-204K", "Scythe Mugen 5",  "Thermaltake Pacific RL140", "CoolerMaster Master V8" };

        NameOfRAM = new string[7] { "Kingston ValueRAM 2 Gb", "Ballistix Elite 4 Gb", "AData XPG V2 4 Gb",
        "Kingston HyperX FURY 8 Gb", "Ballistix Tactical 32 Gb", "Kingston HyperX Predator 32 Gb", "Corsair Vengeance LPX 64 Gb" };

        PriceOfCPU = new float[7] { 94, 146, 241, 241, 1056, 396, 1370 }; // в $

        PriceOfGPU = new float[7] { 157, 157, 278, 488, 844, 844, 84 };

        PriceOfMotherboard = new float[7] { 58, 98, 53, 112, 177, 91, 462 };

        PriceOfPower = new float[7] { 39, 55, 64, 272, 206, 260, 456 };

        PriceOfSSD = new float[7] { 63, 88, 158, 412, 282, 990, 660 };

        PriceOfCooling = new float[7] { 16, 25, 34, 52, 66, 344, 129 };

        PriceOfRAM = new float[7] { 18, 56, 92, 132, 332, 405, 905 };

        /*Объявление валют*/
        currencies = new Currency[1];
        currencies[0] = new Currency("BTC", 30000f);

        /*Объявление типов компонентов. ID в массиве должно совпадать с Id типа!!!!! НЕ МЕНЯТЬ ID GPU!!!!!*/
        typesOfParts = new typeOfPart[7];
        typesOfParts[0] = new typeOfPart(0, "CPU", partImages[0]);
        typesOfParts[1] = new typeOfPart(1, "GPU", partImages[1]);
        typesOfParts[2] = new typeOfPart(2, "Power", partImages[2]);
        typesOfParts[3] = new typeOfPart(3, "Motherboard", partImages[3]);
        typesOfParts[4] = new typeOfPart(4, "SSD", partImages[4]);
        typesOfParts[5] = new typeOfPart(5, "Cooling", partImages[5]);
        typesOfParts[6] = new typeOfPart(6, "RAM", partImages[6]);

        /*Объявление компонентов*/
        /*parts = new Part[8];
        parts[1] = new Part(1, "Intel® Core™i3-2100 3.1 GHz", typesOfParts[0], 8, 15, 215, 167);
        parts[2] = new Part(2, "Hipro 450W", typesOfParts[2], 25, 35, 847, 540);
        parts[0] = new Part(0, "AMD Radeon RX 470", typesOfParts[1], 10, 25, 2, 10);
        parts[3] = new Part(3, "AMD Radeon RX 470", typesOfParts[1], 10, 25, 2, 10);
        parts[4] = new Part(4, "MSI H81M-E33", typesOfParts[3], 10, 25, 2, 10);
        parts[5] = new Part(5, "JRam 4 GB", typesOfParts[6], 10, 25, 2, 10);
        parts[6] = new Part(6, "SiliconPower Slim S60 ", typesOfParts[4], 10, 25, 2, 10);
        parts[7] = new Part(7, "Xilence A402", typesOfParts[5], 10, 25, 2, 10);*/
        parts = new Part[49];
        int partId = 0;
        for (int i = 0; i < 7; i++)
        {
            parts[partId] = new Part(partId, NameOfCPU[i], typesOfParts[0], 12, 25, PriceOfCPU[i], (PriceOfCPU[i] * 0.6f));
            partId++;
            parts[partId] = new Part(partId, NameOfGPU[i], typesOfParts[1], 12, 25, PriceOfGPU[i], (PriceOfGPU[i] * 0.6f));
            partId++;
            parts[partId] = new Part(partId, NameOfPower[i], typesOfParts[2], 12, 25, PriceOfPower[i], (PriceOfPower[i] * 0.6f));
            partId++;
            parts[partId] = new Part(partId, NameOfMotherboard[i], typesOfParts[3], 12, 25, PriceOfMotherboard[i], (PriceOfMotherboard[i] * 0.6f));
            partId++;
            parts[partId] = new Part(partId, NameOfSSD[i], typesOfParts[4], 12, 25, PriceOfSSD[i], (PriceOfSSD[i] * 0.6f));
            partId++;
            parts[partId] = new Part(partId, NameOfCooling[i], typesOfParts[5], 12, 25, PriceOfCooling[i], (PriceOfCooling[i] * 0.6f));
            partId++;
            parts[partId] = new Part(partId, NameOfRAM[i], typesOfParts[6], 12, 25, PriceOfRAM[i], (PriceOfRAM[i] * 0.6f));
            partId++;
        }

        

        if (PlayerPrefs.HasKey("unitySV"))
        {
            sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("unitySV"));
            money = sv.money;
            learn.stage = sv.learningStage;
            sumMoney = sv.sumMoney;
            clickCounter = sv.clickCounter;
            reward = sv.reward;
            rewardCost = sv.rewardCost;
            GameObject miners = GameObject.FindWithTag("Miners").gameObject;

            int resSize = sv.currency.Length;

            farmCount = sv.farmCount;
            partCount = sv.partCount;
            exp = sv.exp;

            ////System.DateTime dt = new System.DateTime(sv.date[0], sv.date[1], sv.date[2], sv.date[3], sv.date[4], sv.date[5]);
            //rew.GetTime();
            //ulong dt = rew.unixTime;



            System.DateTime dt = new System.DateTime(sv.date[0], sv.date[1], sv.date[2], sv.date[3], sv.date[4], sv.date[5]);
            System.TimeSpan ts = System.DateTime.Now - dt;

            int i = 0;
            float nightMoney = 0;
            bool isNight = false;

            while (i < resSize)
            {
                Computer cur = miners.transform.GetChild(i).GetComponent<Computer>();
                cur.cur = currencies[0];
                cur.currency = sv.currency[i];
                cur.isReady = sv.isReady[i];
                cur.bonus = sv.bonus[i];
                cur.bonusCost = sv.bonusCost[i];
                cur.progressCounter = sv.progressCounter[i];
                cur.progressCounter1 = sv.progressCounter1[i];
                cur.progressCounter2 = sv.progressCounter2[i];
                cur.maxClick = sv.maxClick[i];

                cur.isBoughtOff = sv.isBoughtOff[i];
                cur.offProfit = sv.offProfit[i];
                cur.offProfitBonus = sv.offProfitBonus[i];

                cur.upgradeCost = sv.upgradeCost[i];
                cur.upgradePoints = sv.upgradePoints[i];
                cur.timeUpgrade = sv.timeUpgrade[i];
                cur.profiteUpgrade = sv.profiteUpgrade[i];

                if (cur.isBoughtOff && cur.isReady)
                {
                    //cur.currency = cur.currency + cur.offProfit * ts.Seconds;
                    nightMoney = nightMoney + cur.offProfit * ts.Seconds * cur.cur.getExchRate();
                    isNight = true;
                }

                cur.level = sv.level[i];
                cur.autoMiner.isBoughtAuto = sv.isBoughtAuto[i];
                cur.isBoughtAutoRepair = sv.isBoughtAutoRepair[i];
                cur.autoMiner.autoTime = sv.autoTime[i];
                cur.autoMiner.autoProfit = sv.autoProfit[i];
                cur.autoMiner.timeBonus = sv.timeBonus[i];
                cur.isResearched = true;
                if (cur.compParts.Length != 0)
                {
                    PartsOfComputer[] p = JsonHelper.FromJson<PartsOfComputer>(sv.partsOfComp[i]);
                    for (int j = 0; j < cur.compParts.Length; j++)
                    {
                        cur.compParts[j] = p[j];
                    }
                }
                i++;
            }
            i = 0;
            while (i < currencies.Length)
            {
                currencies[i].sum = sv.sumCur[i];
                i++;
            }
            Achievment_sys[] a = JsonHelper.FromJson<Achievment_sys>(sv.achievments);
            for (int j = 0; j < gameObject.GetComponent<Achievment>().achievment.Length; j++)
            {
                gameObject.GetComponent<Achievment>().achievment[j] = a[j];
            }
            for (int j = 0; j < gameObject.GetComponent<Achievment>()._store.Length; j++)
            {
                gameObject.GetComponent<Achievment>()._store[j].isBought = sv.isBought[j];
            }
            isAutoExchangerOn = sv.isAutoExchangerOn;
            money = money + nightMoney;
            if (isNight) showNightMining(nightMoney);
        }
        autoMinerButton = improvementWin.transform.Find("Background/AutoMiner/GameObject/BuyAuto").gameObject.GetComponent<Button>();
        upgradeTimeButton = improvementWin.transform.Find("Background/UpgradeGroup/TimeUpgrade").gameObject.GetComponent<Button>();
        upgradeProfitButton = improvementWin.transform.Find("Background/UpgradeGroup/ProfitUpgrade").gameObject.GetComponent<Button>();
        upgradePointsText = improvementWin.transform.Find("Background/PointGroup/UpgradePoints").gameObject.GetComponent<Text>();
        levelText = improvementWin.transform.Find("Background/Header/LevelText").gameObject.GetComponent<Text>();
    }

    void Start()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            // Удачно или нет?
            if (success)
            {
               // tt.text = Social.localUser.id;
               // test.text = Social.localUser.userName;
            }
        });
        Translator();
        
    }

  

    public void openLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
    public void openCloseImprovementWin()
    {
        GameObject partsContainer = improvementWin.transform.Find("Background/Parts").gameObject;
        int childs = partsContainer.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(partsContainer.transform.GetChild(i).gameObject);
        }
        improvementWin.SetActive(!improvementWin.activeSelf);
    }

    //Функция отобржаения окна ночного заработка
    public void showNightMining(float nightCurrency)
    {
        nightResult.transform.GetChild(0).GetChild(5).GetComponent<Text>().text = "$" + nightCurrency.ToString("0.#0");
        nightResult.SetActive(true);
    }

    public int getCurrent()
    {
        GameObject miners = GameObject.FindWithTag("Miners").gameObject;
        if (farmCount < 2)
        {
            int res = 0;
            while (res < miners.transform.childCount && !miners.transform.GetChild(res).GetComponent<Computer>().isReady) res++;
            return res;
        }
        else if (farmCount == 3)
        {
            return 11;
        }
        else if (miners.transform.GetChild(6).GetComponent<Computer>().isReady && miners.transform.GetChild(7).GetComponent<Computer>().isReady) return 8;
        else if (miners.transform.GetChild(5).GetComponent<Computer>().isReady && miners.transform.GetChild(7).GetComponent<Computer>().isReady) return 9;
        else if (miners.transform.GetChild(5).GetComponent<Computer>().isReady && miners.transform.GetChild(6).GetComponent<Computer>().isReady) return 10;
        else return -1;
    }

    public void saveGame()
    {
        Social.ReportScore((long)money, leaderboard, (bool success) => {
            // Удачно или нет?
        });
        sv.money = money;
        sv.farmCount = farmCount;
        sv.partCount = partCount;
        sv.sumMoney = sumMoney;
        sv.reward = reward;
        sv.rewardCost = rewardCost;
        sv.isAutoExchangerOn = isAutoExchangerOn;
        sv.learningStage = learn.stage;

        sv.date[0] = System.DateTime.Now.Year;
        sv.date[1] = System.DateTime.Now.Month;
        sv.date[2] = System.DateTime.Now.Day;
        sv.date[3] = System.DateTime.Now.Hour;
        sv.date[4] = System.DateTime.Now.Minute;
        sv.date[5] = System.DateTime.Now.Second;

        GameObject miners = GameObject.FindWithTag("Miners").gameObject;

        int resSize = 0;

        while (resSize < miners.transform.childCount && miners.transform.GetChild(resSize).GetComponent<Computer>().isResearched) resSize++;

        sv.currency = new float[resSize];
        sv.isBought = new bool[gameObject.GetComponent<Achievment>()._store.Length];
        sv.isReady = new bool[resSize];
        sv.bonus = new float[resSize];
        sv.bonusCost = new float[resSize];
        sv.maxClick = new int[resSize];
        sv.upgradeCost = new int[resSize];
        sv.upgradePoints = new int[resSize];
        sv.timeUpgrade = new float[resSize];
        sv.profiteUpgrade = new float[resSize];
        sv.progressCounter = new int[resSize];
        sv.progressCounter1 = new int[resSize];
        sv.progressCounter2 = new int[resSize];
        sv.level = new int[resSize];
        sv.isBoughtAuto = new bool[resSize];
        sv.isBoughtOff = new bool[resSize];
        sv.isBoughtAutoRepair = new bool[resSize];
        sv.autoTime = new float[resSize];
        sv.offProfit = new float[resSize];
        sv.offProfitBonus = new float[resSize];
        sv.autoProfit = new float[resSize];
        sv.timeBonus = new float[resSize];
        sv.partsOfComp = new string[resSize];
        sv.sumCur = new float[this.currencies.Length];
        sv.exp = exp;
        sv.clickCounter = clickCounter;

        int i = 0;
        while (i < resSize)
        {
            Computer cur = miners.transform.GetChild(i).GetComponent<Computer>();
            PartsOfComputer[] p = new PartsOfComputer[cur.compParts.Length];

            sv.currency[i] = cur.currency;
            sv.isReady[i] = cur.isReady;
            sv.bonus[i] = cur.bonus;
            sv.bonusCost[i] = cur.bonusCost;
            sv.progressCounter[i] = cur.progressCounter;
            sv.progressCounter1[i] = cur.progressCounter1;
            sv.progressCounter2[i] = cur.progressCounter2;
            sv.maxClick[i] = cur.maxClick;

            sv.upgradeCost[i] = cur.upgradeCost;
            sv.upgradePoints[i] = cur.upgradePoints;
            sv.timeUpgrade[i] = cur.timeUpgrade;
            sv.profiteUpgrade[i] = cur.profiteUpgrade;

            sv.isBoughtOff[i] = cur.isBoughtOff;
            sv.offProfit[i] = cur.offProfit;
            sv.offProfitBonus[i] = cur.offProfitBonus;

            sv.level[i] = cur.level;
            sv.isBoughtAuto[i] = cur.autoMiner.isBoughtAuto;
            sv.isBoughtAutoRepair[i] = cur.isBoughtAutoRepair;
            sv.autoTime[i] = cur.autoMiner.autoTime;
            sv.autoProfit[i] = cur.autoMiner.autoProfit;
            sv.timeBonus[i] = cur.autoMiner.timeBonus;

            for (int j = 0; j < cur.compParts.Length; j++)
            {
                p[j] = cur.compParts[j];
            }

            if (cur.compParts.Length != 0)
            {
                sv.partsOfComp[i] = JsonHelper.ToJson<PartsOfComputer>(p);
            }
            i++;

        }
        i = 0;
        while (i < currencies.Length)
        {
            sv.sumCur[i] = currencies[i].sum;
            i++;
        }

        Achievment_sys[] a = new Achievment_sys[gameObject.GetComponent<Achievment>().achievment.Length];
        for (int j = 0; j < gameObject.GetComponent<Achievment>().achievment.Length; j++)
        {
            a[j] = gameObject.GetComponent<Achievment>().achievment[j];
        }
        for (int j = 0; j < gameObject.GetComponent<Achievment>()._store.Length; j++)
        {
            sv.isBought[j] = gameObject.GetComponent<Achievment>()._store[j].isBought;
        }

        sv.achievments = JsonHelper.ToJson<Achievment_sys>(a);


        PlayerPrefs.SetString("unitySV", JsonUtility.ToJson(sv));
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            saveGame();
        }
    }

    private void OnApplicationQuit()
    {
        saveGame();
    }

    public void restartGame()
    {
        PlayerPrefs.DeleteAll();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Music()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            music = 1;
            PlayerPrefs.SetInt("Music", music);

            audM.mute = false;
               
            ///musicSwitch.GetComponent<Animator>().SetBool("switch", true);
            
        }
        else
        {
            music = PlayerPrefs.GetInt("Music");
            if (music == 1)
            {
                audM.mute = false;
                audM.Play();
               // musicSwitch.GetComponent<Animator>().SetBool("switch", true);
            }
            else
            {
                audM.mute = true;
               // musicSwitch.GetComponent<Animator>().SetBool("switch", false);
                audM.Stop();
            }
        }
    }

    public void getRewardForLearning()
    {
        money = money + 300;
    }

    public void SwitchM()
    {
        music = PlayerPrefs.GetInt("Music");
        if (music == 0)
        {
            music = 1;
          //  musicSwitch.GetComponent<Animator>().SetBool("switch", true);
            PlayerPrefs.SetInt("Music", music);
            audM.mute = false;
            if (!audM.playOnAwake)
            {
                audM.Play();
            }
        }
        else
        {
            music = 0;
          //  musicSwitch.GetComponent<Animator>().SetBool("switch", false);
            PlayerPrefs.SetInt("Music", music);
            audM.mute = true;
            audM.Pause();
        }
    }
}


[System.Serializable]
public class Part
{
    /*-- JSON-сериализация --*/
    public int id; // Номер компонента в списке улучшений
    public typeOfPart type; // Тип компонента
    public string partName; // Имя компонента (AMD RADEON RX480 4GB)
    public float reliabilityNew; // Вероятность поломки каждые 100 заполнений прогресс-бара. 0-10% для Новых, 0-25% для Б/У
    public float reliabilityUsed;
    public float costNew; // Цена нового компонента
    public float costUsed; // Цена Б/У компонента

    /*!-- JSON-сериализация --*/

    public Part(int id, string name, typeOfPart type, float reliabilityNew, float reliabilityUsed, float costNew, float costUsed)
    {
        this.id = id;
        partName = name;
        this.type = type;
        this.reliabilityNew = reliabilityNew;
        this.reliabilityUsed = reliabilityUsed;
        this.costNew = costNew;
        this.costUsed = costUsed;

    }
}

[System.Serializable]
public class typeOfPart
{
    public int id; // Номер типа
    public string name; // Имя типа
    public Sprite image; // картиночка
    public typeOfPart(int id, string name, Sprite image)
    {
        this.id = id;
        this.name = name;
        this.image = image;
    }
}


[System.Serializable]
public class Save
{
    public float money;
    public float[] currency;
    public bool[] isReady;
    public bool[] isBought;
    public int[] progressCounter;
    public int[] progressCounter1;
    public int[] progressCounter2;
    public float[] bonus;
    public float[] bonusCost;
    public int[] maxClick;
    public int exp;
    public int[] upgradeCost;
    public int[] upgradePoints;
    public float[] timeUpgrade;
    public float[] profiteUpgrade;
    public int clickCounter;
    public int[] level;
    public bool[] isBoughtAuto;
    public bool[] isBoughtAutoRepair;
    public bool[] isBoughtOff;
    public float[] autoTime;
    public float[] offProfit;
    public float[] offProfitBonus;
    public float[] autoProfit;
    public float[] timeBonus;
    public float sumMoney;
    public int farmCount;
    public int partCount;
    public int reward;
    public int rewardCost;
    public float[] sumCur;
    public bool isAutoExchangerOn;
    public int learningStage;

    public string[] partsOfComp;
    public string achievments;

    public int[] date = new int[6];

}

[System.Serializable]
public class PartsOfComputer
{
    /*-- JSON-сериализация --*/
    public int id; // Номер компонента в списке улучшений
    public bool isBought; // Куплен ли компонент?
    public bool isBroken; // Сломан ли компонент?
    public float curReliability; // Вероятность поломки компонента в зависимости от новизны
    /*!-- JSON-сериализация --*/
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

//Класс Криптовалюты
[System.Serializable]
public class Currency
{
    private string name;
    private float exchRate;
    public float sum; //общее число заработанной валюты
    public Currency(string name, float exchRate)
    {
        this.name = name;
        this.exchRate = exchRate;
        sum = 0f;
    }
    public string getName()
    {
        return this.name;
    }
    public float getExchRate()
    {
        return this.exchRate;
    }
}
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{

    InterstitialAd interstitial;
    BannerView bannerView;
    // Use this for initialization
    void Start()
    {
        RequestInterstitial();
        //RequestBanner();
    }
    //    private void RequestBanner()
    //    {

    //#if UNITY_ANDROID
    //        string adUnitId = "ca-app-pub-2967267784683496/7152913563";
    //#elif UNITY_IPHONE
    //        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
    //#else
    //        string adUnitId = "unexpected_platform";
    //#endif

    //        // Create a 320x50 banner at the top of the screen.
    //        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
    //        // Create an empty ad request.
    //        AdRequest request = new AdRequest.Builder().Build();
    //        // Load the banner with the request.
    //        bannerView.LoadAd(request);
    //    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-2967267784683496/6983217777";
#elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }
    public void destroyInterstital()
    {
        //interstitial.Destroy();
    }
    public void ReqInter()
    {
        RequestInterstitial();
    }
    // Update is called once per frame
    public void showInterstital()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }
}

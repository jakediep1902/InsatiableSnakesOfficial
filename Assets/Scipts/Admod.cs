using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Admod : MonoBehaviour
{
    public static UnityEvent eventReward = new UnityEvent();
    private BannerView bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    public GameController gameController;
    public string AndroidBannerID = "ca-app-pub-3940256099942544/6300978111";//thu nghiem
    public string IOSBannerID = "ca-app-pub-3940256099942544/2934735716";//thu nghiem
    public string AndroidInterstitialID = "ca-app-pub-3940256099942544/1033173712";//thu nghiem
    public string IOSInterstitialID = "ca-app-pub-3940256099942544/4411468910";//thu nghiem
    public string AndroidRewardedID = "ca-app-pub-3940256099942544/5224354917";//thu nghiem
    public string IOSRewardedID = "ca-app-pub-3940256099942544/1712485313";//thu nghiem




    // Start is called before the first frame update
    void Start()
    {
        //BoardCountDown.eventCountDone.AddListener(() => { ShowInterstitialAd(); });
        GameController.eventGetReward.AddListener(() => { ShowRewardedAd(); });
        gameController = GameController.Instance;
        MobileAds.Initialize(initStatus =>
        {
            //RequestBanner();
            //LoadInterstitialAd();
            LoadRewardedAd();
        });       
    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = AndroidBannerID;
#elif UNITY_IPHONE
            string adUnitId = IOSBannerID;
#else
        string adUnitId = "unexpected_platform";
#endif
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);

        // Create an empty ad request.
        AdRequest request = new AdRequest();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
    public void LoadInterstitialAd()
    {
#if UNITY_ANDROID
        string _adUnitId = AndroidInterstitialID;
#elif UNITY_IPHONE
        string _adUnitId = IOSInterstitialID;
#else
        string _adUnitId = "unused";
#endif
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        Debug.Log("Loading the interstitial ad.");
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });
    }
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            _interstitialAd.Destroy();
            LoadInterstitialAd();
            eventReward?.Invoke();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
#if UNITY_ANDROID
        string _adUnitId = AndroidRewardedID;
#elif UNITY_IPHONE
   string _adUnitId = IOSRewardedID;
#else
   string _adUnitId = "unused";
#endif

        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
               
                _rewardedAd = ad;
                RegisterReloadHandler(ad);
                RegisterEventHandlers(ad);     
            });
    }

    public void ShowRewardedAd()
    {
        
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";      
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                eventReward?.Invoke();
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}

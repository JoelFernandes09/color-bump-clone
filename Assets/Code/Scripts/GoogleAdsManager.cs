using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class GoogleAdsManager : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private string testAdUnitID = "ca-app-pub-3940256099942544/5224354917";
    private UIManager uIManager;
    void Awake()
    {
        MobileAds.Initialize(initStatus => { });
        uIManager = FindObjectOfType<UIManager>();
    }

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        Debug.Log("Loading Rewarded Ad!");
        AdRequest adRequest = new AdRequest.Builder().Build();
        RewardedAd.Load(testAdUnitID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded Ad failed to load an ad with error: " + error);
                return;
            }
            Debug.Log("Rewarded ad loaded with response: " + ad.GetResponseInfo());
            rewardedAd = ad;
        });
    }

    void Start()
    {
        LoadRewardedAd();
    }

    public void ShowRewardAd()
    {

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                uIManager.ShowGameCompleteUI(true);
                Debug.Log("REWARD USER!!");
            });
        } else uIManager.ShowGameCompleteUI(false);
    }

}

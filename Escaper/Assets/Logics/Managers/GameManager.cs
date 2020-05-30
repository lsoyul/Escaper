using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// Use Google Play Services
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using GoogleMobileAds.Api;
using DigitalRuby.SoundManagerNamespace;

public class GameManager : MonoBehaviour
{
    private GameStatics.GAME_STATUS currentGameStatus;
    public GameStatics.GAME_STATUS GameStatus
    {
        get { return currentGameStatus; }
    }

    public Action<GameStatics.GAME_STATUS, GameStatics.GAME_STATUS> onChangeGameStatus;
    public Action<GameStatics.LOGIN_TYPE> onLoginFinish;
    public Action onSignout;

    private static GameObject container;
    private static GameManager instance;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "GameManager";
            instance = container.AddComponent(typeof(GameManager)) as GameManager;
        }

        return instance;
    }


    #region #### FireBase App ####

    public Firebase.FirebaseApp FirebaseApp;
    //public Firebase.Auth.FirebaseAuth FirebaseAuth;

    #endregion


    public bool IsGameInitialize = false;
    public bool IsTestMode = true;

    private void Awake() {
        
        if (instance == null) 
        {
            instance = GetComponent<GameManager>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        currentGameStatus = GameStatics.GAME_STATUS.NONE;
        ChangeGameStatus(GameStatics.GAME_STATUS.SPLASH);

    }

    void Start()
    {
        Input.multiTouchEnabled = false;

        // 1. Load GameConfigs
        // 2. Set PlayerStatus
        if (IsGameInitialize == false)
        {
            GameConfigs.LoadConfigs();
            PlayerManager.Instance().InitializePlayer();
            PlayerManager.Instance().PlayMode = GameConfigs.GetLastPlayMode();

            IsGameInitialize = true;
        }

        InitializeFireBase();
        InitializeGoogleAds();

        EffectManager.GetInstance();

        //Time_LatestStartGame = DateTime.UtcNow;

        //Screen.SetResolution(720, 1280, true, 60);
    }

    public void ChangeGameStatus(GameStatics.GAME_STATUS nextStatus)
    {
        GameStatics.GAME_STATUS beforeGameStatus = currentGameStatus;
        currentGameStatus = nextStatus;

        if (onChangeGameStatus != null) onChangeGameStatus(beforeGameStatus, currentGameStatus);
    }

    void GooglePlayServiceInit()
    {
        //PlayGamesClientConfiguration config
        //    = new PlayGamesClientConfiguration.Builder()
        //    .RequestIdToken()
        //    .Build();
        //
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();
    }

    public void SetAnonymousPlay()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onLoginFinish != null) onLoginFinish(GameStatics.LOGIN_TYPE.FAIL);
        }
        else
        {
            //Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin, "LoginType", GameStatics.EVENT_LOGIN_ANONYMOUS);
            //FirebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            //FirebaseAuth.SignInAnonymouslyAsync().ContinueWith(task =>
            //{
            //    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            //    {
            //        // User is now signed in.
            //
            //        Firebase.Auth.FirebaseUser newUser = task.Result;
            //
            //        Debug.Log(string.Format("FirebaseUser:{0}\nEmail:{1}", newUser.UserId, newUser.Email));
            //        
            //        Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_LOGIN_ANONYMOUS);
            //        //Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin, "LoginType", GameStatics.EVENT_LOGIN_ANONYMOUS);
            //
            //        if (onLoginFinish != null) onLoginFinish(GameStatics.LOGIN_TYPE.ANONYMOUS);
            //    }
            //    else
            //    {
            //        Debug.Log("failed");
            //        if (onLoginFinish != null) onLoginFinish(GameStatics.LOGIN_TYPE.FAIL);
            //    }
            //});
        }
    }

    public void OnClickGoogleLogin()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onLoginFinish != null) onLoginFinish(GameStatics.LOGIN_TYPE.FAIL);
        }
        else
        {
            GooglePlayServiceInit();
            InitializeFireBase();

            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate((bool bSuccess, string str) =>
                {
                    if (bSuccess)
                    {
                        Debug.Log("Success : " + Social.localUser.userName);

                        StartCoroutine(coLogin());
                    }
                    else
                    {
                        Debug.Log("Login Fail");
                        SetAnonymousPlay();
                    }
                });
            }
            else
            {
                // Already google logined
                // => Firebase login
                StartCoroutine(coLogin());
            }
        }
            
    }

    IEnumerator coLogin()
    {
        //Debug.Log(string.Format("\nTry to get Token..."));
        //while (System.String.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        //    yield return null;
        //
        //string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        //
        //Debug.Log(string.Format("\nToken:{0}", idToken));
        //FirebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
        //FirebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(
        //    task =>
        //    {
        //        if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
        //        {
        //            // User is now signed in.
        //            Firebase.Auth.FirebaseUser newUser = task.Result;
        //            Debug.Log(string.Format("FirebaseUser:{0}\nEmail:{1}", newUser.UserId, newUser.Email));
        //
        //            Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin, "LoginType", GameStatics.EVENT_LOGIN_GOOGLE);
        //
        //            if (onLoginFinish != null) onLoginFinish(GameStatics.LOGIN_TYPE.GOOGLE);
        //        }
        //        else
        //        {
        //
        //            if (onLoginFinish != null) onLoginFinish(GameStatics.LOGIN_TYPE.FAIL);
        //        }
        //    });
        yield return null;
    }

    public void OnClickGoogleLogout()
    {
        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    Debug.Log("Error. Check internet connection!");
        //}
        //else
        //{
        //    ((PlayGamesPlatform)Social.Active).SignOut();
        //
        //    Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin, "LoginType", GameStatics.EVENT_LOGOUT);
        //}
        //
        if (onSignout != null) onSignout();
    }

    void InitializeFireBase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                FirebaseApp = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(false);
            }
        });
    }
    #region #### Play Time ####

    public DateTime Time_LatestStartGame = new DateTime();

    #endregion

    #region #### Google Ads ####

    const string rewardAdUnit_test = "ca-app-pub-3940256099942544/5224354917";
    const string rewardAdUnit_revive = "ca-app-pub-1021255306046408/1683457734";


    string adUnitRewardId = string.Empty;


    private RewardedAd Rewarded_Revive_Ad;

    public Action onUserEarnedReward_Revive;
    public Action onAdLoaded_Revive;
    public Action onAdClosed_Revive;

    void InitializeGoogleAds()
    {
        if (IsTestMode) adUnitRewardId = rewardAdUnit_test;
        else adUnitRewardId = rewardAdUnit_revive;

        MobileAds.Initialize(initStatus => {
            Rewarded_Revive_Ad = CreateAndLoadRewardedAd(adUnitRewardId);
        });
    }

    RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        return rewardedAd;
    }

    public void ShowReviveAds()
    {
        if (this.Rewarded_Revive_Ad.IsLoaded())
        {
            this.Rewarded_Revive_Ad.Show();
        }
    }

    void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");

        if (onAdLoaded_Revive != null) onAdLoaded_Revive();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");

        Rewarded_Revive_Ad = CreateAndLoadRewardedAd(adUnitRewardId);

        if (onAdClosed_Revive != null) onAdClosed_Revive();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        StartCoroutine(WaitUserEarnedRewardBecauseOfCrash());
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    IEnumerator WaitUserEarnedRewardBecauseOfCrash()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        if (onUserEarnedReward_Revive != null) onUserEarnedReward_Revive();
    }

    #endregion

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            GameConfigs.SetPlayTime(TopMostControl.Instance().playUnixTime);
        }
        else
        {
            //Time_LatestStartGame = DateTime.UtcNow;
        }
    }

    private void OnApplicationQuit()
    {
        GameConfigs.SetPlayTime(TopMostControl.Instance().playUnixTime);
    }

}

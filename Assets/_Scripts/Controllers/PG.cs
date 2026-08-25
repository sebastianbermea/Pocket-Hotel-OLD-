#if UNITY_ANDROID
    using GooglePlayGames;
#elif UNITY_IOS

#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PG : MonoBehaviour
{


    public void AddScoreToLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
#if UNITY_ANDROID
            Social.ReportScore((long)(GC.INS.stars * 1000), "CgkIpuTpvpsQEAIQAg", success => { });
#elif UNITY_IPHONE
            Social.ReportScore((long)(GC.INS.stars * 1000), "com.stars", success => { });
#endif
        }
    }
    /*void AddScoreTest()
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportScore(1, "CgkIpuTpvpsQEAIQAg", success => {
                Debug.Log("Sucess: " + success);
                
            });
        }
        else
        {
            Debug.Log("Not Autenticated");
        }
    }*/
    public void ShowLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
            Debug.Log("Social good");
            AddScoreToLeaderboard();
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIpuTpvpsQEAIQAg");
#elif UNITY_IPHONE
            Social.ShowLeaderboardUI();
#endif
        }
    }

    public void ShowAchievements()
    {
        if (Social.Active.localUser.authenticated)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowAchievementsUI();
#elif UNITY_IPHONE
            Social.ShowAchievementsUI();
#endif
        }
    }
    
    public void Achivements(int id, int x)
    {
        if (!Social.Active.localUser.authenticated)
            return;
        switch (id)
        {
            case 0:
#if UNITY_ANDROID
                if (GC.INS.level < 6)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQDQ", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.level < 20)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQDg", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.level < 50)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQDw", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
#elif UNITY_IPHONE
                if (GC.INS.level < 6)
                {
                    Social.ReportProgress("com.level5", GC.INS.level/.05f, success => { });
                }
                if (GC.INS.level < 20)
                {
                    Social.ReportProgress("com.level20", GC.INS.level/.2, success => { });
                }
                if (GC.INS.level < 50)
                {
                    Social.ReportProgress("com.level50", GC.INS.level / .5, success => { });
                }
#endif
                break;
            case 1:
#if UNITY_ANDROID
                if (x == 0)
                    Social.ReportProgress("CgkIpuTpvpsQEAIQEA", 100f, success => { });
                else if (x == 1)
                    Social.ReportProgress("CgkIpuTpvpsQEAIQEQ", 100f, success => { });
                else
                    Social.ReportProgress("CgkIpuTpvpsQEAIQEg", 100f, success => { });

#elif UNITY_IPHONE
                if (x == 0)
                    Social.ReportProgress("com.3stars", 100f, success => { });
                else if (x == 1)
                    Social.ReportProgress("com.4stars", 100f, success => { });
                else
                    Social.ReportProgress("com.5stars", 100f, success => { });

#endif
                break;
            case 2:
#if UNITY_ANDROID
                if (x == 0)
                    Social.ReportProgress("CgkIpuTpvpsQEAIQEw", 100f, success => { });
                else if (x == 1)
                    Social.ReportProgress("CgkIpuTpvpsQEAIQFA", 100f, success => { });
                else
                    Social.ReportProgress("CgkIpuTpvpsQEAIQFQ", 100f, success => { });
#elif UNITY_IPHONE
                if (x == 0)
                    Social.ReportProgress("com.1000CR", 100f, success => { });
                else if (x == 1)
                    Social.ReportProgress("com.2000CR", 100f, success => { });
                else
                    Social.ReportProgress("com.5000CR", 100f, success => { });
                
#endif
                break;
            case 3:

                GC.INS.roomFix++;
#if UNITY_ANDROID
                if (GC.INS.roomFix < 51)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQFg", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.roomFix < 201)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQFw", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.roomFix < 1001)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQGA", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
#elif UNITY_IPHONE
                if (GC.INS.roomFix < 51)
                {
                    Social.ReportProgress("com.50RF", GC.INS.roomFix / .5f, success => { });
                }
                if (GC.INS.roomFix < 201)
                {
                    Social.ReportProgress("com.200RF", GC.INS.roomFix / 2f, success => { });
                }
                if (GC.INS.roomFix < 1001)
                {
                    Social.ReportProgress("com.com.100RF", GC.INS.roomFix / 10f, success => { });
                }
#endif
                break;
            case 4:
#if UNITY_ANDROID
                if (FRC.INS.friendList.Count < 4)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQGQ", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (FRC.INS.friendList.Count < 6)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQGg", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (FRC.INS.friendList.Count < 11)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQGw", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
#elif UNITY_IPHONE
                if (FRC.INS.friendList.Count < 4)
                {
                    Social.ReportProgress("com.3friends", FRC.INS.friendList.Count / .03f, success => { });
                }
                if (FRC.INS.friendList.Count < 6)
                {
                    Social.ReportProgress("com.5friends", FRC.INS.friendList.Count / .05f, success => { });
                }
                if (FRC.INS.friendList.Count < 11)
                {
                    Social.ReportProgress("com.10friends", FRC.INS.friendList.Count / .1f, success => { });
                }
#endif
                break;
            case 5:
                GC.INS.giftCount++;
#if UNITY_ANDROID
                if (GC.INS.giftCount < 2)
                {
                    Social.ReportProgress("CgkIpuTpvpsQEAIQHA", 100f, success => { });
                }
                if (GC.INS.giftCount<6)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQHQ", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.giftCount < 26)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQHg", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }

#elif UNITY_IPHONE
                if (GC.INS.giftCount < 2)
                {
                    Social.ReportProgress("com.1gift", 100f, success => { });
                }
                if (GC.INS.giftCount < 6)
                {
                    Social.ReportProgress("com.5gift", GC.INS.giftCount / .05f, success => { });
                }
                if (GC.INS.giftCount < 26)
                {
                    Social.ReportProgress("com.25gift", GC.INS.giftCount / .25f, success => { });
                }

#endif
                break;
            case 6:
                GC.INS.dailyMCount++;
#if UNITY_ANDROID
                if (GC.INS.dailyMCount < 4)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQHw", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.dailyMCount < 11)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQIA", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }
                if (GC.INS.dailyMCount<26)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(
                        "CgkIpuTpvpsQEAIQIQ", 1, (bool success) =>
                        {
                            // handle success or failure
                        });
                }

#elif UNITY_IPHONE
                if (GC.INS.dailyMCount < 4)
                {
                    Social.ReportProgress("com.3DM", GC.INS.giftCount / .03f, success => { });
                }
                if (GC.INS.dailyMCount < 11)
                {
                    Social.ReportProgress("com.10DM", GC.INS.giftCount / .1f, success => { });
                }
                if (GC.INS.dailyMCount < 26)
                {
                    Social.ReportProgress("com.25DM", GC.INS.giftCount / .25f, success => { });
                }

#endif
                break;
            case 7:
#if UNITY_ANDROID
                if (x == 0)
                    Social.ReportProgress("CgkIpuTpvpsQEAIQIg", 100f, success => { });
                else if (x == 1)
                    Social.ReportProgress("CgkIpuTpvpsQEAIQIw", 100f, success => { });
                else
                    Social.ReportProgress("CgkIpuTpvpsQEAIQJA", 100f, success => { });

#elif UNITY_IPHONE
                if (x == 0)
                    Social.ReportProgress("com.prestige1", 100f, success => { });
                else if (x == 1)
                    Social.ReportProgress("com.prestige2", 100f, success => { });
                else
                    Social.ReportProgress("com.prestige5", 100f, success => { });
#endif
                break;

        }

    }

}

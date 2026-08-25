using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Google.Play.Review;
#endif
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class AppReview : MonoBehaviour
{
#if UNITY_ANDROID
    ReviewManager reviewManager;
    PlayReviewInfo reviewInfo;
#endif
    bool reviewChecked;
    private void Start()
    {
        reviewChecked = (PlayerPrefs.GetInt("review")==1);
    }
    public void AskReview()
    {
        if (reviewChecked)
            return;
#if UNITY_ANDROID
        reviewManager = new ReviewManager();
        StartCoroutine(ReviewOp());
#endif
#if UNITY_IOS
        if (Device.RequestStoreReview())
        {
            Debug.Log("IOS REVIEWD");
            PlayerPrefs.SetInt("review", 1);
        }
#endif
    }
#if UNITY_ANDROID
    IEnumerator ReviewOp()
    {
        yield return new WaitForSeconds(1);

        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if(requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }

        reviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(reviewInfo);
        yield return launchFlowOperation;
        reviewInfo = null;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(launchFlowOperation.Error.ToString());
            yield break;
        }

        PlayerPrefs.SetInt("review", 1);
    }
#endif
}

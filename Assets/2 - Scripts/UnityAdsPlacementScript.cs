using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;
public class UnityAdsPlacementScript : MonoBehaviour {

	public string placementId = "revive";

    public void ShowAd () {
        StartCoroutine (WaitForAd ());
    }

    IEnumerator WaitForAd () {
        while (!Monetization.IsReady (placementId)) {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent (placementId) as ShowAdPlacementContent;

        if (ad != null) {
            ad.Show (AdFinished);
        }
    }

    void AdFinished (ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
            GameObject.Find("_ReviveManager").GetComponent<MyReviveManagerScript>().Revive();
            break;

            case ShowResult.Failed:
            GameObject.Find("_ReviveManager").GetComponent<MyReviveManagerScript>().GameOverScreen();
            break;

            case ShowResult.Skipped:
            GameObject.Find("_ReviveManager").GetComponent<MyReviveManagerScript>().GameOverScreen();
            break;
        }
    }

}

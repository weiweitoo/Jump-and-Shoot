using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public class UnityAdsScript : MonoBehaviour {

	string gameId = "2990386";
    bool testMode = false;

    void Start () {
        Monetization.Initialize (gameId, testMode);
    }

}

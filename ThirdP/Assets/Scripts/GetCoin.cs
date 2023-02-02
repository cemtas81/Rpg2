using PlayerNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            MyScore playerScore = FindObjectOfType<MyScore>();
            playerScore.Scoring(1);
        }
    }
   
}

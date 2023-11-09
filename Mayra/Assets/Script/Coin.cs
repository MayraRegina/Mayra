using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Coin : MonoBehaviour
{
    public Text coinText; 
    private int coinCount = 0; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            IncrementCoinCount();
        }
    }

    void IncrementCoinCount()
    {
        coinCount++;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = " " + coinCount.ToString();
        }
    }
}
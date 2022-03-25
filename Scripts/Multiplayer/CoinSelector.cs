using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSelector : MonoBehaviour
{
    [SerializeField] private List<int> betCoins;
    [SerializeField] private Button previous, next;
    public int current;
    [SerializeField] private Text betText;

    public void SetData(List<int> betCoins)
    {
        betText.text = "";

        this.betCoins = betCoins;
        if (betCoins.Count > 0)
        {
            current = 0;
            betText.text = betCoins[current].ToString();
            previous.onClick.AddListener(Previous);
            next.onClick.AddListener(Next);
        }
        else
        {
            current = -1;
            betText.text = "NO COINS ";
        }
    }

    void OnDisable()
    {
        previous.onClick.RemoveListener(Previous);
        next.onClick.RemoveListener(Next);
    }

    public int GetCurrentBet()
    {
        return betCoins[current];
    }

    void Previous()
    {
        if (current > 0)
        {
            current--;
            betText.text = betCoins[current].ToString();
        }
    }

    void Next()
    {
        if (current < betCoins.Count - 1)
        {
            current++;
            betText.text = betCoins[current].ToString();
        }
    }
}
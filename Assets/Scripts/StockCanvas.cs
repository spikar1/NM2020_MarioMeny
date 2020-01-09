using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockCanvas : MonoBehaviour
{
    public TextMeshProUGUI[] textPros;

    static StockCanvas instance = null;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DisablePlayerStocks();
    }

    public void PlayerJoined(int playerNumber, int playerStock)
    {
        for (int i = 0; i < textPros.Length; i++)
        {
            if(i == playerNumber - 1)
            {
                textPros[i].enabled = true;
                textPros[i].text = $"P{playerNumber}:\n{playerStock}";
                textPros[i].color = Manager.WorldOptions.playerColors[playerNumber - 1];
            }
        }
    }

    public void UpdatePlayerStock(int playerNumber, int playerStock)
    {
        for (int i = 0; i < textPros.Length; i++)
        {
            if(i == playerNumber - 1)
            {
                if(playerStock > 0)
                {
                    textPros[i].text = $"P{playerNumber}:\n{playerStock}";
                }
                else
                {
                    textPros[i].text = $"P{playerNumber}:\nDEAD";
                }
            }
        }
    }

    public void DisablePlayerStocks()
    {
        for (int i = 0; i < textPros.Length; i++)
        {
            textPros[i].enabled = false;
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }

}

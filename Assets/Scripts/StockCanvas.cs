using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StockCanvas : MonoBehaviour
{
    public TextMeshProUGUI[] textPros;
    public GameObject[] slots;
    public GameObject textPrefab;

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
        for (int i = 0; i < slots.Length; i++)
        {
            if(i == playerNumber - 1)
            {
                slots[i].SetActive(true);
                var tex = slots[i].GetComponentInChildren<TextMeshProUGUI>();

                tex.text = $"P{playerNumber}:\n{playerStock}";
                tex.color = Manager.WorldOptions.playerColors[playerNumber - 1];

                Image[] images = slots[i].GetComponentsInChildren<Image>();
                foreach (Image image in images)
                {
                    if(image.gameObject.transform.parent.transform.parent != null)
                    {
                        image.color = Manager.WorldOptions.playerColors[playerNumber - 1];
                    }
                }
            }
        }

        //Old:
        /*
        for (int i = 0; i < textPros.Length; i++)
        {
            if(i == playerNumber - 1)
            {
                textPros[i].gameObject.SetActive(true);
                textPros[i].text = $"P{playerNumber}:\n{playerStock}";
                textPros[i].color = Manager.WorldOptions.playerColors[playerNumber - 1];
            }
        }*/
    }

    public void UpdatePlayerStock(int playerNumber, int playerStock)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == playerNumber - 1)
            {
                var tex = slots[i].GetComponentInChildren<TextMeshProUGUI>();

                if (playerStock > 0)
                {
                    tex.text = $"P{playerNumber}:\n{playerStock}";
                    Image[] images = slots[i].GetComponentsInChildren<Image>();
                    foreach (Image image in images)
                    {
                        if (image.gameObject.transform.parent.transform.parent != null)
                        {
                            StartCoroutine(DamageImage(image, 0.3f, slots[i], 1f));
                        }
                    }

                }
                else
                {
                    tex.text = $"P{playerNumber}:\n0";
                }
            }
        }


        //Old:
        /*
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
        }*/
    }

    public void DisablePlayerStocks()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(false);
        }

        //old:
        /*
        for (int i = 0; i < textPros.Length; i++)
        {
            textPros[i].gameObject.SetActive(false);
        }*/
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageImage(Image image, float damageTime, GameObject parent, float textTime)
    {
        Color startColor = image.color;
        GameObject clone = Instantiate(textPrefab, parent.transform.position + new Vector3(40,-20,0), Quaternion.identity);
        clone.transform.parent = parent.transform;
        clone.GetComponent<TextMeshProUGUI>().color = startColor;


        image.color = Color.white;
        yield return new WaitForSeconds(damageTime);
        image.color = startColor;

        yield return new WaitForSeconds(textTime);
        Destroy(clone);
    }

}

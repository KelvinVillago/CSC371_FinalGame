
using UnityEngine;
using TMPro;
using System;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Coins: 0";
    }

    public void increaseNum()
    {
        num += 50;
    }

    public int Count
    {
        get
        {
            // return the count of coins
            return num;
        }
    }


    // Update is called once per frame
    void Update()
    {
        text.text = num.ToString();
    }

}

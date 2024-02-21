using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI text;
    int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Coins: 0";
    }

    public void increaseNum()
    {
        num += 10;
    }


    // Update is called once per frame
    void Update()
    {
        text.text = "Coins: " + num.ToString();
    }

}

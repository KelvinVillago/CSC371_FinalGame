using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopTemplate : MonoBehaviour
{
    public Type MenuType { get; set; }
    public int PositionInMenu { get; set; }
    public TMP_Text TitleText { get; set; }
    public TMP_Text DescriptionText { get; set; }
    public TMP_Text PriceText { get; set; }
    public Button Button { get; set; }
    public TMP_Text ButtonText { get; set; }

    public void PurchasedButton()
    {
        ButtonText.text = "Purchased";
        Button.interactable = false;
    }

}

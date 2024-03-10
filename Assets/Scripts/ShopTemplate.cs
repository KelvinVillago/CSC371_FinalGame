using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public class ShopTemplate : MonoBehaviour
{
    [field: SerializeField] public int PositionInMenu { get; set; } = 0;
    [field:SerializeField] public TMP_Text TitleText { get; private set;}
    [field:SerializeField] public TMP_Text DescriptionText { get;  private set;}
    [field:SerializeField] public TMP_Text PriceText { get;  private set;}
    [field:SerializeField] public Button Button { get;  private set;}
    [field:SerializeField] public TMP_Text ButtonText { get;  private set;}

    private void Start()
    {
    }
    public void PurchasedButton()
    {
        ButtonText.text = "Purchased";
        Button.interactable = false;
    }
}


using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EquiptSlot : MonoBehaviour
{
    [field: SerializeField] public Image Image { get; set; }
    [field: SerializeField] public TextMeshProUGUI AmountText { get; set; }
    [field: SerializeField] public Button Button { get; set; }
    public Item Item { get; set; } = null;
}
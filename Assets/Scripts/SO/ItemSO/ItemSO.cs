using System;
using UnityEngine;
using UnityEngine.UI;

//Not Abstract because basic items can be created with this information.
[CreateAssetMenu(fileName = "NewItemSO", menuName = "Scribtable Objects / Item / New Basic Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [field: Header("Basic Item Properties")]
    [field: Tooltip("The ID of the item, to keep things unique")]
    [field: SerializeField] public int ID { get; private set; }

    [field: Tooltip("The name of the item")]
    [field: SerializeField] public string Name { get; private set; }

    [field: Tooltip("The description of the item. It will show in the shop")]
    [field: SerializeField] public string Description { get; private set; }

    [field: Tooltip("An image for the inventory icon")]
    [field: SerializeField] public Sprite IconSprite { get; private set; }

    [field: Tooltip("The refrence to the prefab of the object")]
    [field: SerializeField] public GameObject Prefab { get; private set; }
}

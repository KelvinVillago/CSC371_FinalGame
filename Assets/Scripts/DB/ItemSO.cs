using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemSO : ScriptableObject
{
    [Tooltip("The ID of the item, to keep things unique")]
    [field: SerializeField] public int ID { get; private set; }

    [Tooltip("The name of the item")]
    [field: SerializeField] public string Name { get; private set; }

    [Tooltip("The description of the item. It will show in the shop")]
    [field: SerializeField] public String Description { get; private set; }

    [Tooltip("An image for the inventory icon")]
    [field: SerializeField] public Sprite IconSprite { get; private set; }

    [Tooltip("The refrence to the prefab of the object")]
    [field: SerializeField] public GameObject Prefab { get; private set; }
}

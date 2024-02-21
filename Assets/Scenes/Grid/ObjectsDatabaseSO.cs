using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectsData;
}

[Serializable]
public class ObjectData
{
    //public int MyProperty { get; set; }
    //public getter = other scripts can get
    //private setter = It can only be set in the serializedField (in the inspector).
    [Tooltip("The name of the item")]
    [field: SerializeField] public string Name { get; private set; }
    [Tooltip("The ID of the item, to keep things unique")]
    [field: SerializeField] public int ID { get; private set; }
    [Tooltip("The Size of the item, default is 1x1")]
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    [Tooltip("The refrence to the prefab of the object")]
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
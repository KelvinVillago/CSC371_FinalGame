using UnityEngine;

//Add to an empty game object, Select the OS you want to spawn into the world.
//On Awake it will delete the empty game object and replace it with the prefab.
public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;

    private void Awake()
    {
        if(item.type == null)
        {
            //If the spawn was made from the inspector it may not generate the type. 
            item = new Item(item.itemSO, item.amount);
        }
        ItemWorld.SpawnItemWorld(item, transform.position);
        Destroy(gameObject);
    }
}
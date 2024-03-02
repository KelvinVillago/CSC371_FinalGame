
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item Item { get; set; }

    public static ItemWorld SpawnItemWorld(Item item, Vector3 position)
    {
        //Create the new Object
        GameObject newObj = Instantiate(item.itemSO.Prefab, position, Quaternion.identity);
        //Get the itemWorld instance from the new object
        ItemWorld itemWorld = newObj.GetComponent<ItemWorld>();
        //Set the item to itself. 
        itemWorld.Item = item;
        //Return the itemWorld for other functions to refrence.
        return itemWorld;
    }
    public void DestroySelf()
    {
        //Prevent error is something else tryed to destroy it first. 
        if(gameObject)
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    public Rigidbody player;
    public Animator run;
    private float x;
    private float y;
    //public Transform parent;
    //public Rigidbody rb;
    //[SerializeField] Animator idle;
    // Start is called before the first frame update
    void Start()
    {
        run = GetComponent<Animator>();
        player = GetComponentInParent<Rigidbody>();
        x = player.position.x;
        y = player.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (x != player.position.x || y != player.position.y)
        {
            run.enabled = true;
            Debug.Log("moving");
        }
        else {
            run.enabled = false;
            Debug.Log("not Moving");
        }

        x = player.position.x;
        y = player.position.y;
    }
}

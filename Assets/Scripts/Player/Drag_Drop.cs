using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


public class Drag_Drop : MonoBehaviour

{
    public GameObject itemObject;
    public PlayerController controller;
    public Camera camera;

    public LayerMask oblayer;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, oblayer))
        {
            Debug.Log("상호작용레이어 포착!");
            if(hit.transform.CompareTag("obLayer"))
            {
                // GetComponent<itemobject>().
                //     hit.
            }
        }
        
    }
}
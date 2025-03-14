using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;


public class DragDrop : MonoBehaviour

{
    public Camera camera;
    public bool isHolding = false;
    public LayerMask oblayer;
    public Rigidbody heldItem;
    public Transform holdPositon;

    public float pickupRange = 5f;

    void Start()
    {
        camera = Camera.main;
    }

    public void Pickup()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //상호작용레이어감지
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,pickupRange, oblayer))
        {
            Rigidbody itemRB = hit.collider.GetComponent<Rigidbody>();
            if (itemRB != null)
            {
                heldItem = itemRB;
                heldItem.useGravity = false;
                isHolding = true;
            }

        }
    }

    public void OnPickupInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started )
        {

            if(isHolding)
            {
                Drop();              
            }
            else
            {
                Pickup();
            }
        }
    }
 
    public void Drop()
    {
        heldItem.useGravity = true ;
        isHolding= false ;
        heldItem = null ;  
    }


    private void FixedUpdate()
    {
        if(isHolding == true && heldItem != null)
        {
            Vector3 targetPosition = holdPositon.position;
            //Vector3 dir = targetPosition -heldItem.position;
            //heldItem.velocity = dir;
            heldItem.transform.position = targetPosition;
        }
    }
}
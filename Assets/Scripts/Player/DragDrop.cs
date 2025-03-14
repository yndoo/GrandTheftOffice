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
    public LayerMask cantDragLayer;
    public Rigidbody heldItem;
    public Transform holdPositon;

    public float pickupRange = 5f;
    private float heldDistance;

    private Coroutine RotateCoroutine;

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
                holdPositon.position = hit.transform.position;
                heldDistance = Vector3.Distance(transform.position, heldItem.position);
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
        heldItem.includeLayers = oblayer;
        isHolding = false ;
        heldItem = null ;  
    }


    private void FixedUpdate()
    {
        if(isHolding == true && heldItem != null)
        {
            Vector3 targetPosition = holdPositon.position;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 벽뚫 방지
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, heldDistance, cantDragLayer))
            {
                targetPosition = hit.point;
            }

            heldItem.transform.position = targetPosition;
        }
    }

    public void OnHoldingRotate(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            RotateCoroutine = StartCoroutine(RotateItem());
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            if(RotateCoroutine != null)
            {
                StopCoroutine(RotateCoroutine);
                RotateCoroutine = null;
            }
        }
    }

    IEnumerator RotateItem()
    {
        while(true)
        {
            heldItem.gameObject.transform.Rotate(Vector3.right * 50 * Time.deltaTime);
            yield return null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCheck : MonoBehaviour
{
    public GameObject Target;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("WeightButton"))
        {
            //Debug.Log("눌림");
            Target.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("WeightButton"))
        {
            //Debug.Log("안눌림");
            Target.SetActive(false);
        }
    }
}

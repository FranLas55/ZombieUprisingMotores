using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Tobias Rodriguez Vieyra

public class Meta : MonoBehaviour
{
    //[SerializeField] private Canvas _canvasToActive;
    
   
   /* private void Start()
    {
        if (_canvasToActive != null)
        {
            _canvasToActive.enabled = false;
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.GetComponent<Player>())
       {
            GameManager.Instance.Win();
           // Cursor.lockState = CursorLockMode.None;
           // ActiveCanvas();
       }
    }

    /*private void ActiveCanvas()
    {
        if ( _canvasToActive != null )
        {
            _canvasToActive.enabled = true;
        }
    }*/
}

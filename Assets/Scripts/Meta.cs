using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Tobias Rodriguez Vieyra

public class Meta : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.TryGetComponent(out Player player))
       {
           player.Win();
       }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscopetaAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _machineGun;

    private void GunON()
    {
        if (_machineGun != null)
        {
            if (_machineGun.name == "Escopeta")
            {
                _machineGun.SetActive(true);
            }
        }
    }

    private void GunOF()
    {
        if (_machineGun != null)
        {
            if (_machineGun.name == "Escopeta") 
            {
                _machineGun.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _machineGun;

    private void MachineGunON()
    {
        _machineGun.SetActive(true);
    }

    private void MachineGunOF()
    {
        _machineGun.SetActive(false);
    }
}

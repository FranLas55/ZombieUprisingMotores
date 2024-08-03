using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnpointsInRoom;
    [SerializeField] private RoomChecker _checker;

    private void Start()
    {
        _checker = GetComponentInParent<RoomChecker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _checker.ActivateRoom(this);
        }
    }

    public Transform[] ReturnSpawnPoints()
    {
        return _spawnpointsInRoom;
    }
}

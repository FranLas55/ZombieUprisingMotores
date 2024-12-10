using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnpointsInRoom;

    private void Start()
    {
        foreach (var spawnpoint in _spawnpointsInRoom)
        {
            GameManager.Instance.AddSpawnPoint(spawnpoint);
        }
        
        RoomChecker.Instance.AddRoom(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            RoomChecker.Instance.ActivateRoom(this);
        }
    }

    public Transform[] ReturnSpawnPoints()
    {
        return _spawnpointsInRoom;
    }
}

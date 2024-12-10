using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    public static RoomChecker Instance { get; private set; }

    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private List<Room> _rooms = new();

    public void ActivateRoom(Room roomToActivate)
    {
        print($"activando {roomToActivate.name}");
        bool active = false;
        foreach (var room in _rooms)
        {
            Transform[] spawnpoints = room.ReturnSpawnPoints();

            active = room == roomToActivate;

            foreach (var VARIABLE in spawnpoints)
            {
                VARIABLE.gameObject.SetActive(active);
            }
        }
    }

    public void AddRoom(Room room)
    {
        if(!_rooms.Contains(room)) _rooms.Add(room);
    }
}

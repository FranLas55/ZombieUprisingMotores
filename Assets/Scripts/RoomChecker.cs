using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [SerializeField] private Room[] _rooms;

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
}

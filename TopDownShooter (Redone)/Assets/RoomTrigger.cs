using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RoomTrigger : MonoBehaviour
{
    public enum TriggerPosition
    {
        North,
        East,
        South,
        West
    }

    public TriggerPosition _triggerPosition; // Get the public enum for direction

    [Header("Ranges")]
    [SerializeField] private float _rangeX;
    [SerializeField] private float _rangeY;

    [Header("References")]
    [SerializeField] private float _destroyTime;
    [SerializeField] private Transform _roomCentre;
    private Vector2 _roomCentrePosition;
    private RoomTemplates _roomLists;

    private int _rand; // Random number
    private bool _spawned = false;


    private void Start()
    {
        // Room Centre Coordinate
        _roomCentrePosition = _roomCentre.transform.position;

        _roomLists = GameObject.FindGameObjectWithTag("RoomTemplate").GetComponent<RoomTemplates>();
    }


    // Check if anything enters the box collider
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (_spawned != true)
        {        // Check if its the player
            if (hitInfo.CompareTag("Player"))
            {
                // Run a different function depending on the assigned trigger position
                switch (_triggerPosition)
                {

                    // If the enum on the trigger is North
                    case (TriggerPosition.North):
                        // Get a random number between 0 and the length of the list - 1
                        _rand = Random.Range(0, (_roomLists.SouthOpenRooms.Count));
                        print($"{_roomLists.NorthOpenRooms.Count}, {_roomLists.NorthOpenRooms.Count - 1}, {_rand}");
                        // Spawn a room from the appropriate list
                        SpawnRoom(_roomLists.SouthOpenRooms[_rand], new Vector2(_roomCentrePosition.x, _roomCentrePosition.y + (2 * _rangeY)));
                        break;


                    case (TriggerPosition.East):
                        _rand = Random.Range(0, (_roomLists.WestOpenRooms.Count));
                        SpawnRoom(_roomLists.WestOpenRooms[_rand], new Vector2(_roomCentrePosition.x + (2 * _rangeX), _roomCentrePosition.y));
                        break;


                    case (TriggerPosition.South):
                        _rand = Random.Range(0, (_roomLists.NorthOpenRooms.Count));
                        SpawnRoom(_roomLists.NorthOpenRooms[_rand], new Vector2(_roomCentrePosition.x, _roomCentrePosition.y - (2 * _rangeY)));
                        break;


                    case (TriggerPosition.West):
                        _rand = Random.Range(0, (_roomLists.EastOpenRooms.Count));
                        SpawnRoom(_roomLists.EastOpenRooms[_rand], new Vector2(_roomCentrePosition.x - (2 * _rangeX), _roomCentrePosition.y));
                        break;
                }
            }
        }
    }



    private void SpawnRoom(GameObject room, Vector2 roomPosition)
    {
        _spawned = true;
        Instantiate(room, roomPosition, Quaternion.identity);
        Destroy(gameObject.transform.parent.transform.parent.gameObject); // TODO: make it inactive instaed, and object pool all the rooms
    }
}



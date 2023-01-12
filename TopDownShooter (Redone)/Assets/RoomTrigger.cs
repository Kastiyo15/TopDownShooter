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
    [SerializeField] private EnemyWaveSpawner _scriptWaveSpawner;
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

        _scriptWaveSpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemyWaveSpawner>();
    }


    // Check if anything enters the box collider
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (_spawned != true && hitInfo.CompareTag("Player"))
        {        // Check if its the player

            // Run a different function depending on the assigned trigger position
            switch (_triggerPosition)
            {

                // If the enum on the trigger is North
                case (TriggerPosition.North):
                    // Get a random number between 0 and the length of the list - 1
                    _rand = Random.Range(0, (_roomLists.SouthOpenRooms.Count));
                    print($"{_roomLists.NorthOpenRooms.Count}, {_roomLists.NorthOpenRooms.Count - 1}, {_rand}");
                    // Spawn a room from the appropriate list
                    StartCoroutine(SpawnClosedRoom(_roomLists.SouthOpenRooms[_rand], new Vector2(_roomCentrePosition.x, _roomCentrePosition.y + (2 * _rangeY))));
                    break;


                case (TriggerPosition.East):
                    _rand = Random.Range(0, (_roomLists.WestOpenRooms.Count));
                    StartCoroutine(SpawnClosedRoom(_roomLists.WestOpenRooms[_rand], new Vector2(_roomCentrePosition.x + (2 * _rangeX), _roomCentrePosition.y)));
                    break;


                case (TriggerPosition.South):
                    _rand = Random.Range(0, (_roomLists.NorthOpenRooms.Count));
                    StartCoroutine(SpawnClosedRoom(_roomLists.NorthOpenRooms[_rand], new Vector2(_roomCentrePosition.x, _roomCentrePosition.y - (2 * _rangeY))));
                    break;


                case (TriggerPosition.West):
                    _rand = Random.Range(0, (_roomLists.EastOpenRooms.Count));
                    StartCoroutine(SpawnClosedRoom(_roomLists.EastOpenRooms[_rand], new Vector2(_roomCentrePosition.x - (2 * _rangeX), _roomCentrePosition.y)));
                    break;

            }
        }
    }



    private IEnumerator SpawnClosedRoom(GameObject newRoom, Vector2 roomPosition)
    {
        // Set the box area room centre
        _scriptWaveSpawner.GetRoomCentre(roomPosition);

        // After spawning in the right room, start this script enables enemy spawning
        _scriptWaveSpawner.StartWave();

        // Spawn in a closed room, and destroy old room
        _spawned = true;
        GameObject closedRoom = Instantiate(_roomLists.ClosedRoom, roomPosition, Quaternion.identity);

        while (!_scriptWaveSpawner.WaveComplete)
        {
            yield return null;
        }

        // above code waits until wave is complete, all enemies are dead, then runs this
        Instantiate(newRoom, roomPosition, Quaternion.identity);
        Destroy(closedRoom);
        Destroy(gameObject.transform.parent.transform.parent.gameObject); // TODO: make it inactive instaed, and object pool all the rooms
    }
}
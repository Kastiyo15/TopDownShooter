using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;


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
    private GameObject _prefabParent; // the game object parent of the room prefab (to be destroyed)
    private CinemachineConfiner2D _cameraConfiner;
    private EnemyWaveSpawner _scriptWaveSpawner;

    private int _rand; // Random number
    private bool _spawned = false;


    private void Start()
    {
        _prefabParent = gameObject.transform.parent.transform.parent.gameObject;

        // Get access to cinemachine camera confiner2d component
        _cameraConfiner = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CinemachineConfiner2D>();

        // Set the camera confiner zone
        SetCameraConfineZone(_prefabParent);

        // Room Centre Coordinate
        _roomCentrePosition = _roomCentre.transform.position;

        // Get access to the roomtemplates script
        _roomLists = GameObject.FindGameObjectWithTag("RoomTemplate").GetComponent<RoomTemplates>();

        // Get access to the enemywavespawner script
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

        // Spawn the new room below the closed room (in the hierarchy)
        GameObject currentRoom = Instantiate(newRoom, roomPosition, Quaternion.identity);

        // Set the camera confiner zone
        SetCameraConfineZone(currentRoom);

        var once = true;
        var newRoomWallsPrefab = currentRoom.transform.GetChild(0).gameObject;

        // Wait until all enemies are killed before we destroy the rooms
        while (!_scriptWaveSpawner.WaveComplete)
        {
            if (once == true)
            {
                // Inactivate the walls folder (containing all the box colliders) on the rooms we dont need them on
                // original room
                _prefabParent.transform.GetChild(0).gameObject.SetActive(false);
                // new room
                newRoomWallsPrefab.SetActive(false);
                once = false;
            }
            yield return null;
        }
        
        Destroy(closedRoom);
        Destroy(_prefabParent); // TODO: make it inactive instaed, and object pool all the rooms

        newRoomWallsPrefab.SetActive(true);
    }


    // Set the camera confiner zone
    private void SetCameraConfineZone(GameObject currentRoom)
    {
        _cameraConfiner.m_BoundingShape2D = currentRoom.GetComponentInChildren<PolygonCollider2D>();
    }
}
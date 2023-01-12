using System.Collections.Generic;
using UnityEngine;


public class RoomTemplates : MonoBehaviour
{
    public List<GameObject> SouthOpenRooms = new List<GameObject>();
    public List<GameObject> WestOpenRooms = new List<GameObject>();
    public List<GameObject> NorthOpenRooms = new List<GameObject>();
    public List<GameObject> EastOpenRooms = new List<GameObject>();

    public GameObject ClosedRoom;
}

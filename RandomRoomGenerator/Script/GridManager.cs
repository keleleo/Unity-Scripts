using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{

    public enum DoorsDirection
    {
        front,
        back,
        left,
        right
    }
    [SerializeField] private int maxRooms = 10;
    [SerializeField] private int roomsCount = 1;
    [SerializeField] private GameObject gridSquare;
    [System.Serializable]
    private struct RoomsToCreate
    {
        public Vector2 position;
        public DoorsDirection requireDirection;

        public RoomsToCreate(Vector2 position, DoorsDirection requireDirection)
        {
            this.position = position;
            this.requireDirection = requireDirection;
        }
    }
    [SerializeField] private List<RoomsToCreate> roomsToCreates = new List<RoomsToCreate>();
    [SerializeField] private Dictionary<Vector2, Square> squares = new Dictionary<Vector2, Square>();

    private Coroutine createRoomCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        roomsCount = 1;
        DoorCreateRoom(Vector2.zero, (DoorsDirection)Random.Range(0, 4));
    }

    public void DoorCreateRoom(Vector2 roomPosition, DoorsDirection requireDirection)
    {
        roomsToCreates.Add(new RoomsToCreate(roomPosition, requireDirection));
        if (createRoomCoroutine == null)
            createRoomCoroutine = StartCoroutine(CreateRoomsIE());
    }
    //Check if the room exists and update
    private bool CheckRoom(DoorsDirection direction, Vector2 position)
    {
        if (direction == DoorsDirection.front && squares.ContainsKey(position + Vector2.up))
        {
            Square square;
            squares.TryGetValue(position + Vector2.up, out square);

            square.AddDoorDirection(DoorsDirection.back);
            return true;
        }
        if (direction == DoorsDirection.back && squares.ContainsKey(position + Vector2.down))
        {
            Square square;
            squares.TryGetValue(position + Vector2.down, out square);
            square.AddDoorDirection(DoorsDirection.front);
            return true;
        }

        if (direction == DoorsDirection.left && squares.ContainsKey(position + Vector2.left))
        {
            Square square;
            squares.TryGetValue(position + Vector2.left, out square);
            square.AddDoorDirection(DoorsDirection.right);
            return true;
        }

        if (direction == DoorsDirection.right && squares.ContainsKey(position + Vector2.right))
        {
            
            Square square;
            squares.TryGetValue(position + Vector2.right, out square);
            square.AddDoorDirection(DoorsDirection.left);
            return true;
        }

        return false;
    }
    //Check If The Room Exists And Return Necessary Doors
    private List<DoorsDirection> CheckRoomExist(Vector2 position)
    {
        List<DoorsDirection> direction = new List<DoorsDirection>();
        //Front
        if (squares.ContainsKey(position + Vector2.up))
        {
            Square square;
            squares.TryGetValue(position + Vector2.up, out square);

            if (square.VerificDoorExist(DoorsDirection.back))
                direction.Add(DoorsDirection.front);
        }
        else if (squares.ContainsKey(position + Vector2.down))
        {
            Square square;
            squares.TryGetValue(position + Vector2.down, out square);

            if (square.VerificDoorExist(DoorsDirection.front))
                direction.Add(DoorsDirection.back);
        }
        else if (squares.ContainsKey(position + Vector2.left))
        {
            Square square;
            squares.TryGetValue(position + Vector2.left, out square);

            if (square.VerificDoorExist(DoorsDirection.right))
                direction.Add(DoorsDirection.left);
        }
        else if (squares.ContainsKey(position + Vector2.right))
        {
            Square square;
            squares.TryGetValue(position + Vector2.right, out square);

            if (square.VerificDoorExist(DoorsDirection.left))
                direction.Add(DoorsDirection.right);
        }

        return direction;
    }
    IEnumerator CreateRoomsIE()
    {
        while (roomsToCreates.Count > 0)
        {
            int maxDoor = 0;

            List<DoorsDirection> requireDirection = new List<DoorsDirection>();

            Vector2 roomPosition = roomsToCreates[0].position;

            if (!squares.ContainsKey(roomPosition))
            {
                requireDirection.Add(roomsToCreates[0].requireDirection);

                requireDirection.AddRange(CheckRoomExist(roomPosition));
                if (!requireDirection.Contains(roomsToCreates[0].requireDirection))

                    Debug.Log((maxRooms - roomsCount) + "---" + roomsCount);
                maxDoor = Mathf.Clamp(maxRooms - roomsCount, 0, 4);

                if (maxDoor == 0)
                {
                    Debug.Log("GridManager->CreateRoom: Doors Count Error");
                }

                //0-Back
                //1-Front
                //2-Left
                //3-Right
                //All doors directions

                List<int> dT = new List<int>() { 0, 1, 2, 3 };
                //Actives directions
                List<int> d = new List<int>();

                requireDirection.ForEach((x) =>
                {
                    if (!d.Contains((int)x))
                        d.Add((int)x);
                    if (dT.Contains((int)x))
                        dT.Remove((int)x);
                });

                Debug.Log(maxDoor+"____"+ (maxDoor - d.Count)+ "____" + d.Count);
                for (int i = 0; i < maxDoor - d.Count; i++)
                {
                    float tryCreateDoor = Random.Range(0, 10);
                    if (tryCreateDoor > 0f)
                    {
                        int doorDiretion = dT[Random.Range(0, dT.Count)];
                        dT.Remove(doorDiretion);
                        d.Add(doorDiretion);
                        yield return new WaitForEndOfFrame();
                    }
                }

                List<DoorsDirection> finalDirections = new List<DoorsDirection>();
                d.ForEach((x) =>
                {
                    if (!CheckRoom((DoorsDirection)x, roomPosition))
                        roomsCount++;
                    finalDirections.Add((DoorsDirection)x);
                });

                Vector3 p = Vector3.zero;
                p.x = roomPosition.x;
                p.z = roomPosition.y;
                GameObject obj = Instantiate(gridSquare, (p * Square.squareScale * 10), Quaternion.identity, this.transform);
                Square tempSquare = obj.GetComponent<Square>();

                squares.Add(roomPosition, tempSquare);
                
                tempSquare.gridManager = this;
                tempSquare.StartGrid(finalDirections, roomPosition);
                //roomsCount += d.Count;
            }
            else
            {
                requireDirection.AddRange(CheckRoomExist(roomPosition));
                Square square;
                squares.TryGetValue(roomPosition, out square);
                requireDirection.ForEach((x) => square.AddDoorDirection(x));
                
            }
            roomsToCreates.Remove(roomsToCreates[0]);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        createRoomCoroutine = null;
    }
}

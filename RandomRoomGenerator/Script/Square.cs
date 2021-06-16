using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Square : MonoBehaviour
{
    public const int squareScale = 4;

    public GridManager gridManager;
    [SerializeField] private Vector2 position;

    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField]
    private List<GridManager.DoorsDirection> directions = new List<GridManager.DoorsDirection>();


    private void UpdateGrid(bool fristUpdate = false)
    {
        foreach (GridManager.DoorsDirection direction in directions)
        {
            if (direction == GridManager.DoorsDirection.front)
            {
                front.SetActive(true);
                if (fristUpdate)
                    gridManager.DoorCreateRoom((position + Vector2.up), GridManager.DoorsDirection.back);
            }
            if (direction == GridManager.DoorsDirection.back)
            {
                back.SetActive(true);
                if (fristUpdate)
                    gridManager.DoorCreateRoom((position + Vector2.down), GridManager.DoorsDirection.front);
            }
            if (direction == GridManager.DoorsDirection.left)
            {
                left.SetActive(true);
                if (fristUpdate)
                    gridManager.DoorCreateRoom((position + Vector2.left), GridManager.DoorsDirection.right);
            }
            if (direction == GridManager.DoorsDirection.right)
            {
                right.SetActive(true);
                if (fristUpdate)
                    gridManager.DoorCreateRoom((position + Vector2.right), GridManager.DoorsDirection.left);
            }

        }
    }
    public bool VerificDoorExist(GridManager.DoorsDirection direction)
    {
        return directions.Contains(direction);
    }
    public void AddDoorDirection(GridManager.DoorsDirection direction)
    {
        this.directions.Add(direction);

        UpdateGrid();
    }
    public void StartGrid(List<GridManager.DoorsDirection> directions, Vector2 position)
    {
        this.position = position;
        foreach (GridManager.DoorsDirection d in directions)
            this.directions.Add(d);

        UpdateGrid(true);
    }
}

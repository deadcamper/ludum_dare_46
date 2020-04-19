using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConwayCell : MonoBehaviour
{
    public enum CellState
    {
        None, //Used for cell recycling
        Alive,
        Dead,
        Dying,
        Creating
    }

    public Vector2Int conwayPosition;

    // Start is called before the first frame update
    public Color dangerColor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

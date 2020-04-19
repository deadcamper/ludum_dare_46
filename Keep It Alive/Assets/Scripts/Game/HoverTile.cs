using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HoverTile : MonoBehaviour
{
    public SpriteRenderer hoverTile;

    //public TileBase hoverTile;

    public Tilemap tilemap;

    public ConwayGame game;

    private Vector3Int lastTile = new Vector3Int(-100,-100, 0);

    // Update is called once per frame

    private void Start()
    {
        
    }
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        Vector3 vec = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = tilemap.WorldToCell(vec);

        Vector3 spritePos = tilemap.CellToWorld(cellPos) + new Vector3(0.5f,0.5f,0);

        hoverTile.transform.position = spritePos;
        hoverTile.enabled = game.gameOfLife.IsInBounds(cellPos);
    }
}

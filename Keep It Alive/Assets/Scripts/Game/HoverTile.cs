using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HoverTile : MonoBehaviour
{
    public TileBase hoverTile;

    public Tilemap tilemap;

    public ConwayGame game;

    private Vector3Int lastTile = new Vector3Int(-100,-100, 0);

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;

        Vector3 vec = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = tilemap.WorldToCell(vec);

        if (cellPos != lastTile)
        {
            tilemap.SwapTile(hoverTile, null);
            if (game.gameOfLife.IsInBounds(cellPos))
            {
                tilemap.SetTile(cellPos, hoverTile);
            }
            lastTile = cellPos;
        }
    }
}

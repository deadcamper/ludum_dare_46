using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConwayDirtDecorator : MonoBehaviour
{
    public ConwayGame game;

    public Tilemap dirtTileMap;

    public List<TileBase> tileBrushes;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTiles();
    }

    void UpdateTiles()
    {
        for (int x = game.gameOfLife.lowerLeftBound.x; x <= game.gameOfLife.upperRightBound.x; x++)
        {
            for (int y = game.gameOfLife.lowerLeftBound.y; y <= game.gameOfLife.upperRightBound.y; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);

                float randomValue = Random.value * Random.value;

                int index = Mathf.FloorToInt(randomValue * tileBrushes.Count);

                dirtTileMap.SetTile(cell, tileBrushes[index]);
            }
        }
    }
}

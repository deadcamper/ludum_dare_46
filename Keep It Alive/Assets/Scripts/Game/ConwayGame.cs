using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConwayGame : MonoBehaviour
{
    public ConwayGameOfLife gameOfLife;

    public Tilemap cropTileMap;

    public TileBase emptyCell = null;

    public List<TileBase> liveSeedStages;
    public List<TileBase> deadSeedStages;

    public float secondsBetweenSteps = 3.0f;

    private float lastStep = 0f;

    public int berriesCollected = 0;
    public int berriesRequired = 25;

    public int seeds = 3;
    public int totalSeeds = 100;

    public float TimeToNextStep { get { return Mathf.Max(0, secondsBetweenSteps - (Time.time - lastStep)); } }

    // Start is called before the first frame update
    void Start()
    {
        lastStep = Time.time;
        UpdateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;

            Vector3 vec = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3Int cellPos = cropTileMap.WorldToCell(vec);

            CheckPlayerToggleCell(cellPos);
        }

        while (Time.time > lastStep + secondsBetweenSteps)
        {
            lastStep += secondsBetweenSteps;
            ConwayDelta update = gameOfLife.NextStep();

            UpdateTiles();
        }
        
    }

    void CheckPlayerToggleCell(Vector3Int cellPos)
    {
        if (!gameOfLife.IsInBounds(cellPos))
            return;

        bool hasLife = gameOfLife.aliveCells.Contains(cellPos);

        // Clear of dead
        if (!hasLife && cropTileMap.GetTile(cellPos) != emptyCell)
        {
            cropTileMap.SetTile(cellPos, emptyCell);
            return;
        }

        if (hasLife)
        {
            TileBase lastTile = cropTileMap.GetTile(cellPos);
            PlayerToggleCell(cellPos);

            if (lastTile == liveSeedStages[liveSeedStages.Count-1])
            {
                seeds+=3; //BONUS SEED
                berriesCollected++;
            }
            else
            {
                seeds++;
            }
        }
        else if (!hasLife && seeds > 0)
        {
            PlayerToggleCell(cellPos);
            seeds--;
        }

        seeds = Mathf.Clamp(seeds,0,totalSeeds);
    }

    void PlayerToggleCell(Vector3Int cellPos)
    {
        PlayerSetCell(cellPos, !gameOfLife.aliveCells.Contains(cellPos));
    }

    bool PlayerSetCell(Vector3Int cellPos, bool isAlive)
    {
        bool change;
        if (isAlive)
        {
            change = gameOfLife.SetCell(cellPos, true);
            //change = gameOfLife.aliveCells.Add(new Vector3Int(cellPos.x, cellPos.y, 0));
            UpdateTile(cellPos);
        }
        else
        {
            change = gameOfLife.SetCell(cellPos, false);
            //change = gameOfLife.aliveCells.Remove(new Vector3Int(cellPos.x, cellPos.y, 0));
            cropTileMap.SetTile(cellPos, emptyCell);
        }

        return change;
    }

    void UpdateTile(Vector3Int cellPos)
    {
        bool isAlive = gameOfLife.aliveCells.Contains(cellPos);
        TileBase tile = cropTileMap.GetTile(cellPos);

        if (isAlive)
        {
            int index = Mathf.Min(gameOfLife.timeAlive[cellPos] - 1, liveSeedStages.Count - 1);
            cropTileMap.SetTile(cellPos, liveSeedStages[index]);
        }
        else
        {
            if (tile != emptyCell && !deadSeedStages.Contains(tile))
            {
                if (liveSeedStages.Contains(tile))
                {
                    int index = liveSeedStages.IndexOf(tile);
                    cropTileMap.SetTile(cellPos, deadSeedStages[index]);
                }
                else
                {
                    cropTileMap.SetTile(cellPos, emptyCell);
                }
            }
        }
    }

    void UpdateTiles()
    {
        for (int x = gameOfLife.lowerLeftBound.x; x <= gameOfLife.upperRightBound.x; x++)
        {
            for (int y = gameOfLife.lowerLeftBound.y; y <= gameOfLife.upperRightBound.y; y++)
            {
                Vector3Int cell = new Vector3Int(x,y,0);
                UpdateTile(cell);
            }
        }
    }
}

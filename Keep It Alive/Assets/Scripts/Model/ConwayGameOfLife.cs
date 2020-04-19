using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ConwayGameOfLife
{
    public int numNeighborsToLive = 2;

    public int numNeighborsToSpawn = 3;

    public int numNeighborsToDie = 4;

    public Vector2Int lowerLeftBound = new Vector2Int();
    public Vector2Int upperRightBound = new Vector2Int();

    // The current state
    public HashSet<Vector3Int> aliveCells = new HashSet<Vector3Int>();

    public Dictionary<Vector3Int, int> timeAlive = new Dictionary<Vector3Int, int>();

    public bool IsInBounds(Vector3Int cell)
    {
        if (cell.x > upperRightBound.x || cell.x < lowerLeftBound.x)
            return false;
        if (cell.y > upperRightBound.y || cell.y < lowerLeftBound.y)
            return false;

        return true;
    }

    public bool SetCell(Vector3Int cell, bool isAlive)
    {
        if (!IsInBounds(cell))
            return false;

        bool change = false;
        if (aliveCells.Contains(cell) && !isAlive)
        {
            aliveCells.Remove(cell);
            timeAlive.Remove(cell);
            change = true;
        }
        else if(!aliveCells.Contains(cell) && isAlive)
        {
            aliveCells.Add(cell);
            timeAlive[cell] = 1;
            change = true;
        }
        return change;
    }

    public ConwayDelta NextStep()
    {
        Dictionary<Vector3Int, int> neighborhoodWorksheet = EvalNeighborCounts();

        ConwayDelta result;

        ApplyNeighborMathToCells(aliveCells, neighborhoodWorksheet, out result);

        ApplyLiveUpdate(result);

        return result;
    }

    private Dictionary<Vector3Int, int> EvalNeighborCounts()
    {
        Dictionary<Vector3Int, int> neighborhoodWorksheet = new Dictionary<Vector3Int, int>();

        foreach (Vector3Int point in aliveCells)
        {
            for (int u = -1; u <= 1; u++)
            {

                for (int v = -1; v <= 1; v++)
                {
                    Vector3Int testPoint = new Vector3Int(point.x + u, point.y + v, 0);

                    if (!IsInBounds(testPoint))
                    {
                        continue;
                    }

                    //Save time, skip checks on previously checked values.
                    if (neighborhoodWorksheet.ContainsKey(testPoint))
                    {
                        continue;
                    }

                    int neighborCount = GetLiveNeighborCount(testPoint);
                    neighborhoodWorksheet[testPoint] = neighborCount;
                }
            }
        }

        return neighborhoodWorksheet;
    }

    private int GetLiveNeighborCount(Vector3Int point)
    {
        int count = 0;
        for (int u = -1; u <= 1; u++)
        {
            for (int v = -1; v <= 1; v++)
            {
                // Skip checking oneself
                if (u == 0 && v == 0)
                {
                    continue;
                }

                // Known bug: By doing the math directly, simulation loops over Long.MAX_VALUE into Long.MIN_VALUE and back.
                Vector3Int testPoint = new Vector3Int(point.x + u, point.y + v, 0);
                if (aliveCells.Contains(testPoint))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private bool ApplyNeighborMathToCells(HashSet<Vector3Int> aliveCells, Dictionary<Vector3Int, int> neighborhood, out ConwayDelta deltas)
    {
        bool change = false;

        deltas = new ConwayDelta();
        foreach (KeyValuePair<Vector3Int, int> entry in neighborhood)
        {
            bool hasChanged = false;
            Vector3Int cell = entry.Key;
            int liveNeighbors = entry.Value;

            if (liveNeighbors >= numNeighborsToDie) // Death by suffocation case
            {
                hasChanged = aliveCells.Remove(cell);
                if (hasChanged)
                    deltas.killed.Add(cell);
            }
            else if (liveNeighbors >= numNeighborsToSpawn) // Create Life case
            {
                hasChanged = aliveCells.Add(cell);
                if (hasChanged)
                    deltas.created.Add(cell);
            }
            else if (liveNeighbors < numNeighborsToLive) // Death by "starvation" case
            {
                hasChanged = aliveCells.Remove(cell);
                if (hasChanged)
                    deltas.killed.Add(cell);
            }
            change |= hasChanged;
        }

        return change;
    }

    private void ApplyLiveUpdate(ConwayDelta deltas)
    {
        Dictionary<Vector3Int, int> newTimeAlive = timeAlive.Where(pair => !deltas.killed.Contains(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value + 1);

        foreach(Vector3Int newCell in deltas.created)
        {
            newTimeAlive[newCell] = 1;
        }

        timeAlive = newTimeAlive;

    }


}

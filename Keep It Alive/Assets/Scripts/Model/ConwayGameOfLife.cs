using System.Collections.Generic;
using UnityEngine;

public class ConwayGameOfLife
{
    public int numNeighborsToLive = 2;

    public int numNeighborsToSpawn = 3;

    public int numNeighborsToDie = 4;

    // The current state
    public HashSet<Vector2Int> aliveCells = new HashSet<Vector2Int>();

    private Dictionary<Vector2Int, bool> testedSpaces = new Dictionary<Vector2Int, bool>(); // used in each simulation to track tested spaces.

    public void NextStep()
    {
        Dictionary<Vector2Int, int> neighborhoodWorksheet = EvalNeighborCounts();

        ApplyNeighborMathToCells(aliveCells, neighborhoodWorksheet, out _);
    }
    public ConwayDelta SimulateStep()
    {
        Dictionary<Vector2Int, int> neighborhoodWorksheet = EvalNeighborCounts();

        ConwayDelta result;

        ApplyNeighborMathToCells(aliveCells, neighborhoodWorksheet, out result);

        return result;
    }

    private Dictionary<Vector2Int, int> EvalNeighborCounts()
    {
        Dictionary<Vector2Int, int> simulationWorksheet = new Dictionary<Vector2Int, int>();

        foreach (Vector2Int point in aliveCells)
        {
            for (int u = -1; u <= 1; u++)
            {
                for (int v = -1; v <= 1; v++)
                {
                    Vector2Int testPoint = new Vector2Int(point.x + u, point.y + v);

                    //Save time, skip checks on previously checked values.
                    if (simulationWorksheet.ContainsKey(testPoint))
                    {
                        continue;
                    }

                    int neighborCount = GetLiveNeighborCount(testPoint);
                    simulationWorksheet[testPoint] = neighborCount;
                }
            }
        }

        return simulationWorksheet;
    }

    private int GetLiveNeighborCount(Vector2Int point)
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
                Vector2Int testPoint = new Vector2Int(point.x + u, point.y + v);
                if (aliveCells.Contains(testPoint))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private bool ApplyNeighborMathToCells(HashSet<Vector2Int> aliveCells, Dictionary<Vector2Int, int> neighborhood, out ConwayDelta deltas)
    {
        bool change = false;

        deltas = new ConwayDelta();
        foreach (KeyValuePair<Vector2Int, int> entry in neighborhood)
        {
            bool hasChanged = false;
            Vector2Int cell = entry.Key;
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


}

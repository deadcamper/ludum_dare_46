using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bare bones structure to track which cells 'changed' between steps.
/// </summary>
public class ConwayDelta
{
    public HashSet<Vector2Int> created = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> killed = new HashSet<Vector2Int>();
}

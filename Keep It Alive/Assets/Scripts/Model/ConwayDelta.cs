using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bare bones structure to track which cells 'changed' between steps.
/// </summary>
public class ConwayDelta
{
    public HashSet<Vector3Int> created = new HashSet<Vector3Int>();
    public HashSet<Vector3Int> killed = new HashSet<Vector3Int>();
}

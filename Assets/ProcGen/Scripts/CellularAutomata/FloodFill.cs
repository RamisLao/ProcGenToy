using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to apply the floodfill algorithm to the Grid.
/// </summary>
public static class FloodFill
{
    // Use the FloodFill algorithm to return a multidimensional array of region ids
    public static int[,] FindDistinctRegions(Cell[][] cellGrid, CellStateData endState)
    {
        int[,] regions = new int[cellGrid.Length, cellGrid[0].Length];
        int currentRegionIdx = 1;

        for (int x = 0; x < cellGrid.Length; x++)
        {
            for (int y = 0; y < cellGrid[0].Length; y++)
            {
                FloodFillRecursive(x, y, currentRegionIdx, ref regions, cellGrid, endState);
                currentRegionIdx++;
            }
        }

        return regions;
    }

    // Recursive function that uses Depth-First Search to get all distinct regions that are bordered by endState
    public static void FloodFillRecursive(int x, int y, int regionIdx, ref int[,] regions, Cell[][] cellGrid, CellStateData endState)
    {

        if (cellGrid[x][y].StateData.ECellState == endState.ECellState || regions[x, y] != 0)
        {
            return;
        }

        regions[x, y] = regionIdx;

        if (x > 0)
            FloodFillRecursive(x - 1, y, regionIdx, ref regions, cellGrid, endState);
        if (x < cellGrid.Length - 1)
            FloodFillRecursive(x + 1, y, regionIdx, ref regions, cellGrid, endState);
        if (y > 0)
            FloodFillRecursive(x, y - 1, regionIdx, ref regions, cellGrid, endState);
        if (y < cellGrid[0].Length - 1)
            FloodFillRecursive(x, y + 1, regionIdx, ref regions, cellGrid, endState);
    }

    // Get a multidimensional array of region ids and return a dictionary mapping region ids with their size in number of cells
    public static Dictionary<int, int> GetSizeOfRegions(int[,] regions)
    {
        Dictionary<int, int> regionsSizes = new();
        for (int x = 0; x < regions.GetLength(0); x++)
        {
            for (int y = 0; y < regions.GetLength(1); y++)
            {

                if (regions[x, y] == 0) continue;

                if (!regionsSizes.ContainsKey(regions[x, y]))
                {
                    regionsSizes.Add(regions[x, y], 1);
                }
                else
                {
                    regionsSizes[regions[x, y]] += 1;
                }
            }
        }

        return regionsSizes;
    }

    // Return the biggest region from a map of region ids to number of cells
    public static int GetBiggestRegionIdx(Dictionary<int, int> regionsSizes)
    {
        int max = 0;
        int maxKey = 0;
        foreach (KeyValuePair<int, int> values in regionsSizes)
        {
            if (values.Value > max)
            {
                max = values.Value;
                maxKey = values.Key;
            }
        }

        return maxKey;
    }

    // All cells that do not have the same id as maxKey get filled with endState
    public static Cell[][] FillDisconnectedHoles(Cell[][] grid, ref int[,] regions, int maxKey, CellStateData endState)
    {
        for (int x = 0; x < grid.Length; x++)
        {
            for (int y = 0; y < grid[0].Length; y++)
            {
                if (regions[x, y] != maxKey)
                {
                    grid[x][y].StateData = endState;
                    regions[x, y] = 0;
                }
            }
        }

        return grid;
    }
}

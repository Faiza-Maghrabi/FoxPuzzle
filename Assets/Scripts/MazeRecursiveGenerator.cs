using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallState
{
    LEFT = 1,
    RIGHT = 2,
    UP = 4,
    DOWN = 8,

}

public static class MazeRecursiveGenerator
{
    public static WallState[,] Generate(int width, int height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; i<height; ++j)
            {
                maze[i, j] = initial; // 1111
            }
        }

        return maze;
    }
}

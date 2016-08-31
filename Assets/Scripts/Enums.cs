using UnityEngine;
using System.Collections;
public enum SkyMaterial { DEFAULT, STONE, DIRT, GRASS, AIR }
public enum Direction { UP, DOWN, NORTH, SOUTH, WEST, EAST }
public static class SkyMaterialExtensions {

    public static SkyMaterialSetting getSetting(this SkyMaterial m) {
        return JsonHandler.getData<SkyMaterialSetting>(JsonHandler.JsonFileType.SKY_MATERIAL, m.ToString());
    }
    public static bool isOpaque(this SkyMaterial m) {
        switch (m) {
            case SkyMaterial.AIR:
                {
                    return true;
                }
        }
        return false;
    }
    public static float getBreakingSpeed(this SkyMaterial m) {
        return 1;
    }
    public static Vector3 toVector(this Direction d) {
        switch (d) {
            case Direction.UP:
                {
                    return new Vector3(0, 1, 0);
                }
            case Direction.DOWN:
                {
                    return new Vector3(0, -1, 0);
                }
            case Direction.EAST:
                {
                    return new Vector3(1, 0, 0);
                }
            case Direction.WEST:
                {
                    return new Vector3(-1, 0, 0);
                }
            case Direction.NORTH:
                {
                    return new Vector3(0, 0, 1);
                }
            case Direction.SOUTH:
                {
                    return new Vector3(0, 0, -1);
                }

        }
        return new Vector3(0, 0, 0);
    }
    public static Direction getRightHandSide(this Direction d) {
        switch (d) {
            case Direction.UP:
                {
                    return Direction.EAST;
                }
            case Direction.DOWN:
                {
                    return Direction.WEST;
                }
            case Direction.EAST:
                {
                    return Direction.SOUTH;
                }
            case Direction.WEST:
                {
                    return Direction.NORTH;
                }
            case Direction.NORTH:
                {
                    return Direction.EAST;
                }
            case Direction.SOUTH:
                {
                    return Direction.WEST;
                }
        }
        return Direction.WEST;
    }
    public static Direction getUpSide(this Direction d) {
        switch (d) {
            case Direction.UP:
                {
                    return Direction.NORTH;
                }
            case Direction.DOWN:
                {
                    return Direction.SOUTH;
                }
            case Direction.EAST:
                {
                    return Direction.UP;
                }
            case Direction.WEST:
                {
                    return Direction.UP;
                }
            case Direction.NORTH:
                {
                    return Direction.UP;
                }
            case Direction.SOUTH:
                {
                    return Direction.UP;
                }
        }
        return Direction.WEST;
    }
    public static Direction getOpposit(this Direction d) {
        switch (d) {
            case Direction.UP:
                {
                    return Direction.DOWN;
                }
            case Direction.DOWN:
                {
                    return Direction.UP;
                }
            case Direction.EAST:
                {
                    return Direction.WEST;
                }
            case Direction.WEST:
                {
                    return Direction.EAST;
                }
            case Direction.NORTH:
                {
                    return Direction.SOUTH;
                }
            case Direction.SOUTH:
                {
                    return Direction.NORTH;
                }
        }
        return Direction.WEST;
    }
}
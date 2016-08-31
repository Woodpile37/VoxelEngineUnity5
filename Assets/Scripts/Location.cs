using UnityEngine;
using System.Collections;

public class Location {

    Vector3 vector;

    Vector3 direction = Vector3.zero;

    public Location(float x, float y, float z) {
        vector = new Vector3(x, y, z);
    }
    public Location(Vector3 vector3) {
        vector = vector3;
    }
    public void setDirection(Vector3 vector) {
        direction = vector;
    }
    public Vector3 getDirection() {
        return new Vector3(direction.x, direction.y, direction.z);
    }
    public float getX() {
        return vector.x;
    }
    public float getY() {
        return vector.y;
    }
    public float getZ() {
        return vector.z;
    }
    public Vector3 getChunkPosition() {
        int x = (int)Mathf.Floor(getX() / Chunk.xSize);
        int y = (int)Mathf.Floor(getY() / Chunk.ySize);
        int z = (int)Mathf.Floor(getZ() / Chunk.zSize);
        return new Vector3(x, y, z);
    }
    public Chunk getChunk() {
        return GameManager.getChunk(getChunkPosition());
    }
    public Location getBlockLocation() {
        int x = (int)Mathf.FloorToInt(getX());
        int y = (int)Mathf.FloorToInt(getY());
        int z = (int)Mathf.FloorToInt(getZ());
        return new Location(x, y, z);
    }
    public Vector3 getPositionInChunk() {
        int x = (int)Mathf.Floor(getX() / Chunk.xSize);
        int y = (int)Mathf.Floor(getY() / Chunk.ySize);
        int z = (int)Mathf.Floor(getZ() / Chunk.zSize);
        x *= Chunk.xSize;
        y *= Chunk.ySize;
        z *= Chunk.zSize;
        int xx = Mathf.FloorToInt(getX()) - x;
        int yy = Mathf.FloorToInt(getY()) - y;
        int zz = Mathf.FloorToInt(getZ()) - z;
        return new Vector3(xx, yy, zz);
    }
    public Block getBlock() {
        Chunk chunk = getChunk();
        if (chunk == null) {
            Debug.Log("chunk not found, generating new one at: " + getX() + "  " + getY() + "  " + getZ());
        }
        Vector3 v = getPositionInChunk();
        return chunk.getBlock(v);
    }
    public Vector3 toVector() {
        return new Vector3(vector.x, vector.y, vector.z);
    }
    public Location getPointedBlockLocation(Vector3 d) {
        Vector3 v = vector;// + (vector * -0.001f);
        v += (d * 0.001f);
        Location l = new Location(v).getBlockLocation();
        return l;
    }
    public Direction getNearestDirectionInBlock(Block block) {
        Vector3 b = block.getLocation().toVector();
        Vector3 l = vector;
        Vector3 v = l - b;
        //Debug.Log("" + l + " - " + b + " = " + v);
        if (v.x <= (0.001f)) {
            return Direction.WEST;
        } else if (v.x >= (0.999f)) {
            return Direction.EAST;
        } else if (v.y <= (0.001f)) {
            return Direction.DOWN;
        } else if (v.y >= (0.999f)) {
            return Direction.UP;
        } else if (v.z <= (0.001f)) {
            return Direction.SOUTH;
        } else if (v.z >= (0.999f)) {
            return Direction.NORTH;
        }
        Debug.LogError("failed to find direction at " + v);
        return Direction.DOWN;
    }
    public Location clone() {
        return new Location(vector.x, vector.y, vector.z);
    }

    public Location add(float x, float y, float z) {
        vector.x = vector.x + x;
        vector.y = vector.y + y;
        vector.z = vector.z + z;
        return this;
    }
    public Location add(Vector3 vector3) {
        add(vector3.x, vector3.y, vector3.z);
        return this;
    }
    public Location add(Direction direction) {
        return add(direction.toVector());
    }
}

using UnityEngine;
using System.Collections;
using System;

public class Block {

    SkyMaterial material;

    Chunk chunk;

    Vector3 locInChunk;

    Location worldLocation;

    BlockFace UpBlockFace;

    BlockFace DownBlockFace;

    BlockFace NorthBlockFace;

    BlockFace SouthBlockFace;

    BlockFace EastBlockFace;

    BlockFace WestBlockFace;

    public Vector2 uvPoint0 = new Vector2(0, 1);
    public Vector2 uvPoint1 = new Vector2(1, 1);
    public Vector2 uvPoint2 = new Vector2(1, 0);
    public Vector2 uvPoint3 = new Vector2(0, 0);

    float durability = 100;

    Material mat;

    public Block(Chunk chunk, Vector3 locInChunk, SkyMaterial material) {
        this.chunk = chunk;
        this.locInChunk = locInChunk;
        this.material = material;
        Vector3 chunkLoc = chunk.getChunkLocation();
        worldLocation = new Location(new Vector3(locInChunk.x + (chunkLoc.x * Chunk.xSize), locInChunk.y + (chunkLoc.y * Chunk.ySize), locInChunk.z + (chunkLoc.z * Chunk.zSize)));
        this.UpBlockFace = new BlockFace(this, Direction.UP);
        this.DownBlockFace = new BlockFace(this, Direction.DOWN);
        this.NorthBlockFace = new BlockFace(this, Direction.NORTH);
        this.SouthBlockFace = new BlockFace(this, Direction.SOUTH);
        this.EastBlockFace = new BlockFace(this, Direction.EAST);
        this.WestBlockFace = new BlockFace(this, Direction.WEST);

        mat = Resources.Load("Material\\" + material.ToString()) as Material;
    }
    public Block getRelative(Direction direction) {
        return getLocation().add(direction.toVector()).getBlock();
    }
    public Block getRelative(Vector3 vector) {
        Location l = new Location(this.getLocation().add(vector).toVector());
        return l.getBlock();
    }
    public Chunk getChunk() {
        return chunk;
    }
    public Location getLocation() {
        return worldLocation.clone();
    }
    public SkyMaterial getType() {
        return material;
    }
    public BlockFace getBlockFace(Direction direction) {
        if (direction == Direction.UP) {
            return UpBlockFace;
        } else if (direction == Direction.DOWN) {
            return DownBlockFace;
        } else if (direction == Direction.NORTH) {
            return NorthBlockFace;
        } else if (direction == Direction.SOUTH) {
            return SouthBlockFace;
        } else if (direction == Direction.WEST) {
            return WestBlockFace;
        } else if (direction == Direction.EAST) {
            return EastBlockFace;
        }
        return null;
    }

    internal void setType(SkyMaterial sm) {
        material = sm;
        getChunk().putToRenderQueue();
    }

    public Vector2 getUVPoint0() {
        return uvPoint0;
    }
    public Vector2 getUVPoint1() {
        return uvPoint1;
    }
    public Vector2 getUVPoint2() {
        return uvPoint2;
    }
    public Vector2 getUVPoint3() {
        return uvPoint3;
    }
    public Material getMaterial() {
        return mat;
    }
    public Vector3 getPositionInChunk() {
        return new Vector3(locInChunk.x, locInChunk.y, locInChunk.z);
    }
}

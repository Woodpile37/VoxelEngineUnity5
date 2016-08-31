using UnityEngine;
using System.Collections;

public class BlockFace {
    Direction direction;

    public Vector3 node0;

    public Vector3 node1;

    public Vector3 node2;

    public Vector3 node3;

    public int node0Index;

    public int node1Index;

    public int node2Index;

    public int node3Index;

    public bool shouldRender = false;

    public bool isRendered = false;

    Block block;

    public BlockFace(Block block, Direction direction) {
        this.direction = direction;
        this.block = block;
        if (direction == Direction.UP) {
            node0 = new Vector3(0, 1, 1);
            node1 = new Vector3(1, 1, 1);
            node2 = new Vector3(1, 1, 0);
            node3 = new Vector3(0, 1, 0);
        } else if (direction == Direction.DOWN) {
            node0 = new Vector3(0, 0, 0);
            node1 = new Vector3(1, 0, 0);
            node2 = new Vector3(1, 0, 1);
            node3 = new Vector3(0, 0, 1);
        } else if (direction == Direction.NORTH) {
            node0 = new Vector3(1, 1, 1);
            node1 = new Vector3(0, 1, 1);
            node2 = new Vector3(0, 0, 1);
            node3 = new Vector3(1, 0, 1);
        } else if (direction == Direction.SOUTH) {
            node0 = new Vector3(0, 1, 0);
            node1 = new Vector3(1, 1, 0);
            node2 = new Vector3(1, 0, 0);
            node3 = new Vector3(0, 0, 0);
        } else if (direction == Direction.EAST) {
            node0 = new Vector3(1, 1, 0);
            node1 = new Vector3(1, 1, 1);
            node2 = new Vector3(1, 0, 1);
            node3 = new Vector3(1, 0, 0);
        } else if (direction == Direction.WEST) {
            node0 = new Vector3(0, 1, 1);
            node1 = new Vector3(0, 1, 0);
            node2 = new Vector3(0, 0, 0);
            node3 = new Vector3(0, 0, 1);
        }
        //         if (direction == Direction.DOWN) {
        //             node0 = new Vector3(0, 0, 0);
        //             node1 = new Vector3(1, 0, 0);
        //             node2 = new Vector3(1, 0, 1);
        //             node3 = new Vector3(0, 0, 1);
        //         } else if (direction == Direction.UP) {
        //             node0 = new Vector3(0, 1, 0);
        //             node1 = new Vector3(0, 1, 1);
        //             node2 = new Vector3(1, 1, 1);
        //             node3 = new Vector3(1, 1, 0);
        //         } else if (direction == Direction.NORTH) {
        //             node0 = new Vector3(1, 1, 1);
        //             node1 = new Vector3(0, 1, 1);
        //             node2 = new Vector3(0, 0, 1);
        //             node3 = new Vector3(1, 0, 1);
        //         } else if (direction == Direction.SOUTH) {
        //             node0 = new Vector3(0, 1, 0);
        //             node1 = new Vector3(1, 1, 0);
        //             node2 = new Vector3(1, 0, 0);
        //             node3 = new Vector3(0, 0, 0);
        //         } else if (direction == Direction.EAST) {
        //             node0 = new Vector3(1, 1, 0);
        //             node1 = new Vector3(1, 1, 1);
        //             node2 = new Vector3(1, 0, 1);
        //             node3 = new Vector3(1, 0, 0);
        //         } else if (direction == Direction.WEST) {
        //             node0 = new Vector3(0, 1, 1);
        //             node1 = new Vector3(0, 1, 0);
        //             node2 = new Vector3(0, 0, 0);
        //             node3 = new Vector3(0, 0, 1);
        //         }
        node0 = block.getLocation().add(node0).toVector();
        node1 = block.getLocation().add(node1).toVector();
        node2 = block.getLocation().add(node2).toVector();
        node3 = block.getLocation().add(node3).toVector();
    }
    public bool isAtAdgeOfChunk() {
        Vector3 v = block.getPositionInChunk();
        //Debug.Log(v.ToString() + "   " + direction.ToString());
        if (v.y == 0 && direction == Direction.DOWN) {
            return true;
        } else if (v.y == Chunk.ySize - 1 && direction == Direction.UP) {
            return true;
        } else if (v.x == Chunk.xSize - 1 && direction == Direction.EAST) {
            return true;
        } else if (v.x == 0 && direction == Direction.WEST) {
            return true;
        } else if (v.z == Chunk.zSize - 1 && direction == Direction.NORTH) {
            return true;
        } else if (v.z == 0 && direction == Direction.SOUTH) {
            return true;
        }
        return false;
    }
    public Block getBlock() {
        return block;
    }
    public BlockFace getRightHandSide() {
        return getBlock().getRelative(direction.getRightHandSide()).getBlockFace(direction);
    }
    public BlockFace getUpSide() {
        return getBlock().getRelative(direction.getUpSide()).getBlockFace(direction);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RenderHandler {

    static List<Chunk> pendingChunksToRender = new List<Chunk>();

    public static void renderOneChunk() {
        if (pendingChunksToRender.Count > 0) {
            Chunk chunk = pendingChunksToRender[0];
            pendingChunksToRender.RemoveAt(0);
            chunk.render();
        }
    }

    public static void putChunkToRenderQueue(Chunk chunk) {
        if (pendingChunksToRender.Contains(chunk) == false) {
            pendingChunksToRender.Add(chunk);
        }
    }

    public static Mesh createMesh(Chunk chunk) {
        Mesh mesh = chunk.mesh;
        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();

        // List<int> triangles = new List<int>();

        List<Vector2> uv = new List<Vector2>();
        String str = "";
        for (int x = 0; x < Chunk.xSize; x++) {
            for (int y = 0; y < Chunk.ySize; y++) {
                for (int z = 0; z < Chunk.zSize; z++) {
                    Block block = chunk.getBlock(x, y, z);
                    if (block.getType().Equals(SkyMaterial.AIR) == false) {
                        foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
                            BlockFace blockFace = block.getBlockFace(direction);
                            Boolean render = false;
                            if (blockFace.isAtAdgeOfChunk()) {
                                str += x + " " + y + " " + z + "  isAtAdge\n";
                                render = true;
                            } else {
                                Block blockNextTo = block.getRelative(direction);
                                Location l = blockNextTo.getLocation();
                                str += x + " " + y + " " + z + "  " + blockNextTo.getType().ToString() + " isOpaque : " + blockNextTo.getType().getSetting().isOpaque() +
                                  l.getX() + " " + l.getY() + " " + l.getZ() + "  " + "\n";
                                if (blockNextTo.getType().getSetting().isOpaque()) {
                                    render = true;
                                }
                            }
                            if (render) {
                                int startIndex = vertices.Count;
                                vertices.Add(blockFace.node0);
                                vertices.Add(blockFace.node1);
                                vertices.Add(blockFace.node2);
                                vertices.Add(blockFace.node3);

                                List<int> triangles = getMaterialTriangles(chunk, block.getType());

                                triangles.Add(startIndex + 1);
                                triangles.Add(startIndex + 2);
                                triangles.Add(startIndex);

                                triangles.Add(startIndex + 2);
                                triangles.Add(startIndex + 3);
                                triangles.Add(startIndex);

                                uv.Add(block.getUVPoint0());
                                uv.Add(block.getUVPoint1());
                                uv.Add(block.getUVPoint2());
                                uv.Add(block.getUVPoint3());
                            }
                        }
                    }
                }
            }
        }
        Debug.Log(str);
        mesh.SetVertices(vertices);
        mesh.uv = uv.ToArray();
        int i = 0;
        List<Material> materials = new List<Material>();
        mesh.subMeshCount = chunk.materialTriangles.Count;
        foreach (SkyMaterial sm in chunk.materialTriangles.Keys) {
            List<int> triangles = getMaterialTriangles(chunk, sm);
            mesh.SetTriangles(triangles.ToArray(), i);
            Material mat = sm.getSetting().getMaterial();
            materials.Add(mat);
            i++;
        }
        chunk.meshRenderer.materials = materials.ToArray();

        return mesh;
    }

    public static Mesh createMesh2(Chunk chunk) {
        Mesh mesh = new Mesh();
        mesh.Clear();
        List<Vector3> vertices = new List<Vector3>();

        // List<int> triangles = new List<int>();

        List<Vector2> uv = new List<Vector2>();
        //String str = "";
        foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
            for (int x = 0; x < Chunk.xSize; x++) {
                for (int y = 0; y < Chunk.ySize; y++) {
                    for (int z = 0; z < Chunk.zSize; z++) {
                        Block block = chunk.getBlock(x, y, z);
                        bool render = false;
                        BlockFace blockFace = block.getBlockFace(direction);
                        if (block.getType().Equals(SkyMaterial.AIR) == false) {
                            if (blockFace.isAtAdgeOfChunk()) {
                                render = true;
                            } else {
                                Block blockNextTo = block.getRelative(direction);
                                if (blockNextTo.getType().getSetting().isOpaque()) {
                                    render = true;
                                }
                            }
                        }
                        blockFace.shouldRender = render;
                        blockFace.isRendered = false;
                    }
                }
            }
        }

        foreach (Direction d in Enum.GetValues(typeof(Direction))) {

            for (int a = 0; a < Chunk.ySize; a++) {
                for (int b = 0; b < Chunk.xSize; b++) {
                    for (int c = 0; c < Chunk.zSize; c++) {
                        int x = a;
                        int y = b;
                        int z = c;
                        if (d.Equals(Direction.WEST) || d.Equals(Direction.EAST)) {
                            x = a;
                            y = c;
                            z = b;
                        } else if (d.Equals(Direction.NORTH) || d.Equals(Direction.SOUTH)) {
                            x = c;
                            y = b;
                            z = a;
                        } else if (d.Equals(Direction.UP) || d.Equals(Direction.DOWN)) {
                            x = c;
                            y = a;
                            z = b;
                        }
                        Block block = chunk.getBlock(x, y, z);
                        if (block.getType().Equals(SkyMaterial.AIR) == false) {
                            BlockFace startBlockFace = block.getBlockFace(d);
                            if (startBlockFace.isRendered == false && startBlockFace.shouldRender) {
                                Block lastBlock = block;
                                BlockFace lastBlockFace = block.getBlockFace(d);

                                int blockCountZ = 1;
                                int blockCountX = 1;
                                int currentZ = z;
                                while (currentZ < Chunk.zSize - 1) {
                                    currentZ++;
                                    Block lb = chunk.getBlock(x, y, currentZ);
                                    BlockFace lbf = lb.getBlockFace(d);
                                    if (canRenderTogether(block, lb, d)) {
                                        lbf.isRendered = true;
                                        lastBlock = lb;
                                        lastBlockFace = lbf;
                                        blockCountZ++;
                                    } else {
                                        break;
                                    }
                                }
                                int currentX = x;
                                while (currentX < Chunk.xSize - 1) {
                                    currentX++;
                                    Block bb = lastBlock;
                                    bool canRender = true;
                                    //String s = "";
                                    for (int zz = 0; zz <= blockCountZ - 1; zz++) {
                                        bb = chunk.getBlock(currentX, y, z + zz);
                                        if (canRenderTogether(block, bb, d)) {
                                            //s += block.getLocation().toVector().ToString() + "  " + bb.getLocation().toVector().ToString() + " " + block.getType().ToString() + "  " + bb.getType().ToString() + "\n";
                                        } else {
                                            canRender = false;
                                            break;
                                        }
                                    }
                                    //str += s;
                                    if (canRender) {
                                        for (int zz = 0; zz <= blockCountZ - 1; zz++) {
                                            chunk.getBlock(currentX, y, z + zz).getBlockFace(d).isRendered = true;
                                        }
                                        lastBlock = bb;
                                        lastBlockFace = lastBlock.getBlockFace(d);
                                        blockCountX++;
                                    } else {
                                        break;
                                    }
                                }

                                currentX = x + blockCountX - 1;
                                currentZ = z + blockCountZ - 1;
                                BlockFace c0bf = null;
                                BlockFace c1bf = null;
                                BlockFace c2bf = null;
                                BlockFace c3bf = null;
                                if (d.Equals(Direction.WEST)) {
                                    c0bf = chunk.getBlock(x, y, currentZ).getBlockFace(d);
                                    c1bf = chunk.getBlock(x, y, z).getBlockFace(d);
                                    c2bf = chunk.getBlock(currentX, y, z).getBlockFace(d);
                                    c3bf = chunk.getBlock(currentX, y, currentZ).getBlockFace(d);
                                    uv.Add(new Vector2(0, blockCountX));
                                    uv.Add(new Vector2(blockCountZ, blockCountX));
                                    uv.Add(new Vector2(blockCountZ, 0));
                                    uv.Add(new Vector2(0, 0));
                                } else if (d.Equals(Direction.EAST)) {
                                    c0bf = chunk.getBlock(currentX, y, z).getBlockFace(d);
                                    c1bf = chunk.getBlock(currentX, y, currentZ).getBlockFace(d);
                                    c2bf = chunk.getBlock(x, y, currentZ).getBlockFace(d);
                                    c3bf = chunk.getBlock(x, y, z).getBlockFace(d);
                                    uv.Add(new Vector2(0, blockCountX));
                                    uv.Add(new Vector2(blockCountZ, blockCountX));
                                    uv.Add(new Vector2(blockCountZ, 0));
                                    uv.Add(new Vector2(0, 0));
                                } else if (d.Equals(Direction.SOUTH)) {
                                    c0bf = chunk.getBlock(x, y, currentZ).getBlockFace(d);
                                    c1bf = chunk.getBlock(currentX, y, currentZ).getBlockFace(d);
                                    c2bf = chunk.getBlock(currentX, y, z).getBlockFace(d);
                                    c3bf = chunk.getBlock(x, y, z).getBlockFace(d);
                                    uv.Add(new Vector2(0, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, 0));
                                    uv.Add(new Vector2(0, 0));
                                } else if (d.Equals(Direction.NORTH)) {
                                    c0bf = chunk.getBlock(currentX, y, currentZ).getBlockFace(d);
                                    c1bf = chunk.getBlock(x, y, currentZ).getBlockFace(d);
                                    c2bf = chunk.getBlock(x, y, z).getBlockFace(d);
                                    c3bf = chunk.getBlock(currentX, y, z).getBlockFace(d);
                                    uv.Add(new Vector2(0, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, 0));
                                    uv.Add(new Vector2(0, 0));
                                } else if (d.Equals(Direction.DOWN)) {
                                    c0bf = chunk.getBlock(x, y, z).getBlockFace(d);
                                    c1bf = chunk.getBlock(currentX, y, z).getBlockFace(d);
                                    c2bf = chunk.getBlock(currentX, y, currentZ).getBlockFace(d);
                                    c3bf = chunk.getBlock(x, y, currentZ).getBlockFace(d);
                                    uv.Add(new Vector2(0, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, 0));
                                    uv.Add(new Vector2(0, 0));
                                } else if (d.Equals(Direction.UP)) {
                                    c0bf = chunk.getBlock(x, y, currentZ).getBlockFace(d);
                                    c1bf = chunk.getBlock(currentX, y, currentZ).getBlockFace(d);
                                    c2bf = chunk.getBlock(currentX, y, z).getBlockFace(d);
                                    c3bf = chunk.getBlock(x, y, z).getBlockFace(d);
                                    uv.Add(new Vector2(0, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, blockCountZ));
                                    uv.Add(new Vector2(blockCountX, 0));
                                    uv.Add(new Vector2(0, 0));
                                }
                                Vector3 node0 = c0bf.node0;
                                Vector3 node1 = c1bf.node1;
                                Vector3 node2 = c2bf.node2;
                                Vector3 node3 = c3bf.node3;

                                int startIndex = vertices.Count;

                                vertices.Add(node0);
                                vertices.Add(node1);
                                vertices.Add(node2);
                                vertices.Add(node3);

                                int node0Index = startIndex + 0;
                                int node1Index = startIndex + 1;
                                int node2Index = startIndex + 2;
                                int node3Index = startIndex + 3;

                                List<int> triangles = getMaterialTriangles(chunk, block.getType());

                                triangles.Add(node0Index);
                                triangles.Add(node1Index);
                                triangles.Add(node2Index);

                                triangles.Add(node2Index);
                                triangles.Add(node3Index);
                                triangles.Add(node0Index);


                            }
                        }
                    }
                }
            }
        }
        //Debug.Log(str);
        mesh.SetVertices(vertices);
        mesh.uv = uv.ToArray();
        int i = 0;
        List<Material> materials = new List<Material>();
        mesh.subMeshCount = chunk.materialTriangles.Count;
        foreach (SkyMaterial sm in chunk.materialTriangles.Keys) {
            List<int> triangles = getMaterialTriangles(chunk, sm);
            mesh.SetTriangles(triangles.ToArray(), i);
            Material mat = sm.getSetting().getMaterial();
            materials.Add(mat);
            i++;
        }

        chunk.meshRenderer.materials = materials.ToArray();

        return mesh;
    }
    public static bool canRenderTogether(Block block1, Block block2, Direction direction) {
        BlockFace bf = block1.getBlockFace(direction);
        BlockFace bf2 = block2.getBlockFace(direction);
        if (block1.getType().Equals(block2.getType()) == false || bf.shouldRender == false ||
                                bf.isRendered || bf2.shouldRender == false || bf2.isRendered) {
            return false;
        }
        return true;
    }

    public static List<int> getMaterialTriangles(Chunk chunk, SkyMaterial m) {
        List<int> l;
        if (chunk.materialTriangles.ContainsKey(m)) {
            chunk.materialTriangles.TryGetValue(m, out l);
        } else {
            l = new List<int>();
            chunk.materialTriangles.Add(m, l);
        }
        return l;
    }
}

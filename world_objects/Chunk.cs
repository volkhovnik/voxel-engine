using System;
using OpenSimplex;
using OpenTK.Mathematics;
using static Settings;


public class Chunk
{
    public VoxelEngine app;
    public World world;
    public Vector3i position { get; private set; }
    public Matrix4 m_model { get; private set; }
    public byte[] voxels;// { get; private set; }

    internal ChunkMesh? mesh;// { get; private set; }
    /*
    public bool IsEmpty { get; private set; }

    public Vector3 Center { get; private set; }
    private Func<Chunk, bool> isOnFrustum;

    /// <summary>
    /// Initializes the Chunk class with a reference to the World instance, its position, and other properties.
    /// </summary>
    public Chunk(World world, Vector3i position)
    {
        this.app = world.app;
        //this.world = world;
        this.Position = position;
        this.MModel = GetModelMatrix();
        this.voxels = new byte[Settings.CHUNK_VOL];
        this.IsEmpty = true;

        this.Center = (position.ToVector3() + new Vector3(0.5f)) * Settings.CHUNK_SIZE;
        this.isOnFrustum = this.app.player.Frustum.IsOnFrustum;
    }
    */

    public Chunk(World world, Vector3i position)
    {
        this.app = world.app;
        this.world = world;
        this.position = position;
        this.m_model = Matrix4.CreateTranslation(new Vector3(position) * Settings.CHUNK_SIZE);
        BuildVoxels(out this.voxels, position, out IsEmpty);
        //BuildMesh(out this.mesh);
    }

    internal void BuildMesh(out ChunkMesh mesh) =>
        mesh = new ChunkMesh(this);

    public void Render()
    {
        /*
        if (!IsEmpty && isOnFrustum(this))
        {
            SetUniform();
            mesh.Render();
        }
        */
        world.app.shader_program.chunk["m_model"] = m_model;
        //this.mesh?.program["m_model"] = m_model;
        this.mesh?.Render();
    }

    static internal byte[] BuildVoxels(out byte[] voxels, Vector3i position, out bool IsEmpty)
    {
        voxels = new byte[CHUNK_VOL];
        var (cx, cy, cz) = position * Settings.CHUNK_SIZE;
        /*
        GenerateTerrain(ref voxels, cx, cy, cz);

        if (Array.Exists(voxels, v => v != 0))
        {
            IsEmpty = false;
        }
        */
        for (byte x = 0; x < CHUNK_SIZE; x++)
        {
            var wx = x + cx;
            for (byte z = 0; z < CHUNK_SIZE; z++)
            {
                var wz = z + cz;
                var world_height = (int)(_noise.Evaluate(wx * 0.02, wz * 0.02) * 32 + 32);
                var localHeight = Math.Min(world_height - cy, CHUNK_SIZE);
                for (byte y = 0; y < localHeight; y++)
                {
                    var wy = y + cy;
                    voxels[x + CHUNK_SIZE * z + CHUNK_AREA * y] = (byte)(wy + 1);
                }
            /*
                for (byte y = 0; y < CHUNK_SIZE; y++)
                {
                    //var nval = 1;
                    var nval = _noise.Evaluate(x * 0.1, y * 0.1, z * 0.1) + 1;
                    voxels[x + CHUNK_SIZE * z + CHUNK_AREA * y] =
                        (byte)((int)nval > 0 ? (x + y + z) : 0);
                }
            */
            }
        }
        return voxels;
    }

    static private Noise _noise;
    static Chunk() =>
        _noise = new OpenSimplex.Noise(Settings.SEED);

    /*
    /// <summary>
    ///  Generates the terrain for the chunk.
    /// </summary>
    private static void GenerateTerrain(ref byte[] voxels, int cx, int cy, int cz)
    {
        for (int x = 0; x < Settings.CHUNK_SIZE; x++)
        {
            int wx = x + cx;
            for (int z = 0; z < Settings.CHUNK_SIZE; z++)
            {
                int wz = z + cz;
                int worldHeight = TerrainGen.GetHeight(wx, wz);
                int localHeight = Math.Min(worldHeight - cy, Settings.CHUNK_SIZE);

                for (int y = 0; y < localHeight; y++)
                {
                    int wy = y + cy;
                    TerrainGen.SetVoxelId(ref voxels, x, y, z, wx, wy, wz, worldHeight);
                }
            }
        }
    }
    */
}

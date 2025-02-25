using OpenTK.Graphics.OpenGL4;
using System;
using System.Linq;

public class CubeMesh : BaseMesh
{
    private VoxelEngine app;

    public CubeMesh(VoxelEngine app) : base()
    {
        this.app = app;
        this.program = app.shader_program.voxelMarkerProgram;

        this.vboFormat = "2f2 3f2";
        this.attrs = new[] { "in_tex_coord_0", "in_position" };
        this.vao = GetVAO();
    }

    private static float[] GetData((float, float, float)[] vertices, (int, int, int)[] indices)
    {
        return indices.SelectMany(triangle => triangle.Select(ind => vertices[ind])).SelectMany(v => new[] { v.Item1, v.Item2, v.Item3 }).ToArray();
    }

    protected override float[] GetVertexData()
    {
        var vertices = new[]
        {
            (0f, 0f, 1f), (1f, 0f, 1f), (1f, 1f, 1f), (0f, 1f, 1f),
            (0f, 1f, 0f), (0f, 0f, 0f), (1f, 0f, 0f), (1f, 1f, 0f)
        };
        var indices = new[]
        {
            (0, 2, 3), (0, 1, 2),
            (1, 7, 2), (1, 6, 7),
            (6, 5, 4), (4, 7, 6),
            (3, 4, 5), (3, 5, 0),
            (3, 7, 4), (3, 2, 7),
            (0, 6, 1), (0, 5, 6)
        };
        var vertexData = GetData(vertices, indices);

        var texCoordVertices = new[] { (0f, 0f), (1f, 0f), (1f, 1f), (0f, 1f) };
        var texCoordIndices = new[]
        {
            (0, 2, 3), (0, 1, 2),
            (0, 2, 3), (0, 1, 2),
            (0, 1, 2), (2, 3, 0),
            (2, 3, 0), (2, 0, 1),
            (0, 2, 3), (0, 1, 2),
            (3, 1, 2), (3, 0, 1),
        };
        var texCoordData = GetData(texCoordVertices.Select(v => (v.Item1, v.Item2, 0f)).ToArray(), texCoordIndices);
        var combinedData = vertexData.Concat(texCoordData).ToArray();
        return combinedData;
    }
}


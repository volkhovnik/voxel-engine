using System;
using ModernGL;

public class QuadMesh : BaseMesh
{
    public QuadMesh(VoxelEngine app)
        : base(ref app.ctx, ref app.shader_program.quad, "3f 3f", ["in_position", "in_color"])
    {
        Rebuild();
    }

    protected override object GetVertexData()
    {
        var vertices = new (double, double, double)[]
        {
            (0.5, 0.5, 0.0), (-0.5, 0.5, 0.0), (-0.5, -0.5, 0.0),
            (0.5, 0.5, 0.0), (-0.5, -0.5, 0.0), (0.5, -0.5, 0.0)
        };

        var colors = new (double, double, double)[]
        {
            (0, 1, 0), (1, 0, 0), (1, 1, 0),
            (0, 1, 0), (1, 1, 0), (0, 0, 1)
        };

        var vertex_data = // np.hstack([tex_coords, vertices])
            vertices.Zip(colors, (v, c) => new float[] {
                (float)v.Item1, (float)v.Item2, (float)v.Item3, (float)c.Item1, (float)c.Item2, (float)c.Item3 })
            .SelectMany(x => x)
            .ToArray();

        return vertex_data;
    }
}



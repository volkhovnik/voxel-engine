using OpenTK.Graphics.OpenGL4;

public class Clouds
{
    private VoxelEngine app;
    private CloudMesh mesh;

    public Clouds(VoxelEngine app)
    {
        this.app = app;
        this.mesh = new CloudMesh(app);
    }

    /// <summary>
    /// Updates the u_time uniform in the shader program with the current time from the VoxelEngine instance.
    /// </summary>
    public void Update()
    {
        app.shader_program.SetUniform(mesh.program, "u_time", (float)app.time);
    }

    public void Render()
    {
        mesh.Render();
    }
}

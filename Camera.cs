using OpenTK.Mathematics;

public class Camera
{
    public Vector3 Position { get; private set; }

    protected float Yaw { get; private set; }
    protected float Pitch { get; private set; }

    public Vector3 Up { get; private set; } = Vector3.UnitY;
    public Vector3 Right { get; private set; } = Vector3.UnitX;
    public Vector3 Forward { get; private set; } = -Vector3.UnitZ;


    public Matrix4 m_proj { get; set; }
    public Matrix4 m_view { get; private set; }

    public Frustum Frustum { get; private set; }

    public Camera(Vector3 position, float yaw, float pitch)
    {
        Position = position;

        Yaw = MathHelper.DegreesToRadians(yaw);
        Pitch = MathHelper.DegreesToRadians(pitch);

        m_proj = Matrix4.CreatePerspectiveFieldOfView(Settings.V_FOV, Settings.ASPECT_RATIO, Settings.NEAR, Settings.FAR);
        m_view = Matrix4.Identity;

        Frustum = new Frustum(this);
    }

    public virtual void Update()
    {
        UpdateVectors();
        UpdateViewMatrix();
    }

    private void UpdateViewMatrix()
    {
        m_view = Matrix4.LookAt(Position, Position + Forward, Up);
    }

    private void UpdateVectors()
    {
        // the front matrix is calculated
        Forward = (MathF.Cos(Yaw) * MathF.Cos(Pitch), MathF.Sin(Pitch), MathF.Sin(Yaw) * MathF.Cos(Pitch));

        // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
        Forward = Vector3.Normalize(Forward);

        // Calculate both the right and the up vector using cross product.
        // Note that we are calculating the right from the global up; this behaviour might
        // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
        Right = Vector3.Normalize(Vector3.Cross(Forward, Vector3.UnitY));
        Up = Vector3.Normalize(Vector3.Cross(Right, Forward));
    }

    public void RotatePitch(float deltaY)
    {
        Pitch -= deltaY;
        Pitch = MathHelper.Clamp(Pitch, -Settings.PITCH_MAX, Settings.PITCH_MAX);
    }

    public void RotateYaw(float deltaX) =>
        Yaw += deltaX;

    // Moves the camera in the specified direction based on the given velocity.
    public void MoveLeft(float velocity) =>
        Position -= Right * velocity;

    public void MoveRight(float velocity) =>
        Position += Right * velocity;

    public void MoveUp(float velocity) =>
        Position += Up * velocity;

    public void MoveDown(float velocity) =>
        Position -= Up * velocity;

    public void MoveForward(float velocity) =>
        Position += Forward * velocity;

    public void MoveBack(float velocity) =>
        Position -= Forward * velocity;
}

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;


public class Player : Camera
{
    private VoxelEngine app;

    public Player(VoxelEngine app, Vector3 position, float yaw = -90, float pitch = 0)
        : base(position, yaw, pitch)
    {
        this.app = app;
    }

    public void HandleEvent(MouseState mouseState, KeyboardState keyState)
    {
        MouseControl(mouseState);
        KeyboardControl(keyState);
        Update();
    }

    /*
    public void HandleEvent(MouseState e)
    {
        // Adding and removing voxels with clicks
        if (e.IsAnyButtonDown)
        {
            var voxelHandler = app.scene.world.voxelHandler;

            if (e.IsButtonDown(MouseButton.Button1))
            {
                voxelHandler.SetVoxel();
            }
            if (e.IsButtonDown(MouseButton.Button3))
            {
                voxelHandler.SwitchMode();
            }
        }
    }
    */

    // hack: for RDP connection mouse-delta fix
    static Vector2 pos = new Vector2(0, 0);

    private void MouseControl(MouseState mouseState)
    {
        if (mouseState.Delta != Vector2.Zero)
        {
            var (mouse_dx, mouse_dy) = mouseState.Delta - pos;
            if (mouse_dx != 0)
                RotateYaw(mouse_dx * Settings.MOUSE_SENSITIVITY);
            if (mouse_dy != 0)
                RotatePitch(mouse_dy * Settings.MOUSE_SENSITIVITY);
            if (mouse_dx != 0 || mouse_dy != 0)
            {
                Console.WriteLine("Mouse dx {0} dy {1}", mouse_dx, mouse_dy);
                //Console.WriteLine("mouse delta {0}", mouseState.Delta);
                //Console.WriteLine("mouse delta own {0}", mouseState.Delta - pos);
            }
            pos = mouseState.Delta;
        }
    }

    private void KeyboardControl(KeyboardState keyState)
    {
        if (keyState.IsAnyKeyDown)
        {
            var velocity = Settings.PLAYER_SPEED * (float)app.deltaTime;
            if (keyState.IsKeyDown(Keys.W))
                MoveForward(velocity);
            if (keyState.IsKeyDown(Keys.S))
                MoveBack(velocity);
            if (keyState.IsKeyDown(Keys.D))
                MoveRight(velocity);
            if (keyState.IsKeyDown(Keys.A))
                MoveLeft(velocity);
            if (keyState.IsKeyDown(Keys.Q))
                MoveUp(velocity);
            if (keyState.IsKeyDown(Keys.E))
                MoveDown(velocity);

            Console.WriteLine("position {0} yaw {1} pitch {2}", Position, Yaw, Pitch);
        }
    }
}

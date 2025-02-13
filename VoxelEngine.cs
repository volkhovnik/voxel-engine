using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using ModernGL;

public class MovingAverage
{
    private Queue<double> _values;
    private int _maxCount = 10;
    private double _valuesSum = 0;

    public MovingAverage() => _values = new Queue<double>();

    public double Add(double value)
    {
        if (_values.Count >= _maxCount)
            _valuesSum -= _values.Dequeue();
        _values.Enqueue(value);
        _valuesSum += value;
        return _valuesSum / _values.Count;
    }
}
public class VoxelEngine : GameWindow
{
    public glContext ctx;

    MovingAverage deltaTimeAvg = new();
    public double deltaTime;
    public double time_init;
    public double time;

    //public Textures textures;
    public Player player;
    public ShaderProgram shader_program;
    public Scene scene;

    public VoxelEngine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        GL.ClearColor(Color4.CornflowerBlue);
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.Blend);

        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        this.ctx = moderngl.create_context();

        //textures = new Textures(this);
        player = new Player(this, Settings.PLAYER_POS);
        shader_program = new ShaderProgram(this, ctx);
        scene = new Scene(this);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        time_init = DateTime.Now.Ticks;
    }


    private void HandleEvents()
    {
        // event.type == pg.QUIT or (event.type == pg.KEYDOWN and event.key == pg.K_ESCAPE)
        if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
        {
            Close();
        }

        // Handle other events, such as player input
        //player.HandleEvent(MouseState);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        HandleEvents();

        player.Update();
        shader_program.Update();
        scene.Update();

        time = (DateTime.Now.Ticks - time_init) * 0.001;
        deltaTime = args.Time * 1000;

        Title = $"{1.0 / deltaTimeAvg.Add(args.Time):F0} FPS";
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Thread.Sleep(100); // Sleep for 100 milliseconds

        scene.Render();

        SwapBuffers();
    }


    protected override void OnUnload()
    {
        base.OnUnload();
        // Add any necessary cleanup code here
    }

    public static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = Settings.WIN_RES,
            Title = "OpenGL 3.3 Window",
            API = ContextAPI.OpenGL,
            APIVersion = new Version(3, 3),
            Profile = ContextProfile.Core,
            Flags = ContextFlags.ForwardCompatible,
            DepthBits = Settings.DEPTH_SIZE
        };

        using (var window = new VoxelEngine(GameWindowSettings.Default, nativeWindowSettings))
        {
            window.Run();
        }
    }
}

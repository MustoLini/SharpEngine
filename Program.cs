using System.IO;
using System.Runtime.InteropServices;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        
        private static Triangle triangle= new Triangle ( new Vertex[]
        
            {
                new Vertex(new Vector(0f, 0f), Color.Red),
                new Vertex(new Vector(1f, 0f), Color.Green),
                new Vertex(new Vector(0f, 1f), Color.Blue)
            }
        );

        private static bool test;
        private const int vertexSize = 3;


        
        static void Main(string[] args)
        {
            
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();


            var direction = new Vector(0.0003f, 0.0003f);
            var multip = 0.9999f;
            
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                triangle.scale(multip);
                // 2. Keep track of the Scale, so we can reverse it
                
                if (triangle.currentScale <= 0.5f) {
                    multip = 1.0001f;
                }
                if (triangle.currentScale >= 1f) {
                    multip = 0.9999f;
                }
                
                triangle.Move(direction);
                // 4. Check the X-Bounds of the Screen
                if (triangle.GetMaxBound().x >= 1 && direction.x > 0 || triangle.GetMinBound().x <= -1 && direction.x < 0) {
                    direction.x *= -1;
                }
                
                // 5. Check the Y-Bounds of the Screen
                if (triangle.GetMaxBound().y >= 1 && direction.y > 0 || triangle.GetMinBound().y <= -1 && direction.y < 0) {
                    direction.y *= -1;
                }

                
            }
        }

        private static void Render(Window window)
        {
            triangle.Render();
            Glfw.SwapBuffers(window);
            // glFlush();
        }

        private static void ClearScreen()
        {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        private static unsafe void LoadTriangleIntoBuffer()
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);

            

            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Color)));

            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }

        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/position-color.vert"));
            glCompileShader(vertexShader);

            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/vertex-color.frag"));
            glCompileShader(fragmentShader);

            // create shader program- rendering pipeline.
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        static Window CreateWindow()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);
            
            var window = Glfw.CreateWindow(1024,768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;

        }
       
       

        
    }
}

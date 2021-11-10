using System;
using System.IO;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;

        }

        public static Vector operator * (Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }
    }
    
    class Program
    {
        static Vector[] vertices = new Vector[] {
            new Vector(-.1f, -.1f),
            new Vector(.1f, -.1f),
            new Vector(0f, .1f),
            
            new Vector(.4f, .4f),
            new Vector(.6f, .4f),
            new Vector(.5f, .6f),

        };

        private static bool test;
        private const int vertexSize = 3;

        
        
        static void Main(string[] args)
        {
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();


            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                if (test==false)
                {
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i].x += 0.0001f;
                        vertices[i].y += 0.0001f;
                        
                        if (vertices[1].x >= 1f)
                        {
                            test = true;
                        }
                        
                        
                    }
                    
                }
                else
                {
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i].x -= 0.0001f;
                        vertices[i].y -= 0.0001f;
                        
                        if (vertices[0].x <= -1f)
                        {
                            test = false;
                        }
                    }
                }
                
                UpdateTriangleBuffer();
            }
        }

        private static void Render(Window window)
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
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

            UpdateTriangleBuffer();

            glVertexAttribPointer(0, 3, GL_FLOAT, false, vertexSize * sizeof(float), null);

            glEnableVertexAttribArray(0);
        }

        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/screen-coordinates.vert"));
            glCompileShader(vertexShader);

            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/green-triangle.frag"));
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
        static unsafe void UpdateTriangleBuffer() {
            fixed (Vector* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }

        
    }
}

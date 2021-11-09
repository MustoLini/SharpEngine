using System;
using System.IO;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static float[] vertices = new float[] {
            // vertex 1 x, y, z
            -.5f, -.5f, 0f,
            // vertex 2 x, y, z
            .5f, -.5f, 0f,
            // vertex 3 x, y, z
            0f, .5f, 0f
        };
        static void Main(string[] args)
        {
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();


            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                glClearColor(.2f, .05f, .2f, 1);
                glClear(GL_COLOR_BUFFER_BIT);
                glDrawArrays(GL_TRIANGLES, 0, 3);
                glFlush();
                
                if (vertices[7]>0)
                {
                    vertices[0] += 0.0001f;
                    vertices[1] += 0.0001f;
                    vertices[3] -= 0.0001f;
                    vertices[4] += 0.0001f;
                    vertices[7] -= 0.0001f;
                }
                UpdateTriangleBuffer();
            }
        }

        private static unsafe void LoadTriangleIntoBuffer()
        {
            float[] vertites = new float[]
            {
                -.5f, -.5f, 0f,
                .5f, -.5f, 0f,
                0f, .5f, 0f
            };

            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            unsafe
            {
                fixed (float* vertex = &vertites[0])
                {
                    glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertites.Length, vertex, GL_STATIC_DRAW);
                }

                glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
            }

            glEnableVertexAttribArray(0);
        }

        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);

            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/change-triangle.frag"));
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
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);
            var window = Glfw.CreateWindow(1024,768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;

        }
        static unsafe void UpdateTriangleBuffer() {
            fixed (float* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
    }
}

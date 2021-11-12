using System;
using System.IO;
using System.Runtime.InteropServices;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        
        private static Shapes _shapes= new Shapes ( new Vertex[]
        
            {
                new Vertex(new Vector(0f, 0f), Color.Red),
                new Vertex(new Vector(1f, 0f), Color.Green),
                new Vertex(new Vector(0f, 1f), Color.Blue)
            }
        );
        private static Shapes triangle2= new Shapes ( new Vertex[]
        
            {
                new Vertex(new Vector(0f, 0f), Color.Red),
                new Vertex(new Vector(0.5f, 0f), Color.Green),
                new Vertex(new Vector(0f, 0.5f), Color.Blue)
            }
        );

        private static bool test;
        private const int vertexSize = 3;


        
        static void Main(string[] args)
        {
            
            var window = CreateWindow();
            

            CreateShaderProgram();


            var direction = new Vector(0.0003f, 0.0003f);
            var direction2 = new Vector(0.0004f, 0.0004f);
            var multip = 0.9999f;
            
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                _shapes.scale(multip);
                triangle2.scale(multip);
                // 2. Keep track of the Scale, so we can reverse it
                
                if (_shapes.currentScale <= 0.5f) {
                    multip = 1.0001f;
                }
                if (_shapes.currentScale >= 1f) {
                    multip = 0.9999f;
                }
                
                _shapes.Move(direction);
                triangle2.Move(direction2);
              
                if (_shapes.GetMaxBound().x >= 1 && direction.x > 0 || _shapes.GetMinBound().x <= -1 && direction.x < 0) {
                    direction.x *= -1;
                }
                
                if (_shapes.GetMaxBound().y >= 1 && direction.y > 0 || _shapes.GetMinBound().y <= -1 && direction.y < 0) {
                    direction.y *= -1;
                }
                
                if (triangle2.GetMaxBound().x >= 1 && direction2.x > 0 || triangle2.GetMinBound().x <= -1 && direction2.x < 0) {
                    direction2.x *= -1;
                }
                
                if (triangle2.GetMaxBound().y >= 1 && direction2.y > 0 || triangle2.GetMinBound().y <= -1 && direction2.y < 0) {
                    direction2.y *= -1;
                }
                _shapes.Rotation();
                triangle2.Rotation();


            }
        }

        private static void Render(Window window)
        {
            _shapes.Render();
            triangle2.Render();
            Glfw.SwapBuffers(window);
            // glFlush();
        }

        private static void ClearScreen()
        {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
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

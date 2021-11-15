using System;
using System.Runtime.InteropServices;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Shapes
    {
        private Vertex[] vertices;
        public float currentScale { get; private set; }
        public Shapes(Vertex[] vertices)
        {
            this.vertices = vertices;
            currentScale = 1f;
            LoadTriangleIntoBuffer();
        }

        public Vector GetCenter()
        {
            return (GetMinBound() + GetMaxBound()) / 2;         
        }
        public void scale(float multip)
        {

            var center = GetCenter();
            
            Move(center*-1);
            
            for (var i = 0; i < vertices.Length; i++) {
                vertices[i].position *= multip;
            }
            
            Move(center);

            currentScale *= multip;
        }

        public Vector GetMaxBound()
        {
            var max = vertices[0].position;
            for (var i = 1; i < vertices.Length; i++)
            {
                max = Vector.Max(max, vertices[i].position);
            }

            return max;
        }

        public Vector GetMinBound()
        {
            var min = vertices[0].position;
            for (var i = 1; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].position);
            }

            return min;
        }

        public void Move(Vector direction)
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += direction;
            }

            // center += direction;
        }
        public unsafe void Render() {
            fixed (Vertex* vertex = &vertices[0]) {
                Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, Gl.GL_DYNAMIC_DRAW);
            }
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, vertices.Length);
        }
        private static unsafe void LoadTriangleIntoBuffer()
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color)));

            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }

        public void Rotation()
        {
            float rotation = 0.003f;
            var center = GetCenter();
            Move(center * -1);

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector position = vertices[i].position;
                var currentangle = Math.Atan2(position.y, position.x);
                var currentmagnitude = MathF.Sqrt(MathF.Pow(position.x, 2) + MathF.Pow(position.y, 2));
                var newXPosition = MathF.Cos((float) currentangle + rotation) * currentmagnitude;
                var newYPosition = MathF.Sin((float) currentangle + rotation) * currentmagnitude;
                vertices[i].position = new Vector(newXPosition, newYPosition);
            }
            Move(center);
        }
    }
}
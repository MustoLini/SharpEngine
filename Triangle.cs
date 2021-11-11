using OpenGL;

namespace SharpEngine
{
    public class Triangle
    {
        private Vertex[] vertices;
        public float currentScale { get; private set; }
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            currentScale = 1f;
        }

        public void scale(float multip)
        {
            
            var min = GetMinBound();
            var max = GetMaxBound();
            
            var center = (min + max) / 2;
            
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
        }
        public unsafe void Render() {
            fixed (Vertex* vertex = &vertices[0]) {
                Gl.glBufferData(Gl.GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, Gl.GL_DYNAMIC_DRAW);
            }
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, vertices.Length);
        }
    }
}
using System.Collections.Generic;

namespace SharpEngine {
	public class Scene {

		public List<Shape> Shapes;

		public Scene() {
			Shapes = new List<Shape>();
		}
		
		public void Add(Shape shape) {
			Shapes.Add(shape);
		}

		public void Render() {
			for (int i = 0; i < this.Shapes.Count; i++) {
				Shapes[i].Render();
			}
		}
	}
}
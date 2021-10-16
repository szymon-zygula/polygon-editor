using System.Collections.Generic;
using System.Windows.Controls;

namespace polygon_editor {
    public class CanvasState {
        public DrawingPlane Plane { get; }

        public Image Canvas { get; }
        public ListView ShapeList { get; }

        public int TotalPolygonCount { get; set; }
        public int TotalCircleCount { get; set; }

        public List<Polygon> Polygons { get; }
        public List<Circle> Circles { get; }
        public Circle ActiveCircle { get; }
        public Polygon ActivePolygon { get; }

        public CanvasControlState ControlState { get; private set; }

        public CanvasState(Image canvas, ListView shapeList) {
            Canvas = canvas;
            ShapeList = shapeList;
            TotalPolygonCount = 0;
            TotalCircleCount = 0;
            Polygons = new List<Polygon>();
            Circles = new List<Circle>();
            Plane = new DrawingPlane((int)Canvas.Width, (int)Canvas.Height);
            ControlState = new DoingNothingControlState(this);

            Plane.Fill(CanvasOptions.BACKGROUND_COLOR);
            Canvas.Source = Plane.CreateBitmapSource();
        }

        public void AddPolygon(Polygon polygon) {
            Polygons.Add(polygon);
            TotalPolygonCount += 1;
            polygon.Index = TotalPolygonCount;
            ShapeList.Items.Add(polygon);
        }

        public void AddCircle(Circle circle) {
            Circles.Add(circle);
            TotalCircleCount += 1;
            circle.Index = TotalCircleCount;
            ShapeList.Items.Add(circle);
        }

        public void UpdateCanvas() {
            Plane.Fill(CanvasOptions.BACKGROUND_COLOR);

            foreach (Polygon polygon in Polygons) {
                Plane.DrawPolygon(polygon);
            }

            foreach (Circle circle in Circles) {
                Plane.DrawCircle(circle);
            }

            ControlState.DrawStateFeatures();

            Canvas.Source = Plane.CreateBitmapSource();
        }
        public void SetControlState(CanvasControlState controlState) {
            ControlState.ExitState();
            ControlState = controlState;
            ControlState.EnterState();
            UpdateCanvas();
        }
    }
}

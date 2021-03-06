using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace polygon_editor {
    public class CanvasState {
        public DrawingPlane Plane { get; }

        public Image Canvas { get; }
        public ListView ShapeList { get; }
        public TextBox ConstraintParam { get; }

        public int TotalPolygonCount { get; set; }
        public int TotalCircleCount { get; set; }

        public List<Polygon> Polygons { get; }
        public List<Circle> Circles { get; }

        public CanvasControlState ControlState { get; private set; }

        public CanvasState(Image canvas, ListView shapeList, TextBox constraintParam) {
            Canvas = canvas;
            ShapeList = shapeList;
            ConstraintParam = constraintParam;
            TotalPolygonCount = 0;
            TotalCircleCount = 0;
            Polygons = new List<Polygon>();
            Circles = new List<Circle>();
            Plane = new DrawingPlane((int)Canvas.Width, (int)Canvas.Height);
            ControlState = new DoingNothingControlState(this);

            Plane.Fill(CanvasOptions.BACKGROUND_COLOR);
            Canvas.Source = Plane.CreateBitmapSource();
        }

        public int GetConstraintParam() {
            try {
                return int.Parse(ConstraintParam.Text);
            }
            catch {
                return 0;
            }
        }

        public (int?, Polygon) FindAnyEdgeWithinMouse(double mouseX, double mouseY) {
            int? activeEdge = null;
            Polygon activePolygon = null;
            foreach(Polygon polygon in Polygons) {
                activePolygon = polygon;
                activeEdge = polygon.FindEdgeWithinSquareRadius(
                    mouseX, mouseY,
                    CanvasOptions.ACTIVE_EDGE_RADIUS
                );
                if (activeEdge != null) break;
            }

            return (activeEdge, activePolygon);
        }

        public Circle FindAnyCircleCenterWithinMouse(double mouseX, double mouseY) {
            foreach(Circle circle in Circles) {
                if (IsWithinCircleCenterMouse(circle, mouseX, mouseY)) {
                    return circle;
                }
            }

            return null;
        }

        public bool IsWithinCircleCenterMouse(Circle circle, double mouseX, double mouseY) {
            bool isWithinXRange = Math.Abs(circle.X - mouseX) <= CanvasOptions.ACTIVE_VERTEX_RADIUS;
            bool isWithinYRange = Math.Abs(circle.Y - mouseY) <= CanvasOptions.ACTIVE_VERTEX_RADIUS;
            return isWithinXRange && isWithinYRange;
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
                Plane.Draw(polygon);
            }

            foreach (Circle circle in Circles) {
                Plane.Draw(circle);
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

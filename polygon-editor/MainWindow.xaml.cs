using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace polygon_editor {

    public partial class MainWindow : Window {
        private readonly CanvasState State;

        public MainWindow() {
            InitializeComponent();
            State = new CanvasState(Canvas, ShapeList, TextBoxConstraintParameter);

            CreateDefaultScene();
        }

        private void CreateDefaultScene() {
            Polygon polygon1 = new Polygon();
            polygon1.AddPoint(10, 10);
            polygon1.AddPoint(50, 50);
            polygon1.AddPoint(100, 200);
            polygon1.AddPoint(200, 100);
            polygon1.Color = CanvasOptions.NORMAL_LINE_COLOR;
            State.AddPolygon(polygon1);

            Polygon polygon2 = new Polygon();
            polygon2.AddPoint(300, 300);
            polygon2.AddPoint(300, 200);
            polygon2.AddPoint(400, 300);
            polygon2.AddPoint(325, 420);
            polygon2.AddPoint(250, 425);
            polygon2.Color = CanvasOptions.NORMAL_LINE_COLOR;
            State.AddPolygon(polygon2);

            Circle circle1 = new Circle {
                X = 200,
                Y = 200,
                R = 50,
                Color = CanvasOptions.NORMAL_LINE_COLOR
            };
            State.AddCircle(circle1);

            Circle circle2 = new Circle {
                X = 350,
                Y = 150,
                R = 80,
                Color = CanvasOptions.NORMAL_LINE_COLOR
            };
            State.AddCircle(circle2);

            polygon1.Constraints[0] = new ConstantEdgeLengthConstraint(polygon1, 0, 50);
            var perpenConstraint = new PerpendicularEdgesConstraint(polygon1, 1, polygon1, 3);
            polygon1.Constraints[1] = perpenConstraint;
            polygon1.Constraints[3] = perpenConstraint;
            var eqConstraint = new EqualEdgeLengthsConstraint(polygon1, 2, polygon2, 0);
            polygon1.Constraints[2] = eqConstraint;
            polygon2.Constraints[0] = eqConstraint;

            var tanConstraint = new TangentCircleConstraint(circle2, polygon2, 1);
            circle2.Constraint = tanConstraint;
            polygon2.Constraints[1] = tanConstraint;

            circle1.Constraint = new ConstantRadiusConstraint(circle1, 50);

            polygon1.ForceAllConstraints();
            polygon2.ForceAllConstraints();
            circle1.Constraint.ForceConstraint();
            circle2.Constraint.ForceConstraint();

            State.UpdateCanvas();
        }

        private void ButtonDrawPolygon_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new DrawingPolygonControlState(State));
        }

        private void ButtonDrawCircle_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new DrawingCircleControlState(State));
        }

        private void CanvasImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            State.ControlState.OnMouseLeftButtonUp(e);
        }

        private void CanvasImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            State.ControlState.OnMouseRightButtonUp(e);
        }

        private void CanvasImage_MouseMove(object sender, MouseEventArgs e) {
            State.ControlState.OnMouseMove(e);
        }

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            State.ControlState.OnMouseLeftButtonDown(e);
        }

        private void CanvasImage_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Middle) {
                State.ControlState.OnMouseMiddleButtonDown(e);
            }
        }

        private void ShapeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ShapeList.SelectedItem is Polygon) {
                Polygon polygon = ShapeList.SelectedItem as Polygon;
                State.SetControlState(new ActivePolygonControlState(State, polygon));
                State.UpdateCanvas();
            }
            else if (ShapeList.SelectedItem is Circle) {
                Circle circle = ShapeList.SelectedItem as Circle;
                State.SetControlState(new ActiveCircleControlState(State, circle));
                State.UpdateCanvas();
            }

            ShapeList.SelectedItem = null;
        }

        private void ButtonSegLenConstr_Click(object sender, RoutedEventArgs e) {

            State.SetControlState(new SegmentLenConstrControlState(State));
        }

        private void ButtonCircRadConstr_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new CircleRadiusConstrControlState(State));
        }

        private void ButtonTwoSegLen_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new TwoSegmentsEqLenConstrControlState(State));
        }

        private void ButtonCirSegTang_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new TangentSegCirConstrControlState(State));
        }

        private void ButtonSegPerpen_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new SegmentPerpenConstrControlState(State));
        }

        private void ButtonRemoveConstraint_Click(object sender, RoutedEventArgs e) {
            State.SetControlState(new RemoveConstraintControlState(State));
        }
    }
}

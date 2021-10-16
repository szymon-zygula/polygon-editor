using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace polygon_editor {

    public partial class MainWindow : Window {
        private CanvasState State;

        public MainWindow() {
            InitializeComponent();
            State = new CanvasState(Canvas, ShapeList);
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

        private void ShapeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ShapeList.SelectedItem is Polygon) {
                Polygon polygon = ShapeList.SelectedItem as Polygon;
                State.SetControlState(new ActivePolygonControlState(State, polygon));
            }
            else if (ShapeList.SelectedItem is Circle) {
                Circle circle = ShapeList.SelectedItem as Circle;
                State.SetControlState(new ActiveCircleControlState(State, circle));
            }

            State.UpdateCanvas();
        }

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            State.ControlState.OnMouseLeftButtonDown(e);
        }

        private void CanvasImage_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Middle) {
                State.ControlState.OnMouseMiddleButtonDown(e);
            }
        }
    }
}

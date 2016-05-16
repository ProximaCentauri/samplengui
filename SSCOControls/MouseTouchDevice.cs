using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using RPSWNET;

namespace SSCOControls
{
    public class MouseTouchDevice : TouchDevice
    {
        public MouseTouchDevice(int deviceId) :
            base(deviceId)
        {
            Position = new Point();
        }

        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
            return new TouchPointCollection();
        }

        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            Point point = Position;
            if (relativeTo != null)
            {
                try
                {
                    point = this.ActiveSource.RootVisual.TransformToDescendant((Visual)relativeTo).Transform(Position);
                }
                catch (NullReferenceException e)
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "MouseTouchDevice.GetTouchPoint() caught NullReferenceException {0}", e.Message);
                }
                catch (InvalidOperationException e)
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "MouseTouchDevice.GetTouchPoint() caught InvalidOperationException {0}", e.Message);
                }
                catch (ArgumentException e)
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskError, "MouseTouchDevice.GetTouchPoint() caught ArgumentException {0}", e.Message);
                }
            }
            Rect rect = new Rect(point, new Size(1, 1));
            return new TouchPoint(this, point, rect, TouchAction.Move);
        }

        public static void RegisterEvents(FrameworkElement root)
        {
            root.PreviewMouseDown += MouseDown;
            root.PreviewMouseMove += MouseMove;
            root.PreviewMouseUp += MouseUp;
        }

        public static void UnregisterEvents(FrameworkElement root) 
        { 
            root.PreviewMouseDown -= MouseDown;
            root.PreviewMouseMove -= MouseMove; 
            root.PreviewMouseUp -= MouseUp; 
        }

        public Point Position { get; set; }

        private static void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (device != null && device.IsActive)
            {
                device.ReportUp();
                device.Deactivate();
                device = null;
            }
            device = new MouseTouchDevice(e.MouseDevice.GetHashCode());
            device.SetActiveSource(e.MouseDevice.ActiveSource);
            device.Position = e.GetPosition(null);
            device.Activate();
            device.ReportDown();
            e.Handled = true;
        }

        private static void MouseMove(object sender, MouseEventArgs e)
        {
            if (device != null && device.IsActive)
            {
                device.Position = e.GetPosition(null);
                device.ReportMove();
                e.Handled = true;
            }
        }

        private static void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (device != null && device.IsActive)
            {
                device.Position = e.GetPosition(null);
                device.ReportUp();
                device.Deactivate();
                device = null;
                e.Handled = true;
            }
        }

        private static MouseTouchDevice device;
    }
}
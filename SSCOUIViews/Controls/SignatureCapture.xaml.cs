using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSCOUIModels.Controls;
using SSCOUIModels.Models;
using SSCOUIModels;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Text;
using FPsxWPF;
using RPSWNET;
using SSCOControls;

namespace SSCOUIViews.Controls
{
    /// <summary>
    /// Interaction logic for TenderStep.xaml
    /// </summary>
    public partial class SignatureCapture : UserControl
    {
        public SignatureCapture()
        {
            InitializeComponent();
            InitializeSignaturePath();          
        }

        public void InitializeSignaturePath()
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            SignaturePath.Data = geometryGroup;
            pathGeometry = new PathGeometry(new PathFigureCollection());
            geometryGroup.Children.Add(pathGeometry);
        }

        public void RefreshUI()
        {
            SignatureButtons.Visibility = Visibility.Visible;
            if (IsShowSignatureCapture())
            {
                this.Instruction.Property(TextBlock.TextProperty).SetResourceValue("SignBelow");
                SignatureArea.Visibility = Visibility.Visible;
                SignatureButtons.Visibility = Visibility.Visible;
                if (viewModel.StateParam.Equals("RequestSig"))
                {
                    ClearSignature();
                }
                EnableSignatureButtons();
            }
            else
            {
                this.Instruction.Property(TextBlock.TextProperty).SetResourceValue("SignSignature");
                SignatureArea.Visibility = Visibility.Collapsed;
                if (viewModel.StateParam.Equals("RequestSig"))
                {
                    SignatureButtons.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewModel = DataContext as IMainViewModel;
            if (this.viewModel != null)
            {
                this.viewModel.PropertyChanged -= new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);
            }
            this.viewModel = DataContext as IMainViewModel;
            this.viewModel.PropertyChanged += new PropertyChangedEventHandler(this.ViewModel_PropertyChanged);

            DrawSignatureData();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SignatureData"))
            {
                DrawSignatureData();
            } 
            else if (e.PropertyName.Equals("CMButton1MedShown") || e.PropertyName.Equals("CMButton1MedEnabled"))
            {
                EnableOKButton();
            }
            else if (e.PropertyName.Equals("CMButton2MedShown") || e.PropertyName.Equals("CMButton2MedEnabled"))
            {
                EnableClearButton();
            }
        }

        private void DrawSignatureData()
        {
            String signatureData = (String)(viewModel.GetPropertyValue("SignatureData"));
            if (!String.IsNullOrEmpty(signatureData) && !signatureData.Equals(CreateSignatureData()))
            {
                ClearSignature();

                string[] data = signatureData.Split('|');
                for(int i=2; i<data.Length; i++)
                {
                    string[] coordinates =data[i].Split(' ');
                    if (coordinates .Length >= 2)
                    {
                        for (int j = 0; j < coordinates.Length - 1; j = j + 2)
                        {
                            currentPoint = new Point(Double.Parse(coordinates[j]), Double.Parse(coordinates[j+1]));
                            AddCurrentPointToCurrentStrock();
                        }
                    }
                    AddCurrentStrockToSignaureStrocks();
                } 
            }
            EnableSignatureButtons();
        }

        private void SignatureCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            Point touchPoint=e.GetTouchPoint(this.SignatureCanvas).Position;
            if (IsValidPoint(touchPoint))
            {
                currentPoint = touchPoint;
                AddCurrentPointToCurrentStrock();
            }
        }

        private void SignatureCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            AddCurrentStrockToSignaureStrocks();
        }

        private void SignatureCanvas_TouchLeave(object sender, TouchEventArgs e)
        {
            AddCurrentStrockToSignaureStrocks();
        }

        private void SignatureCanvas_TouchMove(object sender, TouchEventArgs e)
        {
            Point touchPoint = e.GetTouchPoint(this.SignatureCanvas).Position;
            if (IsValidPoint(touchPoint))
            {
                currentPoint = new Point(touchPoint.X, touchPoint.Y);
                AddCurrentPointToCurrentStrock();
            }
            else
            {
                AddCurrentStrockToSignaureStrocks();
            }
        }

        private void ClearSignature()
        {
            signatureStrokes = null;
            currentStroke = null;

            EnableSignatureButtons();
           
            if (SignaturePath.Data != null)
            {
                InitializeSignaturePath();
            }            
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearSignature();
            viewModel.ActionCommand.Execute("CMButton2Med");
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (IsShowSignatureCapture()) 
            {
                viewModel.ActionCommand.Execute(String.Format("Signature({0})", CreateSignatureData()));
            }
            else
            {
                viewModel.ActionCommand.Execute("CMButton1Med");
            }
        }

        private string CreateSignatureData()
        {
            StringBuilder strSignature = new StringBuilder();

            PresentationSource source = PresentationSource.FromVisual(this);
            double dpiX = 0.0, dpiY = 0.0;
            if (source != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            }
            strSignature.Append(dpiX);
            strSignature.Append(' ');
            strSignature.Append(dpiY);
            strSignature.Append('|');

            strSignature.Append(SignatureCanvas.ActualWidth);
            strSignature.Append(' ');
            strSignature.Append(SignatureCanvas.ActualHeight);

            if (signatureStrokes != null)
            {
                foreach (ArrayList stroke in signatureStrokes)
                {
                    strSignature.Append('|');
                    for (int i = 0; i < stroke.Count; i++)
                    {
                        if (i != 0)
                        {
                            strSignature.Append(' ');
                        }
                        strSignature.Append(((Point)stroke[i]).X);
                        strSignature.Append(' ');
                        strSignature.Append(((Point)stroke[i]).Y);
                    }
                }
            }
            return strSignature.ToString();
        }

        private bool IsValidPoint(Point p)
        {
            if(p.X<0 || p.X>SignatureCanvas.ActualWidth ||
                p.Y<0 || p.Y>SignatureCanvas.ActualHeight)
            {
                return false;
            }
            return true;
        }

        private void AddCurrentPointToCurrentStrock()
        {
            if (currentStroke == null)
            {
                currentStroke = new ArrayList();     
            }
            
            currentStroke.Add(currentPoint);
            if(currentStroke.Count==1)
            {
                currentPathFigure = new PathFigure(currentPoint, new List<PathSegment>(), false);
                pathGeometry.Figures.Add(currentPathFigure);

                currentPathFigure.StartPoint = currentPoint;
                if (signatureStrokes == null || signatureStrokes.Count == 0)
                {
                    viewModel.ActionCommand.Execute(String.Format("Signature({0})", "start"));                    
                }
                EnableSignatureButtons();
            }
            else
            {
                LineSegment line = new LineSegment(currentPoint, true);
                line.Freeze();
                currentPathFigure.Segments.Add(line);
            }

        }

        private void AddCurrentStrockToSignaureStrocks()
        {
            if (currentStroke != null && currentStroke.Count > 0)
            {
                if (currentStroke.Count == 1)
                {
                    EllipseGeometry ellipseGeometry= new EllipseGeometry((Point)(currentStroke[currentStroke.Count - 1]), 1, 1);
                    ((GeometryGroup)SignaturePath.Data).Children.Add(ellipseGeometry);
                }

                if(signatureStrokes == null)
                {
                    signatureStrokes = new ArrayList();
                }
                signatureStrokes.Add(currentStroke);
                
                currentStroke = null;
                currentPathFigure.Freeze();                
            }
            currentPoint = new Point(-1, -1);
        }

        private void EnableOKButton()
        {
            EnableSignatureButton(OKButton, "CMButton1Med") ;
        }

        private void EnableClearButton()
        {
            EnableSignatureButton(ClearButton, "CMButton2Med");
        }

        private void EnableSignatureButton(ImageButton button, String viewModelPropertyPrexName)
        {
            button.IsEnabled = ((bool)viewModel.GetPropertyValue(viewModelPropertyPrexName + "Shown")) &&
                ((bool)viewModel.GetPropertyValue(viewModelPropertyPrexName + "Enabled")) &&
                (IsSigned() || !IsShowSignatureCapture());
        }

        private void EnableSignatureButtons()
        {
            EnableOKButton();
            EnableClearButton();
        }

        private bool IsSigned()
        {
            return ((currentStroke != null) && currentStroke.Count > 0) || (signatureStrokes!=null && signatureStrokes.Count > 0);
        }

        private bool IsShowSignatureCapture()
        {
            return FPsx.ConvertFromOleBool((String)viewModel.GetPropertyValue("ShowSigCapture"));
        }

        private IMainViewModel viewModel;

        private Point currentPoint = new Point(-1, -1);
        private ArrayList currentStroke;
        private ArrayList signatureStrokes;
        private PathFigure currentPathFigure;
        private PathGeometry pathGeometry;        
    }
}

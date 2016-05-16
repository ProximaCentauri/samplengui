using System;
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
using SSCOControls;
using System.Windows.Controls.Primitives;
using System.Timers;
using System.ComponentModel;
using PsxNet;
using RPSWNET;

namespace SSCOUIViews.Views
{
    /// <summary>
    /// Interaction logic for VolumePopup.xaml
    /// </summary>
    public partial class VolumePopup : PopupView
    {
        public VolumePopup(IMainViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
            VSlider.MouseLeave += (object sender, MouseEventArgs e) => { viewModel.ActionCommand.Execute("UIActivity"); };
        }

        public override void Show(bool isShowing)
        {
            if (isShowing)
            {
                holdTimer = new Timer(200);
                holdTimer.AutoReset = true;
                if (holdTimer != null)
                {
                    holdTimer.Elapsed += HoldTimer_Elapsed;
                }

                VSlider.ApplyTemplate();

                btnThumb = (VSlider.Template.FindName("SliderThumb", VSlider) as Track).Thumb;
                btnIncrease = VSlider.Template.FindName("SliderIncreaseButton", VSlider) as ImageButton;
                btnDecrease = VSlider.Template.FindName("SliderDecreaseButton", VSlider) as ImageButton;

                if (btnThumb != null)
                {
                    btnThumb.TouchUp += Thumb_TouchUp;
                    btnThumb.MouseEnter += Thumb_MouseEnter;
                    btnThumb.MouseLeave += Thumb_MouseLeave;
                }
                if (btnIncrease != null)
                {
                    btnIncrease.TouchDown += IncreaseButton_TouchDown;
                    btnIncrease.TouchUp += IncreaseButton_TouchUp;
                }
                if (btnDecrease != null)
                {
                    btnDecrease.TouchDown += DecreaseButton_TouchDown;
                    btnDecrease.TouchUp += DecreaseButton_TouchUp;
                }

                string volIncLevel = GetPropertyStringValue("VolumeControlIncLevel");
                incLevel = volIncLevel != null && volIncLevel.Length > 0 ? double.Parse(volIncLevel) / 100 : 0;

                try
                {
                    Psx.GetVolume(out lVolumeLevel, out rVolumeLevel);
                }
                catch (PsxException ex)
                {
                    CmDataCapture.CaptureFormat(CmDataCapture.MaskWarning, "VolumePopup.Show() - psx exception: {0}", ex.Message);
                }
                VSlider.Value = (VSlider.Maximum * lVolumeLevel) / 100;
                if (0 == lVolumeLevel && 0 == rVolumeLevel)
                {
                    lVolumeLevel = -1;
                    rVolumeLevel = -1;
                }
            }
            else 
            {
                if (btnThumb != null)
                {
                    btnThumb.TouchUp -= Thumb_TouchUp;
                    btnThumb.MouseLeave -= Thumb_MouseLeave;
                    btnThumb.MouseEnter -= Thumb_MouseEnter;
                }
                if (btnIncrease != null)
                {
                    btnIncrease.TouchDown -= IncreaseButton_TouchDown;
                    btnIncrease.TouchUp -= IncreaseButton_TouchUp;
                }
                if (btnDecrease != null)
                {
                    btnDecrease.TouchDown -= DecreaseButton_TouchDown;
                    btnDecrease.TouchUp -= DecreaseButton_TouchUp;
                }

                if (holdTimer != null)
                {
                    holdTimer.Elapsed -= HoldTimer_Elapsed;
                    holdTimer.Dispose();
                }
            }
            base.Show(isShowing);
        }

        private void volumeGoBack_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ActionCommand.Execute("ViewModelSet(Context;)");
        }

        private void IncreaseButton_TouchDown(object sender, TouchEventArgs e)
        {
            holdTimer.Start();
            timerElapsed = false;
            isVolIncrease = true;
            ChangeVolumeLevel();
            VSlider.Value = (VSlider.Maximum * lVolumeLevel) / 100;
        }

        private void DecreaseButton_TouchDown(object sender, TouchEventArgs e)
        {
            holdTimer.Start();
            timerElapsed = false;
            isVolIncrease = false;
            ChangeVolumeLevel();
            VSlider.Value = (VSlider.Maximum * lVolumeLevel) / 100;
        }

        private void IncreaseButton_TouchUp(object sender, TouchEventArgs e)
        {
            holdTimer.Stop();
            if (timerElapsed)
            {
                ControlsAudio.PlaySound(btnIncrease.ClickSound);
            }
        }

        private void DecreaseButton_TouchUp(object sender, TouchEventArgs e)
        {
            holdTimer.Stop();
            if (timerElapsed)
            {
                ControlsAudio.PlaySound(btnDecrease.ClickSound);
            }
        }

        private void ChangeVolumeLevel()
        {
            if (isVolIncrease)
            {
                if (-1 == lVolumeLevel && -1 == rVolumeLevel)
                {
                    lVolumeLevel = Convert.ToInt32(incLevel * 100);
                    rVolumeLevel = Convert.ToInt32(incLevel * 100);
                    viewModel.ActionCommand.Execute(String.Format("Volume({0})", lVolumeLevel));
                }
                else if (100 > lVolumeLevel && 100 > rVolumeLevel)
                {
                    lVolumeLevel += Convert.ToInt32(incLevel * 100);
                    rVolumeLevel += Convert.ToInt32(incLevel * 100);
                    if (100 < lVolumeLevel && 100 < rVolumeLevel)
                    {
                        lVolumeLevel = 100;
                        rVolumeLevel = 100;
                    }
                    viewModel.ActionCommand.Execute(String.Format("Volume({0})", lVolumeLevel));
                }
            }
            else 
            {
                if (0 < lVolumeLevel && 0 < rVolumeLevel)
                {
                    lVolumeLevel -= Convert.ToInt32(incLevel * 100);
                    rVolumeLevel -= Convert.ToInt32(incLevel * 100);
                    if (0 >= lVolumeLevel && 0 >= rVolumeLevel)
                    {
                        lVolumeLevel = -1;
                        rVolumeLevel = -1;
                    }
                    viewModel.ActionCommand.Execute(String.Format("Volume({0})", lVolumeLevel));
                }
            }
        }

        private void Thumb_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                isVolIncrease = (e.GetPosition(btnThumb).Y < startPos.Y) ? true : false;
                ChangeVolumeLevel();
                VSlider.Value = (VSlider.Maximum * lVolumeLevel) / 100;
            }
        }

        private void Thumb_MouseEnter(object sender, MouseEventArgs e)
        {
            startPos = e.GetPosition(btnThumb);
        }

        private void Thumb_TouchUp(object sender, TouchEventArgs e)
        {
            ControlsAudio.PlaySound(btnIncrease.ClickSound);
        }

        private void HoldTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ChangeVolumeLevel();
            timerElapsed = true;
            if (lVolumeLevel == -1 || lVolumeLevel == 100)
            {
                holdTimer.Stop();
            }
            VSlider.Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate()
            {
                VSlider.Value = (VSlider.Maximum * lVolumeLevel) / 100;
            }));
        }

        private Thumb btnThumb = null;
        private ImageButton btnIncrease = null;
        private ImageButton btnDecrease = null;
        private int lVolumeLevel = -1;
        private int rVolumeLevel = -1;
        private double incLevel = 0;
        private Point startPos;
        private Timer holdTimer;
        private bool isVolIncrease= true;
        private bool timerElapsed = false;
    }
}
/******************************************************************************/
/*                                                                            */
/*   Programm MyScaleWindow                                                   */
/*   Demo for scaling a window an store its size and position                 */
/*                                                                            */
/*   13.06.2014 0.0.0 uhwgmxorg Start                                         */
/*                                                                            */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyScaleWindow
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        const double WINDOW_WIDTH = 525;
        const double WINDOW_HEIGHT = 350;
 
        private double _aspectRatio = 0.0;

        #region DependencyProperty ScaleValue
        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));
        public double ScaleValue { get { return (double)GetValue(ScaleValueProperty); } set { SetValue(ScaleValueProperty, value); } }

        /// <summary>
        /// OnCoerceScaleValue
        /// </summary>
        /// <param name="o"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                return mainWindow.OnCoerceScaleValue((double)value);
            else
                return value;
        }

        /// <summary>
        /// OnCoerceScaleValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0f;

            value = Math.Max(0.1, value);
            return value;
        }

        /// <summary>
        /// OnScaleValueChanged
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        /// <summary>
        /// OnScaleValueChanged
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {

        }

        /// <summary>
        /// MainGrid_SizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainGrid_SizeChanged(object sender, EventArgs e)
        {
            CalculateScale();
        }

        /// <summary>
        /// CalculateScale
        /// </summary>
        private void CalculateScale()
        {
            double xScale = ActualWidth / WINDOW_WIDTH;
            double yScale = ActualHeight / WINDOW_HEIGHT;
            double value = Math.Min(xScale, yScale);
            ScaleValue = (double)OnCoerceScaleValue(Grid_Main, value);
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_100_Click
        /// Reset the window to the orginal size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_100_Click(object sender, RoutedEventArgs e)
        {
            ScaleWindow.Width = WINDOW_WIDTH;
            ScaleWindow.Height = WINDOW_HEIGHT;
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// OnRenderSizeChanged
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (_aspectRatio > 0)
                if (sizeInfo.WidthChanged)
                {
                    this.Height = sizeInfo.NewSize.Width * 1 / _aspectRatio;
                }
                else
                {
                    this.Width = sizeInfo.NewSize.Height * _aspectRatio;
                }
        }

        /// <summary>
        /// ScaleWindow_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScaleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _aspectRatio = this.ActualWidth / this.ActualHeight;
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="p"></param>
        private void OnPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        #endregion

    }
}

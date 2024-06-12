using infosystems.task.shellv1.Enums;
using System;
using System.Collections.Generic;
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
using wcheck.wcontrols;

namespace infosystems.task.shellv1.Controls
{
    /// <summary>
    /// Логика взаимодействия для SelectISControl.xaml
    /// </summary>
    public partial class SelectISControl : UserControl
    {
        public EventHandler<ISType> Click;
        public ISType ISType { get; private set; }
        public SelectISControl()
        {
            InitializeComponent();
            ISType = ISType.GIS;
        }

        private void uiGIS_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if(ISType == ISType.GIS && border != uiGIS)
            {
                border.Background = "#fc8343".GetBrush();
            }
            else if(ISType == ISType.ISPD && border != uiISPD)
            {
                border.Background = "#fc8343".GetBrush();
            }
            else if (ISType == ISType.AS && border != uiAuto)
            {
                border.Background = "#fc8343".GetBrush();
            }
        }

        private void uiGIS_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (ISType == ISType.GIS && border != uiGIS)
            {
                border.Background = "#1f1f1f".GetBrush();
            }
            else if (ISType == ISType.ISPD && border != uiISPD)
            {
                border.Background = "#1f1f1f".GetBrush();
            }
            else if (ISType == ISType.AS && border != uiAuto)
            {
                border.Background = "#1f1f1f".GetBrush();
            }
        }

        private void uiGIS_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (ISType != ISType.GIS && border == uiGIS)
            {
                ISType = ISType.GIS;
                uiGIS.Background = "#fca577".GetBrush();
                uiGISText.Foreground = "#1f1f1f".GetBrush();

                uiISPD.Background = "#1f1f1f".GetBrush();
                uiISPDText.Foreground = "#dfdfdf".GetBrush();

                uiAuto.Background = "#1f1f1f".GetBrush();
                uiAutoText.Foreground = "#dfdfdf".GetBrush();
            }
            else if (ISType != ISType.ISPD && border == uiISPD)
            {
                ISType = ISType.ISPD;
                uiISPD.Background = "#fca577".GetBrush();
                uiISPDText.Foreground = "#1f1f1f".GetBrush();

                uiGIS.Background = "#1f1f1f".GetBrush();
                uiGISText.Foreground = "#dfdfdf".GetBrush();

                uiAuto.Background = "#1f1f1f".GetBrush();
                uiAutoText.Foreground = "#dfdfdf".GetBrush();
            }
            else if (ISType != ISType.AS && border == uiAuto)
            {
                ISType = ISType.AS;
                uiAuto.Background = "#fca577".GetBrush();
                uiAutoText.Foreground = "#1f1f1f".GetBrush();

                uiISPD.Background = "#1f1f1f".GetBrush();
                uiISPDText.Foreground = "#dfdfdf".GetBrush();

                uiGIS.Background = "#1f1f1f".GetBrush();
                uiGISText.Foreground = "#dfdfdf".GetBrush();
            }
            Click?.Invoke(this, ISType);
        }

        internal void SetIs(ISType type)
        {
            switch (type)
            {
                case ISType.GIS:
                    ISType = ISType.GIS;
                    uiGIS.Background = "#fca577".GetBrush();
                    uiGISText.Foreground = "#1f1f1f".GetBrush();

                    uiISPD.Background = "#1f1f1f".GetBrush();
                    uiISPDText.Foreground = "#dfdfdf".GetBrush();

                    uiAuto.Background = "#1f1f1f".GetBrush();
                    uiAutoText.Foreground = "#dfdfdf".GetBrush();
                    break;
                case ISType.ISPD:
                    ISType = ISType.ISPD;
                    uiISPD.Background = "#fca577".GetBrush();
                    uiISPDText.Foreground = "#1f1f1f".GetBrush();

                    uiGIS.Background = "#1f1f1f".GetBrush();
                    uiGISText.Foreground = "#dfdfdf".GetBrush();

                    uiAuto.Background = "#1f1f1f".GetBrush();
                    uiAutoText.Foreground = "#dfdfdf".GetBrush();
                    break;
                case ISType.AS:
                    ISType = ISType.AS;
                    uiAuto.Background = "#fca577".GetBrush();
                    uiAutoText.Foreground = "#1f1f1f".GetBrush();

                    uiISPD.Background = "#1f1f1f".GetBrush();
                    uiISPDText.Foreground = "#dfdfdf".GetBrush();

                    uiGIS.Background = "#1f1f1f".GetBrush();
                    uiGISText.Foreground = "#dfdfdf".GetBrush();
                    break;
            }
            Click?.Invoke(this, ISType);
        }
    }
}

using infosystems.task.shellv1.Forms.Abstract;
using LiveChartsCore.VisualElements;
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
    /// Логика взаимодействия для TestingControl.xaml
    /// </summary>
    public partial class TestingControl : UserControl
    {
        public TestElement TestElement { get; }
        public TestingControl(TestElement element)
        {
            InitializeComponent();
            TestElement = element;
            uiQuestion.Text = TestElement.Question;
            foreach(var answer in TestElement.Answers)
            {
                RadioButton rb = new RadioButton { GroupName = "Answers", Margin = new Thickness(10, 5, 10, 5) };
                rb.Content = new TextBlock()
                {
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 12,
                    Foreground = "#1f1f1f".GetBrush(),
                    Text = answer,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                rb.Checked += (sender, e) =>
                {
                    element.Select(answer);
                };
                        
                uiRadioButtons.Children.Add(rb);
            }
        }
    }
}

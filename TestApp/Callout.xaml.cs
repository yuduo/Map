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

namespace TestApp
{
    /// <summary>
    /// Callout.xaml 的交互逻辑
    /// </summary>
    public partial class Callout : UserControl
    {
        public Callout()
        {
            InitializeComponent();
        }

        public string CalloutTitle
        {
            get {
                return title.Text;
            }
            set {
                title.Text = value;
            }
        }

        public string CalloutContent
        {
            get
            {
                return content.Text;
            }
            set
            {
                content.Text = value;
            }
        }
    }
}

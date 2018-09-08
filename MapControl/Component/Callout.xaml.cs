using System.Windows;
using System.Windows.Input;

namespace HongLi.MapControl.Component
{
    /// <summary>
    /// 地图气泡信息框
    /// </summary>
    public partial class Callout
    {
        public Callout()
        {
            InitializeComponent();
        }

        public string CalloutTitle
        {
            get
            {
                return Title.Text;
            }
            set
            {
                Title.Text = value;
            }
        }

        public string CalloutContent
        {
            get
            {
                return Content.Text;
            }
            set
            {
                Content.Text = value;
            }
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }
    }
}

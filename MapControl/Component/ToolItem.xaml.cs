using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HongLi.MapControl.Component
{
    /// <summary>
    /// 地图工具按钮，工具栏中
    /// </summary>
    public partial class ToolItem
    {
        public delegate void ClickHandler(object sender, RoutedEventArgs e);

        public event ClickHandler Click;

        public ToolItem()
        {
            InitializeComponent();
        }

        private string _icon = "";
        public string Icon
        {
            set
            {
                _icon = value;
                //图标激活
                Active = Active;
            }
        }

        private bool _isChecked;
        public bool Active
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;

                Img.Source = _isChecked ? new BitmapImage(new Uri("../Asset/selected_" + _icon, UriKind.Relative)) : new BitmapImage(new Uri("../Asset/" + _icon, UriKind.Relative));
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);            
        }
    }
}

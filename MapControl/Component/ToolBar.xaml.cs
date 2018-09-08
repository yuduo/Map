using System.Linq;
using System.Windows;

namespace HongLi.MapControl.Component
{
    /// <summary>
    /// 地图工具栏
    /// </summary>
    public partial class ToolBar
    {
        public delegate void ToolActiveHandler(string tool);

        public event ToolActiveHandler ToolActive;

        public delegate void ToolDeActiveHandler(string tool);

        public event ToolDeActiveHandler ToolDeActive;

        public ToolBar()
        {
            InitializeComponent();
        }

        private string _currentTool = "";
        private void ToolItem_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in Tools.Children.OfType<ToolItem>())
            {
                if (item.Name == ((ToolItem)sender).Name)
                {
                    if(_currentTool== item.Name)
                    {
                        item.Active = false;
                        _currentTool = "";
                        ToolDeActive?.Invoke(item.Name);
                    }
                    else
                    {                       
                        item.Active = true;
                        _currentTool = item.Name;
                        ToolActive?.Invoke(item.Name);
                    }
                    
                    continue;
                }
                if (item.Active)
                {
                    ToolDeActive?.Invoke(item.Name);
                }
                item.Active = false;
            }
        }

        public void CancelToolItem(string itemName)
        {
            foreach (var item in Tools.Children.OfType<ToolItem>())
            {
                if (item.Name == itemName)
                {
                    if (_currentTool == item.Name)
                    {
                        _currentTool = "";
                    }
                    item.Active = false;
                    break;
                }
            }
        }
    }
}

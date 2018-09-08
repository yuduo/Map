using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MapPoint overlayLocation = null;

            //var ferneyTip = this.mapView.FindName("ferneyOverlay") as FrameworkElement;
            //// if the overlay element is found, set its position and make it visible
            //if (ferneyTip != null)
            //{
            //    overlayLocation = new Esri.ArcGISRuntime.Geometry.MapPoint(6.1081, 46.2558, Esri.ArcGISRuntime.Geometry.SpatialReferences.Wgs84);
            //    Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(ferneyTip, overlayLocation);
            // //   ferneyTip.Visibility = Visibility.Visible;
            //}

            //return;
            
            Callout c = new Callout();
            c.CalloutTitle = "标题";
            c.CalloutContent = "内容";
            OverlayItemsControl f = new OverlayItemsControl();
            f.Items.Add(c);
           
            mapView.Overlays.Items.Add(c);
            // if the overlay element is found, set its position and make it visible
            if (f != null)
            {
                overlayLocation = new Esri.ArcGISRuntime.Geometry.MapPoint(6.1081, 46.2558, Esri.ArcGISRuntime.Geometry.SpatialReferences.Wgs84);
                Esri.ArcGISRuntime.Controls.MapView.SetViewOverlayAnchor(c, overlayLocation);
                c.Visibility = Visibility.Visible;
            }
        }
    }
}

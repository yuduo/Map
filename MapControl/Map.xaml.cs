using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using HongLi.MapControl.Behavior;
using HongLi.MapControl.Layers;
using HongLi.MapControl.Util;
using HongLi.MapControl.Util.TrackSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;

namespace HongLi.MapControl
{
    /// <summary>
    /// Map.xaml 的交互逻辑
    /// </summary>
    public partial class Map
    {
        public static string TaskGuid = "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC";

        private static bool _needInit = true;
        [StructLayout(LayoutKind.Sequential)]
        public struct rect_STRUCT
        {

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]

            public char[] lon;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]

            public char[] lan;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PointRectangle
        {
            /// <summary>
            /// 左上
            /// </summary>
            /// 

            public rect_STRUCT TopLeft;
            /// <summary>
            /// 右下
            /// </summary>
            /// 

            public rect_STRUCT BottomRight;
        }
        [DllImport("User32.DLL")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, IntPtr lParam);
        private const int WM_USER = 0x400;
        //Timer timerMap;
        public Map()
        {
            //第一次加载时进行授权
            if (_needInit)
            {
                try
                {
                    ArcGISRuntimeEnvironment.ClientId = "uK0DxqYT0om1UXa9";
                    ArcGISRuntimeEnvironment.Initialize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to initialize the ArcGIS Runtime with the client ID provided: " + ex.Message);
                }
            }
            // ArcGISRuntimeEnvironment.ClientId = "uK0DxqYT0om1UXa9";
            InitializeComponent();
        }

        /// <summary>
        /// 载入配置危机
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static bool Init(string configPath)
        {
            try
            {
                ConfigUtil.Load(configPath);
            }
            catch (Exception ee)
            {
                _needInit = true;
                LogUtil.Error(typeof(Map), ee.Message);
                return false;
            }
            _needInit = false;
            return true;
        }
        public void ClickItem(string lon, string lan)
        {
           
           // foreach (Graphic gra in _highLightLayer.Graphics)
            //foreach (Graphic gra in _drawingLayer.Graphics)
                
            {
                //MapPoint point = gra.Geometry as MapPoint;
                
                //if (point.X.ToString() == lon && point.Y.ToString()==lan)

                {
                    string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"001\" DataType = \"SetExtent\">"
                   + "<LON Type = \"SINGLE\">" + lon + "</LON>"
                   + "<LAT Type = \"SINGLE\" >" + lan + "</LAT>"
                   + "<Level Type = \"LONG\" ></Level></Document>";
                    SetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "SetExtent", xml, (string x) =>
                    {
                        //MessageBox.Show(x.ToString());
                    });
                }
            }
           
        }
        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            TrackCtrl.SetMap(this);

            //timerMap = new Timer();
            //timerMap.Interval = 60 * 1000;
            //timerMap.Elapsed += TimerMap_Elapsed;
            //timerMap.Start();

            IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;
            SendMessage((int)windowHandle, WM_USER + 58, 2, IntPtr.Zero);

            
        }

        private void TimerMap_Elapsed(object sender, ElapsedEventArgs e)
        {
            InitMap();
        }

        public void InitMap()
        {
            if (_behaviors != null) _behaviors.Clear();
            MapObj.Layers.Clear();
            MapView.LayerLoaded -= MapView_LayerLoaded;
            MapView.LayerLoaded += MapView_LayerLoaded;
           
            LogUtil.Info(typeof(Map), "初始化地图...");

            MapView.MaxScale = ConfigUtil.MaxScale;
            MapView.MinScale = ConfigUtil.MinScale;

            var lo = ConfigUtil.TiledLayer;
            if (lo != null)
            {
                var tiledLayer = new ArcGISTiledMapServiceLayer(lo.Uri)
                {
                    ID = lo.Id,
                    DisplayName = lo.Name
                };
                if (lo.Token != null)
                {
                    tiledLayer.Token = lo.Token.Replace("\r\n", "");
                    //tiledLayer.Token = "XDjutneIavm-1NSbIhZFhq3mCcseAViwRgf48LVcEbk.";
                }
                MapObj.Layers.Add(tiledLayer);

            }

            lo = ConfigUtil.ImageLayer;
            if (lo != null)
            {
                var imageLayer = new ArcGISImageServiceLayer(lo.Uri)
                {
                    ID = lo.Id,
                    DisplayName = lo.Name
                };
                MapObj.Layers.Add(imageLayer);
            }

            var los = ConfigUtil.DynamicLayer;
            if (los != null)
            {
                foreach (var l in los)
                {
                    var dynamicLayer = new ArcGISDynamicMapServiceLayer(l.Uri) { ID = l.Id };
                    if (l.Token != null)
                    {
                        dynamicLayer.Token = l.Token.Replace("\r\n", ""); ;
                    }

                    dynamicLayer.DisplayName = l.Name;
                    MapObj.Layers.Add(dynamicLayer);
                }
            }

            _drawingLayer = new OverlayLayer(MapCtrl)
            {
                DisplayName = "用户绘图",
                SceneProperties = { SurfacePlacement = SurfacePlacement.Absolute }
            };
            MapObj.Layers.Add(_drawingLayer);

            _highLightLayer = new OverlayLayer(MapCtrl)
            {
                DisplayName = "查询高亮",
                SceneProperties = { SurfacePlacement = SurfacePlacement.Absolute }
            };
            MapObj.Layers.Add(_highLightLayer);


            MapView.SetViewAsync(ConfigUtil.InitCenter, ConfigUtil.InitScale, new TimeSpan(10000));

            LogUtil.Info(typeof(Map), "地图初始化完成");
        }

        public void SetTrack(TrackModel tm)
        {
            TrackCtrl.Dispatcher.Invoke(() =>
            {
                TrackCtrl.Visibility = Visibility.Visible;
                TrackCtrl.SetModel(tm);
            });
        }

        /// <summary>
        /// 绘制图层定义
        /// </summary>
        private OverlayLayer _highLightLayer;
        private OverlayLayer _drawingLayer;

        /// <summary>
        /// 根据名称获取一个绘制图层
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public OverlayLayer GetGraphicLayer(string target)
        {
            OverlayLayer gra = null;
            switch (target)
            {
                case "Highlight":
                    gra = _highLightLayer;
                    break;
                case "Drawing":
                    gra = _drawingLayer;
                    break;
            }
            return gra;
        }

        /// <summary>
        /// 向制定名称的绘制图层添加一个绘制对象
        /// </summary>
        /// <param name="gra">绘制对象</param>
        /// <param name="target">绘制图层名称</param>
        /// <param name="param">参数</param>
        public void AddGraphic(Graphic gra, string target, object param = null)
        {
            switch (target)
            {
                case "Highlight":
                    //if (gra.Geometry is MapPoint)
                    //{
                    //    SimpleMarkerSymbol s = new SimpleMarkerSymbol
                    //    {
                    //        Color = Color.FromArgb(0, 0, 0, 0),
                    //        Size = 20,
                    //        Outline =
                    //            new SimpleLineSymbol { Color = Colors.Red, Style = SimpleLineStyle.Solid, Width = 3 }
                    //    };
                    //    gra.Symbol = s;
                    //}
                    if (gra.Geometry is MapPoint)
                    {
                        var bytes = param as byte[];
                        if (bytes != null)
                        {
                            var ps = new PictureMarkerSymbol();
                            ps.SetSource(bytes);
                            gra.Symbol = ps;
                        }
                    }
                    _highLightLayer.Graphics.Add(gra);
                    break;
                case "Drawing":
                    if (gra.Geometry is MapPoint)
                    {
                        var bytes = param as byte[];
                        if (bytes != null)
                        {
                            var ps = new PictureMarkerSymbol();
                            ps.SetSource(bytes);
                            gra.Symbol = ps;
                        }
                    }
                    if (gra.Geometry is Polyline)
                    {
                        if (param == null)
                        {

                        }
                        else
                        {
                            SimpleLineSymbol sls = new SimpleLineSymbol
                            {
                                Color = Colors.Red,
                                Width = 5
                            };
                            gra.Symbol = sls;
                        }
                    }
                    _drawingLayer.Graphics.Add(gra);
                    break;
            }


        }

        /// <summary>
        /// 清空指定名称的绘制图层
        /// </summary>
        /// <param name="target"></param>
        public void ClearGraphic(string target)
        {
            GetGraphicLayer(target)?.ClearGraphics();
            MapCtrl.MapView.Overlays.Items.Clear();
        }

        /// <summary>
        /// 切片图层信息
        /// </summary>
        private TiledLayerInitializationInfo _tiledInfo;

        public TiledLayerInitializationInfo TiledInfo
        {
            get
            {
                return _tiledInfo;
            }
            set
            {
                _tiledInfo = value;
            }
        }

        /// <summary>
        /// 根据地图层级获取比例
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public double GetScale(int? level)
        {
            if (level == null || level < 0 || level >= _tiledInfo.Lods.Count)
            {
                return _tiledInfo.Lods[_tiledInfo.Lods.Count - 2].Scale;
            }
            return _tiledInfo.Lods[(int)level].Scale;
        }

        /// <summary>
        /// 地图图层（控件图层）加载完成，每个图层都会触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapView_LayerLoaded(object sender, Esri.ArcGISRuntime.Controls.LayerLoadedEventArgs e)
        {
            var layer = e.Layer as ArcGISTiledMapServiceLayer;

            if (layer != null)
            {
                if (_tiledInfo != null)
                {
                    return;
                }
                _tiledInfo = layer.TileInfo;
            }
            var eLayer = e.Layer as ArcGISDynamicMapServiceLayer;
            if (eLayer == null) return;
            foreach (LayerObject lo in ConfigUtil.DynamicLayer)
            {
                if (lo.Id == eLayer.ID)
                {
                    lo.ServiceInfo = eLayer.ServiceInfo;
                }
            }
        }

        #region 回调

        /// <summary>
        /// 行为清单
        /// </summary>
        private List<IBehavior> _behaviors;

        /// <summary>
        /// 增加一个行为
        /// </summary>
        /// <param name="beh"></param>
        public void AddBehavior(IBehavior beh)
        {
            if (_behaviors == null)
            {
                _behaviors = new List<IBehavior>();
            }
            _behaviors.Add(beh);
        }

        /// <summary>
        /// 增加一个单例行为
        /// </summary>
        /// <param name="beh"></param>
        public void AddSingletonBehavior(IBehavior beh)
        {
            if (_behaviors == null)
            {
                _behaviors = new List<IBehavior>();
            }
            foreach (var b in _behaviors)
            {
                if (beh.GetType() == b.GetType())
                {
                    return;
                }
            }
            _behaviors.Add(beh);
        }

        /// <summary>
        /// 根据标识删除一个行为
        /// </summary>
        /// <param name="id"></param>
        public void RemoveBehavior(string id)
        {
            if (_behaviors == null)
            {
                _behaviors = new List<IBehavior>();
            }
            for (var i = _behaviors.Count - 1; i >= 0; i--)
            {
                if (((BaseBehavior)_behaviors[i]).Id == id)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 清空行为
        /// </summary>
        public void RemoveAllBehavior()
        {
            for (int i = _behaviors.Count - 1; i >= 0; i--)
            {
                if (((BaseBehavior)_behaviors[i]).IsObsolete)
                {
                    _behaviors.RemoveAt(i);
                }
            }
        }
        #endregion

        /// <summary>
        /// 地图单击事件，行为大多在此生效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapView_MapViewTapped(object sender, Esri.ArcGISRuntime.Controls.MapViewInputEventArgs e)
        {
            
            if (_behaviors == null)
            {
                string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"ClearGraphic\">"
                  + "<Target Type = \"TEXT\">Drawing</Target>"
                  + "</Document>";
                SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "ClearGraphic", xml);
                return;
            }
            foreach (var b in _behaviors)
            {
                if (b is GetMapPointBehavior)
                {
                    b.Work(e);
                    continue;
                }
                if (b is IdentifyBehavior)
                {
                    b.Work(e);
                    continue;
                }
                if (b is HitTestBehavior)
                {
                    b.Work(e);
                }
            }
            //清空已经过时的行为
            for (var i = _behaviors.Count - 1; i >= 0; i--)
            {
                if (((BaseBehavior)_behaviors[i]).IsObsolete)
                {
                    _behaviors.RemoveAt(i);
                }
            }

        }
        // <summary>
        /// 
        /// </summary>
        /// <param name="strCh"></param>
        /// <param name="chArry"></param>
        private void ChangeStringToChar(string strCh, out char[] chArry)
        {
            chArry = new char[16];
            char[] chTmp = strCh.ToCharArray();
            Array.Copy(chTmp, chArry, chTmp.Length > 16 ? 16 : chTmp.Length);
        }

        //测量行为
        MapMeasureBehavior mapBehavior;
        /// <summary>
        /// 工具栏激活事件
        /// </summary>
        /// <param name="tool"></param>
        private void ToolBar_ToolActive(string tool)
        {
            string xml = "<Document TaskGuid = \"FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC\" DataGuid = \"004\" DataType = \"ClearGraphic\">"
                  + "<Target Type = \"TEXT\">Drawing</Target>"
                  + "</Document>";
            SetData("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "", "ClearGraphic", xml);
            switch (tool)
            {
                case "ToolLayer":
                    //LayerCtrl.Visibility = Visibility.Visible;
                    
                    GetDataAsync("TEST", "FF17D1B3-85C7-40F4-8CDC-73DC57CD29BC", "014", "DrawRectangle", (string x) =>
                    {
                        toolBar.CancelToolItem("ToolLayer");


                        XmlDocument xmlDoc = new XmlDocument(); // Create an XML document object
                        xmlDoc.LoadXml(x); // Load the XML document from the specified file
                        List<string> lonList = new List<string>();
                        List<string> lanList = new List<string>();
                        // Get elements
                        XmlNodeList elemList = xmlDoc.GetElementsByTagName("X");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            // use node variable here for your beeds
                            string lon = elemList[i].InnerXml;
                            lonList.Add(lon);
                        }
                        elemList = xmlDoc.GetElementsByTagName("Y");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            // use node variable here for your beeds
                            string lan = elemList[i].InnerXml;
                            lanList.Add(lan);
                        }
                        IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;
                        PointRectangle pointrect = new PointRectangle();


                        if (lonList.Count > 2 && lanList.Count > 2)
                        {
                            ChangeStringToChar(lonList[0], out pointrect.TopLeft.lon);

                            ChangeStringToChar(lanList[0], out pointrect.TopLeft.lan);

                            ChangeStringToChar(lonList[2], out pointrect.BottomRight.lon);

                            ChangeStringToChar(lanList[2], out pointrect.BottomRight.lan);
                        }

                        // Initialize unmanged memory to hold the struct.
                        IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(pointrect));
                        Marshal.StructureToPtr(pointrect, pnt, false);

                        if (pnt != null)
                        {
                            SendMessage((int)windowHandle, WM_USER + 58, 7, pnt);
                        }
                    });
                    break;
                case "ToolMeasureLength":
                    
                    if (mapBehavior == null)
                    {
                        mapBehavior = new MapMeasureBehavior(this, "016", (string s) =>
                        {
                            toolBar.CancelToolItem("ToolMeasureLength");
                        });
                    }
                    mapBehavior.IsObsolete = false;
                    mapBehavior.Work("0");
                    break;
                case "CenterLayer":
                    {
                        MapView.SetViewAsync(ConfigUtil.InitCenter, ConfigUtil.InitScale, new TimeSpan(1000));
                    }
                    break;
            }
        }
       
        /// <summary>
        /// 工具栏取消激活事件
        /// </summary>
        /// <param name="tool"></param>
        private void ToolBar_ToolDeActive(string tool)
        {
            switch (tool)
            {
                case "ToolLayer":
                    LayerCtrl.Visibility = Visibility.Hidden;
                    
                    break;
                case "ToolMeasureLength":
                    if (mapBehavior != null)
                        mapBehavior.Cancel();
                    break;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskGuid"></param>
        /// <param name="dataGuid"></param>
        /// <param name="dataType"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int SetData(string userId, string taskGuid, string dataGuid, string dataType, string xmlData)
        {
            TaskUtil task = new TaskUtil(this);
            return task.SetData(userId, taskGuid, dataGuid, dataType, xmlData);
        }

        /// <summary>
        /// 异步写入数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskGuid"></param>
        /// <param name="dataGuid"></param>
        /// <param name="dataType"></param>
        /// <param name="xmlData"></param>
        /// <param name="callback"></param>
        public Task<string> SetDataAsync(string userId, string taskGuid, string dataGuid, string dataType, string xmlData, Action<string> callback)
        {
            TaskUtil task = new TaskUtil(this);
            return task.SetDataAsync(userId, taskGuid, dataGuid, dataType, xmlData, callback);

        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskGuid"></param>
        /// <param name="dataGuid"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetData(string userId, string taskGuid, string dataGuid, string dataType)
        {
            TaskUtil task = new TaskUtil(this);
            return task.GetData(userId, taskGuid, dataGuid, dataType);
        }

        public async void GetDataAsync(string userId, string taskGuid, string dataGuid, string dataType, Action<string> callback)
        {
            TaskUtil task = new TaskUtil(this);
            await task.GetDataAsync(userId, taskGuid, dataGuid, dataType, callback);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskGuid"></param>
        /// <param name="xmlTransform"></param>
        /// <returns></returns>
        public string TransformData(string userId, string taskGuid, string xmlTransform)
        {
            TaskUtil task = new TaskUtil(this);
            return task.TransformData(userId, taskGuid, xmlTransform);
        }

        public async void TransformDataAsync(string userId, string taskGuid, string xmlTransform, Action<string> callback)
        {
            TaskUtil task = new TaskUtil(this);
            await task.TransformDataAsync(userId, taskGuid, xmlTransform, callback);
        }

    }

    public class EnumeratorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var layer = value as Layer;
            if (layer != null && targetType == typeof(IEnumerable<Layer>))
            {
                return new[] { layer };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // throw new NotImplementedException();
            return null;
        }
    }
}

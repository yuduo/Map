using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Windows;

namespace HongLi.MapControl.Layers.Support
{
    /// <summary>
    /// 聚合对象
    /// </summary>
    public class ClusterItem
    {
        public GraphicCollection Graphics { get; set; }
        
        private int _size = 25;

        private MapPoint _center;

        public bool IsObsolete { get; set; }

        public Point ScreenLocation { get; set; }

        private readonly int _totalCount;

        public ClusterItem(ViewBase map, Graphic g,int totalCount)
        {
            _totalCount = totalCount;
            Graphics = new GraphicCollection {new Graphic(g.Geometry, g.Attributes, g.Symbol)};
            var mapPoint = g.Geometry as MapPoint;
            if (mapPoint != null)
                _center = new MapPoint(mapPoint.X, mapPoint.Y);
            ScreenLocation= map.LocationToScreen(_center);
        }

        /// <summary>
        /// 判断两个聚合对象是否需要被聚合
        /// </summary>
        /// <param name="ci"></param>
        /// <param name="buffer">缓冲区</param>
        /// <returns></returns>
        public bool IncludeCluster( ClusterItem ci,double buffer)
        {
            var pt1 = ci.ScreenLocation;

            var pt2 = ScreenLocation;

            var dis = Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));

            return dis <= buffer + 1.0*(_size+ci._size)/2;
        }

        /// <summary>
        /// 将聚合对象加入到聚合中
        /// </summary>
        /// <param name="ci"></param>
        public void AddGraphics(ClusterItem ci)
        {
            double x = 0, y = 0, sx = ScreenLocation.X * Graphics.Count, sy = ScreenLocation.Y * Graphics.Count;
            foreach (var g in ci.Graphics)
            {
                Graphics.Add(g);
            }
            sx += ci.ScreenLocation.X * ci.Graphics.Count;
            sy += ci.ScreenLocation.Y * ci.Graphics.Count;
            ci.Graphics.Clear();

            
           
            foreach (var g in Graphics)
            {
                var point = g.Geometry as MapPoint;
                if (point != null) x += point.X;
                var mapPoint = g.Geometry as MapPoint;
                if (mapPoint != null) y += mapPoint.Y;
            }
            _center = new MapPoint(x / Graphics.Count, y / Graphics.Count);
            ScreenLocation = new Point(sx/Graphics.Count,sy/Graphics.Count);
            _size = 25 + Graphics.Count / 20;
        }

        /// <summary>
        /// 获取聚合图形，用户地图展示
        /// </summary>
        /// <returns></returns>
        public Graphic GetGraphic()
        {
            if (Graphics.Count == 1)
            {
                return Graphics[0];
            }
            var g = new Graphic {Geometry = _center};

            var cs = new CompositeSymbol();
            var sms = new SimpleMarkerSymbol
            {
                Outline = new SimpleLineSymbol(),
                Size = _size
            };

            //聚合数量占比超过30%时，显示为红色
            if (100.0*Graphics.Count/ _totalCount > 30)
            {
                sms.Color = System.Windows.Media.Color.FromArgb(100, 255, 0, 0);
            }
            else if (100.0 * Graphics.Count/ _totalCount > 5)
            {
                sms.Color = System.Windows.Media.Color.FromArgb(100, 0, 0, 255);
            }
            else
            {
                sms.Color = System.Windows.Media.Color.FromArgb(100, 0, 255, 0);
            }
            
            sms.Outline.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
            sms.Outline.Width = 1;
            cs.Symbols.Add(sms);

            var ts = new TextSymbol
            {
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Baseline,
                Font =
                {
                    FontSize = 14,
                    FontWeight = SymbolFontWeight.Bold
                },
                Text = Graphics.Count.ToString()
            };
            cs.Symbols.Add(ts);

            g.Symbol = cs;

            return g;
        }
    }
}

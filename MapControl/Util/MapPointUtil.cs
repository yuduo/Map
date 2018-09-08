using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using HongLi.MapControl.Behavior;
using HongLi.MapControl.Util.TaskSupport;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// 点工具类
    /// </summary>
    public class MapPointUtil
    {
        public Action<string> Callback { get; set; }

        public List<Graphic> MapPoints { get; set; }

        /// <summary>
        /// 所在图层
        /// </summary>
        public string LName { get; set; }

        /// <summary>
        /// 自动缩放
        /// </summary>
        public bool AutoZoom { get; set; }

        /// <summary>
        /// 自动居中
        /// </summary>
        public bool AutoPan { get; set; }

        public Envelope PointsEnvelope { get; set; }

        /// <summary>
        /// 聚合
        /// </summary>
        public bool IsCluster { get; set; }

        public string Id { get; set; }

        public MapPointUtil(TaskEventArgs args, Action<string> callback)
        {
            Callback = callback;

            Id = args.DataGuid;

            LName = args.GetString("//LName");
            AutoZoom = args.GetString("//AutoZoom").Trim().ToLower() == "1";
            AutoPan = args.GetString("//AutoPan").Trim().ToLower() == "1";
            IsCluster = args.GetString("//IsCluster").Trim().ToLower() == "1";

            XmlNodeList pts = args.GetXmlNodes("//PData");
            if (pts == null)
            {
                return;
            }

            MapPoints = new List<Graphic>();
            foreach (XmlNode n in pts)
            {
                var tp = GetGraphic(n);

                if (tp == null)
                {
                    continue;
                }
                var ms = GetPointSymbol(n);
                if (ms == null)
                {
                    continue;
                }
                tp.Symbol = ms;
                MapPoints.Add(tp);
            }
            if (MapPoints.Count < 1)
            {
                return;
            }
            double xMax, yMax;
            MapPoint p = MapPoints[0].Geometry as MapPoint;
            if (p == null) return;
            var xMin = xMax = p.X;
            var yMin = yMax = p.Y;
            foreach (var g in MapPoints)
            {
                p = g.Geometry as MapPoint;
                if (p != null && p.X < xMin)
                {
                    xMin = p.X;
                }
                if (p != null && p.X > xMax)
                {
                    xMax = p.X;
                }
                if (p != null && p.Y < yMin)
                {
                    yMin = p.Y;
                }
                if (p != null && p.Y > yMax)
                {
                    yMax = p.Y;
                }
            }

            PointsEnvelope = new Envelope(xMin, yMin, xMax, yMax);
        }

        private Graphic GetGraphic(XmlNode data)
        {
            var xd = new XmlDocument();
            var g = new Graphic();

            try
            {
                xd.LoadXml(data.OuterXml);
                g.Geometry = new MapPoint(TaskEventArgs.GetDouble(xd, "//LON"), TaskEventArgs.GetDouble(xd, "//LAT"));

                foreach (XmlNode n in data.ChildNodes)
                {
                    g.Attributes[n.Name] = n.InnerText;
                }
                g.Attributes["_data"] = data.InnerXml;
            }
            catch (Exception ee)
            {
                LogUtil.Error(GetType(), ee);
                g = null;
            }
            return g;
        }

        private MarkerSymbol GetPointSymbol(XmlNode data)
        {
            var xd = new XmlDocument();
            xd.LoadXml(data.OuterXml);

            MarkerSymbol ms;
            try
            {
                var icon = TaskEventArgs.GetString(xd, "//Icon");
                if (icon != "")
                {
                    ms = new PictureMarkerSymbol();
                    ((PictureMarkerSymbol)ms).SetSource(ImageUtil.ImageToBytes(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Asset\\Icon\\" + icon + ".png")));
                    ((PictureMarkerSymbol)ms).Width = ((PictureMarkerSymbol)ms).Height = 24;
                }
                else
                {
                    ms = new SimpleMarkerSymbol();
                    var c = ColorTranslator.FromHtml(TaskEventArgs.GetString(xd, "//Color"));
                    ((SimpleMarkerSymbol)ms).Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                    ((SimpleMarkerSymbol)ms).Size = 5;
                    ((SimpleMarkerSymbol)ms).Outline = new SimpleLineSymbol
                    {
                        Color = System.Windows.Media.Color.FromArgb((byte)(double.Parse(TaskEventArgs.GetString(xd, "//Transpatecy")) * 255), c.R, c.G, c.B)
                        ,
                        Style = SimpleLineStyle.Solid,
                        Width = ((SimpleMarkerSymbol)ms).Size
                    };
                }
            }
            catch (Exception ee)
            {
                ms = null;
                LogUtil.Error(GetType(), ee);
            }
            return ms;
        }

        public void Draw(Map map)
        {
            map.Dispatcher.Invoke(
                            new Action(
                                async delegate
                                {
                                    map.RemoveBehavior(Id);
                                    map.AddSingletonBehavior(new HitTestBehavior(Id, map, map.GetGraphicLayer(LName), Callback));

                                    map.GetGraphicLayer(LName).Labeling.IsEnabled = true;
                                    map.GetGraphicLayer(LName).AddLabel("Label");


                                    map.GetGraphicLayer(LName).IsCluster = IsCluster;
                                    map.GetGraphicLayer(LName).ClusterDistance = 15;

                                    map.ClearGraphic(LName);
                                    foreach (Graphic g in MapPoints)
                                    {
                                        map.AddGraphic(g, LName);
                                    }
                                    GisUtil util = new GisUtil(map);
                                    if (AutoZoom)
                                    {
                                        await util.Locate(PointsEnvelope, 20);
                                    }
                                }
                            )
                        );

        }
        
    }
}

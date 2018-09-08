using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using HongLi.MapControl.Util.TaskSupport;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace HongLi.MapControl.Util.TrackSupport
{
    /// <summary>
    /// 轨迹模型
    /// </summary>
    public class TrackModel
    {
        public Action<string> Callback { get; set; }

        public List<Graphic> TrackPoints { get; set; }

        public Graphic TrackLine { get; set; }

        public TrackModel(TaskEventArgs args,Action<string> callback)
        {
            Callback = callback;
            TrackPoints = new List<Graphic>();
            TrackLine = new Graphic {Geometry = new Polyline(new PointCollection())};

            PolylineBuilder polylineBuilder = new PolylineBuilder(TrackLine.Geometry as Polyline);

            XmlNodeList pts = args.GetXmlNodes("//PLocation");
            if (pts == null)
            {
                return;
            }

            var sms = GetPointSymbol(args.GetXmlNode("//LSymbol"));
            foreach (XmlNode n in pts)
            {
                var tp = GetGraphic(n);
                tp.Symbol = sms;
                TrackPoints.Add(tp);
                polylineBuilder.AddPoint(tp.Geometry as MapPoint);
            }

            TrackLine.Geometry = polylineBuilder.ToGeometry();
            TrackLine.Symbol = GetTrackSymbol(args.GetXmlNode("//LSymbol"));

            
            GetIcon(args.GetXmlNode("//PData"));
        }

        private Image _startIcon;

        public PictureMarkerSymbol StartSymbol
        {
            get
            {
                PictureMarkerSymbol ps = new PictureMarkerSymbol();
                ps.SetSource(ImageUtil.ImageToBytes(_startIcon));
                ps.Width = ps.Height = 24;
                return ps;
            }
        }

        private Image _icon;

        public Image Icon => _icon;


        private void GetIcon(XmlNode data)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(data.OuterXml);
            _startIcon =Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Asset\\Icon\\"+ TaskEventArgs.GetString(xd, "//StartIcon")+".png");
            _icon = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Asset\\Icon\\" + TaskEventArgs.GetString(xd, "//Icon")+".png");
        }

        private SimpleLineSymbol GetTrackSymbol(XmlNode data)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(data.OuterXml);

            var sls = new SimpleLineSymbol();
            try
            {
                Color c;
                switch(TaskEventArgs.GetString(xd, "//LStyle"))
                {
                    case "solid":
                        sls = new SimpleLineSymbol();
                        c = ColorTranslator.FromHtml(TaskEventArgs.GetString(xd, "//LColor"));
                        sls.Color = System.Windows.Media.Color.FromArgb((byte)(double.Parse(TaskEventArgs.GetString(xd, "//Transpatecy")) * 255), c.R, c.G, c.B);
                        sls.Width = double.Parse(TaskEventArgs.GetString(xd, "//LWight"));
                        break;
                    case "dash":
                        sls = new SimpleLineSymbol {Style = SimpleLineStyle.Dash};
                        c = ColorTranslator.FromHtml(TaskEventArgs.GetString(xd, "//LColor"));
                        sls.Color = System.Windows.Media.Color.FromArgb((byte)(double.Parse(TaskEventArgs.GetString(xd, "//Transpatecy")) * 255), c.R, c.G, c.B);
                        sls.Width = double.Parse(TaskEventArgs.GetString(xd, "//LWight"));
                        break;
                    default:
                        sls = new SimpleLineSymbol();
                        c = ColorTranslator.FromHtml(TaskEventArgs.GetString(xd, "//LColor"));
                        sls.Color = System.Windows.Media.Color.FromArgb((byte)(double.Parse(TaskEventArgs.GetString(xd, "//Transpatecy")) * 255), c.R, c.G, c.B);
                        sls.Width = double.Parse(TaskEventArgs.GetString(xd, "//LWight"));
                        break;
                }
            }
            catch (Exception ee)
            {
                LogUtil.Error(GetType(), ee);
            }

            return sls;
        }

        private SimpleMarkerSymbol GetPointSymbol(XmlNode data)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(data.OuterXml);
            SimpleMarkerSymbol sms = new SimpleMarkerSymbol();
            try
            {
                Color c = ColorTranslator.FromHtml(TaskEventArgs.GetString(xd, "//LColor"));
                sms.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                sms.Size = 5;
                sms.Outline = new SimpleLineSymbol {
                    Color = System.Windows.Media.Color.FromArgb((byte)(double.Parse(TaskEventArgs.GetString(xd, "//Transpatecy")) * 255), c.R, c.G, c.B)
                    , Style = SimpleLineStyle.Solid, Width = sms.Size
                };
            }
            catch (Exception ee)
            {
                LogUtil.Error(GetType(), ee);
            }
            return sms;
        }

        private Graphic GetGraphic(XmlNode data)
        {
            var xd = new XmlDocument();            
            var g = new Graphic();

            try
            {
                xd.LoadXml(data.OuterXml);
                g.Geometry = new MapPoint(TaskEventArgs.GetDouble(xd, "//LON"), TaskEventArgs.GetDouble(xd, "//LAT"));
                g.Attributes["ID"] = TaskEventArgs.GetGuid(xd, "//Guid");
                g.Attributes["Lon"] = TaskEventArgs.GetDouble(xd, "//LON");
                g.Attributes["Lat"] = TaskEventArgs.GetDouble(xd, "//LAT");
                g.Attributes["Label"] = TaskEventArgs.GetString(xd, "//Label");
                g.Attributes["Time"] = TaskEventArgs.GetTime(xd, "//Time").ToString();
                g.Attributes["Speed"] = TaskEventArgs.GetDouble(xd, "//Speed");
                g.Attributes["Status"] = TaskEventArgs.GetString(xd, "//Status");
                g.Attributes["Icon"] = TaskEventArgs.GetString(xd, "//Icon");
                g.Attributes["Remark"] = TaskEventArgs.GetString(xd, "//Remark");
                g.Attributes["Angle"] = TaskEventArgs.GetString(xd, "//Angle");
            }
            catch(Exception ee)
            {
                LogUtil.Error(GetType(), ee);
                g = null;
            }
            return g;
        }
    }
}

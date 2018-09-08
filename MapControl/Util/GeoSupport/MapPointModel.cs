using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using HongLi.MapControl.Util.TaskSupport;
using System;
using System.Xml;

namespace HongLi.MapControl.Util.GeoSupport
{
    /// <summary>
    /// 点对象
    /// </summary>
    public class MapPointModel
    {
        public Graphic G { get; set; }

        public MapPointModel(XmlNode data)
        {
            
            XmlDocument xd = new XmlDocument();
            G = new Graphic();

            try
            {
                xd.LoadXml(data.OuterXml);
                G.Geometry = new MapPoint(TaskEventArgs.GetDouble(xd, "//LON"), TaskEventArgs.GetDouble(xd, "//LAT"));

                foreach(XmlNode n in xd.ChildNodes)
                {
                    G.Attributes[n.Name] = n.InnerText;
                }



                //G.Attributes["ID"] = TaskEventArgs.GetGuid(xd, "//Guid");
                //G.Attributes["Lon"] = TaskEventArgs.GetDouble(xd, "//LON");
                //G.Attributes["Lat"] = TaskEventArgs.GetDouble(xd, "//LAT");
                //G.Attributes["Label"] = TaskEventArgs.GetString(xd, "//Label");
                //G.Attributes["Time"] = TaskEventArgs.GetTime(xd, "//Time").ToString();
                //G.Attributes["Speed"] = TaskEventArgs.GetDouble(xd, "//Speed");
                //G.Attributes["Status"] = TaskEventArgs.GetString(xd, "//Status");
                //G.Attributes["Icon"] = TaskEventArgs.GetString(xd, "//Icon");
                //G.Attributes["Remark"] = TaskEventArgs.GetString(xd, "//Remark");
                //G.Attributes["Angle"] = TaskEventArgs.GetString(xd, "//Angle");
            }
            catch (Exception ee)
            {
                LogUtil.Error(GetType(), ee);
                G = null;
            }
        }
    }
}

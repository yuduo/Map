using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using HongLi.MapControl.Component;
using HongLi.MapControl.Util;
using HongLi.MapControl.Util.TaskSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HongLi.MapControl.Behavior
{
    /// <summary>
    /// 用于选中一个Graphic的行为
    /// </summary>
    public class HitTestBehavior : BaseBehavior, IBehavior
    {
        public bool Once { get; set; }

        public GraphicsLayer Layer { get; set; }

        public Map MapObj { get; set; }

        public HitTestBehavior(string id, Map map, GraphicsLayer layer, Action<string> callback) : base(id, callback)
        {
            MapObj = map;
            Layer = layer;
            Once = false;
        }

        public async void Work(object obj)
        {
            try
            {
                var args = obj as MapViewInputEventArgs;
                if (args == null) return;
                MapObj.MapView.Overlays.Items.Clear();

                MapViewInputEventArgs e = args;
                var graphics = await Layer.HitTestAsync(MapObj.MapView, e.Position, 1) as List<Graphic>;
                if (graphics == null || !graphics.Any())
                {
                    return;
                }
                var message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \"" + Id + "\" >";

                foreach (var g in graphics)
                {
                    message += "<PData>";

                    message += g.Attributes["_data"];

                    message += "</PData>";
                }
                message += "</Document>";
                Callback?.Invoke(message);

                if (graphics.Count != 1)
                {
                    return;
                }

                var xd = new XmlDocument();
                xd.LoadXml(message);

                if (TaskEventArgs.GetXmlNode(xd, "//PData//Callout") == null)
                {
                    return;
                }

                var point = graphics[0].Geometry as MapPoint;
                if (point != null)
                {
                    ShowCallout(TaskEventArgs.GetString(xd, "//PData//Callout//Title")
                        , TaskEventArgs.GetString(xd, "//PData//Callout//Content"), point);
                }
            }
            catch (Exception ee)
            {
                LogUtil.Error(GetType(), ee);
            }
        }

        private void ShowCallout(string title, string content, MapPoint location)
        {
            var c = new Callout
            {
                CalloutTitle = title,
                CalloutContent = content
            };
            OverlayItemsControl f = new OverlayItemsControl();
            f.Items.Add(c);

            MapObj.MapView.Overlays.Items.Add(c);
            ViewBase.SetViewOverlayAnchor(c, location);
        }

    }
}

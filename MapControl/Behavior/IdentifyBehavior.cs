using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Tasks.Query;
using HongLi.MapControl.Util;
using System;
using System.Collections.Generic;
using System.Xml;

namespace HongLi.MapControl.Behavior
{
    /// <summary>
    /// 点查询
    /// </summary>
    public class IdentifyBehavior : BaseBehavior, IBehavior
    {
        private readonly Map _map;

        public List<SingleIdentify> Identifys { get; set; }

        /// <summary>
        /// 只执行一次
        /// </summary>
        public bool Once { get; set; }

        public IdentifyBehavior(string id, XmlNode node, Map map, Action<string> callback) : base(id, callback)
        {
            _map = map;
            if (node.Attributes != null) Once = node.Attributes["Once"].Value == "TRUE";
            Identifys = new List<SingleIdentify>();
            foreach (XmlNode n in node.ChildNodes)
            {
                Identifys.Add(new SingleIdentify(n));
            }
        }

        public async void Work(object obj)
        {
            _map.ClearGraphic("Highlight");
            if (Once)
            {
                IsObsolete = true;
            }
            var message = "";
            var args = obj as MapViewInputEventArgs;
            if (args != null)
            {
                var e = args;


                message = "<Document TaskGuid=\"" + Map.TaskGuid + "\" DataGuid = \"数据标识\" DataType=\"Identify\">";

                var util = new GisUtil(_map);

                if (Identifys != null)
                {
                    foreach (SingleIdentify si in Identifys)
                    {
                        var result = await util.DoIdentify(si.Service, si.Layer, e.Location);

                        if (result == null)
                        {
                            continue;
                        }

                        if (result.Results != null && result.Results.Count > 0)
                        {
                            message += "<Service ID=\"" + si.Service + "\">";
                            foreach (IdentifyItem item in result.Results)
                            {
                                _map.AddGraphic(new Graphic(item.Feature.Geometry, item.Feature.Attributes), "Highlight");

                                message += "<Item LayerID=\"" + item.LayerID + "\" LayerName=\"" + item.LayerName + "\">";
                                foreach (KeyValuePair<string, object> kv in item.Feature.Attributes)
                                {
                                    message += string.Format("<{0}>{1}</{0}>", kv.Key, kv.Value);
                                }
                                message += "</Item>";
                            }
                            message += "</Service>";
                        }
                        //break;
                    }
                }

                message += "</Document>";
            }

            Callback?.Invoke(message);


        }

        //private  Task<IdentifyResult> QueryLayer(string layerID, MapPoint pt, string layers)
        //{
        //    LayerObject lo = null;
        //    for (int i = 0; i < ConfigUtil.DynamicLayer.Count; i++)
        //    {
        //        if (layerID != ConfigUtil.DynamicLayer[i].ID)
        //        {
        //            continue;
        //        }
        //        lo = ConfigUtil.DynamicLayer[i];
        //    }
        //    if (lo == null)
        //    {
        //        return null;
        //    }

        //    //创建查询IdentifyTask
        //    IdentifyTask identify = new IdentifyTask(lo.Uri);


        //    if (lo.Token != null)
        //    {
        //        identify.Token = lo.Token;
        //    }

        //    //设置查询参数
        //    IdentifyParameters QueryPara = new IdentifyParameters(pt, mapView.Extent, 7
        //        , (int)mapView.ActualHeight, (int)mapView.ActualWidth);
        //    QueryPara.LayerOption = LayerOption.Visible;

        //    string[] layerArr = layers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //    int? layerId = null;
        //    List<int> ids = new List<int>();
        //    foreach (string layer in layerArr)
        //    {
        //        layerId = lo.GetLayerId(layer);
        //        if (layerId != null)
        //        {
        //            ids.Add((int)layerId);
        //        }
        //    }
        //    QueryPara.LayerIDs = ids;

        //    QueryPara.SpatialReference = mapView.SpatialReference;
        //    return identify.ExecuteAsync(QueryPara);
        //}
    }

    public class SingleIdentify
    {
        public string Service { get; set; }

        public string Layer { get; set; }

        public SingleIdentify(XmlNode node)
        {
            if (node.Attributes != null)
            {
                Service = node.Attributes["Service"].Value;
                Layer = node.Attributes["Layer"].Value;
            }
        }
    }
}

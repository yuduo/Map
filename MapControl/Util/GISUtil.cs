using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Tasks.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// GIS工具集
    /// </summary>
    public class GisUtil
    {
        public Map Map { get; set; }

        public GisUtil(Map map)
        {
            Map = map;
        }

        public Task<bool> Locate(double x, double y, double scale)
        {
            return Map.MapView.SetViewAsync(new MapPoint(x, y), scale, new TimeSpan(1000));
        }

        public Task<bool> Locate(Geometry geo, double scale)
        {
            var point = geo as MapPoint;
            return point != null ? Map.MapView.SetViewAsync(point, scale, new TimeSpan(1000)) : Map.MapView.SetViewAsync(geo, new TimeSpan(1000), new System.Windows.Thickness(scale));
        }

        public Task<QueryResult> DoQuery(string serviceId, string layer, Geometry geometry, string where)
        {
            LayerObject lo = null;
            foreach (var t in ConfigUtil.DynamicLayer)
            {
                if (serviceId != t.Id)
                {
                    continue;
                }
                lo = t;
            }
            if (lo == null)
            {
                return null;
            }
            var layerId = lo.GetLayerId(layer);
            var task = layerId != null ? new QueryTask(new Uri(lo.Uri + "/" + layerId)) : new QueryTask(lo.Uri);
            if (lo.Token != null)
            {
                task.Token = lo.Token;
            }
            var query = new Query("1=1");
            if (geometry != null)
            {
                query.Geometry = geometry;
            }
            if (where.Trim() != "")
            {
                query.Where = where;
            }
            query.ReturnGeometry = true;
            return task.ExecuteAsync(query);
        }

        public Task<FindResult> DoFind(string serviceId, Geometry geometry,string fields, string value)
        {
            LayerObject lo = null;
            foreach (var t in ConfigUtil.DynamicLayer)
            {
                if (serviceId != t.Id)
                {
                    continue;
                }
                lo = t;
            }
            if (lo == null)
            {
                return null;
            }
            var findTask = new FindTask(lo.Uri);
            if (lo.Token != null)
            {
                findTask.Token = lo.Token;
            }

            //初始化FindParameters参数
            FindParameters findParameters = new FindParameters();
            string[] Layers = lo.Layers.Split('、');
            if (Layers != null && Layers.Length > 0)
            {
                foreach (var item in Layers)
                {
                    var layerId = lo.GetLayerId(item);
                    if (layerId != null)
                        findParameters.LayerIDs.Add(Convert.ToInt16(layerId)); //查找的图层
                }
            }
            findParameters.SearchFields.Add(fields); //查找的字段范围
            findParameters.ReturnGeometry = true;
            findParameters.Contains = false;
            findParameters.SearchText = value; //查找的“属性值”
            
            //异步执行
            return findTask.ExecuteAsync(findParameters);
        }

        public Task<IdentifyResult> DoIdentify(string serviceId, string layers, MapPoint pt)
        {
            LayerObject lo = null;
            foreach (var t in ConfigUtil.DynamicLayer)
            {
                if (serviceId != t.Id)
                {
                    continue;
                }
                lo = t;
            }
            if (lo == null)
            {
                return null;
            }

            //创建查询IdentifyTask
            IdentifyTask task = new IdentifyTask(lo.Uri);


            if (lo.Token != null)
            {
                task.Token = lo.Token;
            }

            //设置查询参数
            var queryPara = new IdentifyParameters(pt, Map.MapView.Extent, 7
                , (int)Map.MapView.ActualHeight, (int)Map.MapView.ActualWidth)
            { LayerOption = LayerOption.Visible };

            var layerArr = layers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var ids = (from layer in layerArr select lo.GetLayerId(layer) into layerId where layerId != null select (int)layerId).ToList();
            queryPara.LayerIDs = ids;

            queryPara.SpatialReference = Map.MapView.SpatialReference;
            return task.ExecuteAsync(queryPara);
        }
    }
}

using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Tasks.Query;
using HongLi.MapControl.Behavior;
using HongLi.MapControl.Util.TaskSupport;
using HongLi.MapControl.Util.TrackSupport;
using System;
using System.Threading.Tasks;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// 任务工具类，用于调度控件任务
    /// </summary>
    public class TaskUtil
    {
        private readonly Map _map;

        public TaskUtil(Map map)
        {
            _map = map;
        }

        public int SetData(string userId, string taskGuid, string dataGuid, string dataType, string xmlData)
        {
            var args = new SetDataEventArgs(userId, taskGuid, dataGuid, dataType, xmlData);
            if (!args.Valid)
            {
                return -1;
            }

            switch (args.DataType)
            {
                case "SetExtent":
                    var pt = new MapPoint(args.GetDouble("//LON"), args.GetDouble("//LAT"));
                    _map.MapView.SetViewAsync(pt, _map.GetScale(args.GetLong("//Level")), new TimeSpan(1000));
                    break;
                case "ClearGraphic":
                    switch (args.GetString("//Target"))
                    {
                        case "Highlight":
                            _map.ClearGraphic("Highlight");
                            break;
                        case "Drawing":
                            _map.ClearGraphic("Drawing");
                            break;
                    }
                    break;
                case "DrawPicture":
                    _map.AddGraphic(new Graphic(new MapPoint(args.GetDouble("//LON"), args.GetDouble("//LAT"))), "Drawing", ImageUtil.ImageToBytes(ImageUtil.Base64ToImage(args.GetString("//Picture"))));
                    break;
            }

            return 1;
        }

        public Task<string> SetDataAsync(string userId, string taskGuid, string dataGuid, string dataType, string xmlData, Action<string> callback)
        {
            GisUtil util;
            var args = new SetDataEventArgs(userId, taskGuid, dataGuid, dataType, xmlData);


            return Task.Run(async () =>
            {
                if (!args.Valid)
                {
                    return "";
                }

                switch (args.DataType)
                {
                    case "SetExtent":
                        _map.Dispatcher.Invoke(
                            new Action(
                                async delegate
                                {
                                    util = new GisUtil(_map);
                                    await util.Locate(args.GetDouble("//LON"), args.GetDouble("//LAT"), _map.GetScale(args.GetLong("//Level")));

                                }
                            )
                        );
                        break;
                    case "LocateObject":
                        util = new GisUtil(_map);
                        QueryResult res = await util.DoQuery(args.GetString("//Service"), args.GetString("//Layer"), null, args.GetString("//Where"));
                        if (res.FeatureSet.Features.Count < 1)
                        {
                            return "";
                        }
                        _map.Dispatcher.Invoke(
                            new Action(
                                async delegate
                                {
                                    _map.ClearGraphic("Highlight");
                                    _map.AddGraphic(new Graphic(res.FeatureSet.Features[0].Geometry, res.FeatureSet.Features[0].Attributes), "Highlight");

                                    util = new GisUtil(_map);
                                    await util.Locate(res.FeatureSet.Features[0].Geometry, _map.GetScale(args.GetLong("//Level")));

                                }
                            )
                        );
                        break;
                    case "LocateObjectAll":
                        util = new GisUtil(_map);
                        FindResult find = await util.DoFind(args.GetString("//Service"), null, args.GetString("//Fields"), args.GetString("//Value"));
                        if (find.Results.Count < 1)
                        {
                            return "";
                        }
                        _map.Dispatcher.Invoke(
                            new Action(
                                async delegate
                                {
                                    _map.ClearGraphic("Highlight");
                                    _map.AddGraphic(new Graphic(find.Results[0].Feature.Geometry, find.Results[0].Feature.Attributes), "Highlight");

                                    util = new GisUtil(_map);
                                    await util.Locate(find.Results[0].Feature.Geometry, _map.GetScale(args.GetLong("//Level")));

                                }
                            )
                        );
                        break;
                    case "HisLocation":
                        TrackModel tm = new TrackModel(args, callback);
                        _map.SetTrack(tm);
                        break;
                    case "DrawPointCollection":
                        MapPointUtil mu = new MapPointUtil(args, callback);
                        mu.Draw(_map);
                        break;
                }

                return "1";
            });


        }

        public string GetData(string userId, string taskGuid, string dataGuid, string dataType)
        {
            return "";
        }

        public Task<string> GetDataAsync(string userId, string taskGuid, string dataGuid, string dataType, Action<string> callback)
        {

            return Task.Run(() =>
            {
                GetDataEventArgs args = new GetDataEventArgs(userId, taskGuid, dataGuid, dataType);
                if (!args.Valid)
                {
                    return "";
                }
                switch (args.DataType)
                {
                    case "GetMapPoint":
                        _map.AddSingletonBehavior(new GetMapPointBehavior(args.DataGuid, callback));
                        break;
                    case "DrawMapPoint":
                        new MapDrawBehavior(_map, args.DataType, args.DataGuid, callback).Work(DrawShape.Point);
                        break;
                    case "DrawPolyline":
                        new MapDrawBehavior(_map, args.DataType, args.DataGuid, callback).Work(DrawShape.Polyline);
                        break;
                    case "DrawPolygon":
                        new MapDrawBehavior(_map, args.DataType, args.DataGuid, callback).Work(DrawShape.Polygon);
                        break;
                    case "DrawCircle":
                        new MapDrawBehavior(_map, args.DataType, args.DataGuid, callback).Work(DrawShape.Circle);
                        break;
                    case "GetMapView":
                        new GetMapViewBehavior(args.DataGuid, callback).Work(_map.MapView.Extent);
                        break;
                    case "DrawRectangle":
                        new MapDrawBehavior(_map, args.DataType, args.DataGuid, callback).Work(DrawShape.Rectangle);
                        break;
                }

                return "";
            });
        }

        public string TransformData(string userId, string taskGuid, string xmlTransform)
        {
            return "";
        }

        public Task<string> TransformDataAsync(string userId, string taskGuid, string xmlTransform, Action<string> callback)
        {
            return Task.Run(() =>
            {
                TransformDataEventArgs args = new TransformDataEventArgs(userId, taskGuid, xmlTransform);
                if (!args.Valid)
                {
                    return "参数错误";
                }
                try
                {
                    switch (args.DataType)
                    {
                        case "IdentifyByClick":
                            _map.AddSingletonBehavior(new IdentifyBehavior(args.DataGuid, args.GetXmlNode("//Identifys"), _map, callback));
                            break;
                    }
                }
                catch
                {
                    return "参数错误";
                }
                return "";
            });
        }


    }
}

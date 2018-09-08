using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using HongLi.MapControl.Layers.Support;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HongLi.MapControl.Layers
{
    /// <summary>
    /// 可聚合图层
    /// </summary>
    public class OverlayLayer : GraphicsLayer
    {
        public Map MapObj { get; set; }

        public OverlayLayer(Map mapObj)
        {
            RenderingMode = GraphicsRenderingMode.Static;
            MapObj = mapObj;
            MapObj.MapView.ExtentChanged += Map_ExtentChanged;

        }

        /// <summary>
        /// 前一次缩放的比例，用于判断若比例不变的话不进行重新聚合，以提高效率
        /// </summary>
        private double _lastScale = -1;

        /// <summary>
        /// 地图边界盒改变，进行聚合操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_ExtentChanged(object sender, EventArgs e)
        {
            if (!IsCluster)
            {
                return;
            }

            if (Math.Abs(_lastScale - MapObj.MapView.Scale) < 0.001)
            {
                return;
            }
            _lastScale = MapObj.MapView.Scale;

            if (_originalGraphics == null)
            {
                _originalGraphics = new GraphicCollection();
                foreach (var g in Graphics)
                {
                    _originalGraphics.Add(new Graphic(g.Geometry, g.Attributes, g.Symbol));
                }
            }
            Graphics.Clear();

            var cs = new List<ClusterItem>();
            foreach (var g in _originalGraphics)
            {
                if (!(g.Geometry is MapPoint))
                {
                    continue;
                }
                if (((MapPoint)g.Geometry).X < MapObj.MapView.Extent.XMin
                    || ((MapPoint)g.Geometry).X > MapObj.MapView.Extent.XMax
                    || ((MapPoint)g.Geometry).Y < MapObj.MapView.Extent.YMin
                    || ((MapPoint)g.Geometry).Y > MapObj.MapView.Extent.YMax)
                {
                    continue;
                }
                cs.Add(new ClusterItem(MapObj.MapView, g, _originalGraphics.Count));
            }
            //如果地图已经放到最大那么就不再聚合
            if (MapObj.TiledInfo != null && MapObj.TiledInfo.Lods.Count > 0 && Math.Abs(MapObj.TiledInfo.Lods[MapObj.TiledInfo.Lods.Count - 1].Scale - MapObj.MapView.Scale) < 0.001)
            {
                foreach (var ci in cs)
                {
                    Graphics.Add(ci.GetGraphic());
                }
                return;
            }

            _clusterThread?.Abort();

            _clusterThread = new Thread(DoCluster);
            _clusterThread.Start(cs);
        }
        
        /// <summary>
        /// 聚合线程
        /// </summary>
        private Thread _clusterThread;

        /// <summary>
        /// 聚合计算
        /// </summary>
        /// <param name="param"></param>
        private void DoCluster(object param)
        {
            List<ClusterItem> cs = param as List<ClusterItem>;

            for (;;)
            {
                //递归，完成聚合计算
                if (!DoSingleCluster(cs))
                {
                    break;
                }
            }
            Dispatcher.Invoke(delegate
            {
                Graphics.Clear();
                if (cs != null)
                    foreach (ClusterItem ci in cs)
                    {
                        //System.Windows.Point pt =Map.LocationToScreen(ci.GetGraphic().Geometry as MapPoint);
                        //Graphic g= ci.GetGraphic();
                        //g.Attributes["Label"] = pt.X + "," + pt.Y;
                        Graphics.Add(ci.GetGraphic());
                    }
            });
        }

        /// <summary>
        /// 单次聚合操作
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        private bool DoSingleCluster(List<ClusterItem> cs)
        {
            bool tag = false;

            for (int i = cs.Count - 1; i >= 0; i--)
            {
                if (cs[i].IsObsolete)
                {
                    continue;
                }
                for (int j = i - 1; j >= 0; j--)
                {
                    if (cs[j].IsObsolete)
                    {
                        continue;
                    }

                    if (cs[j].IncludeCluster(cs[i], ClusterDistance))
                    {
                        cs[j].AddGraphics(cs[i]);
                        cs[i].IsObsolete = true;
                        tag = true;
                        break;
                    }
                }
            }

            for (int i = cs.Count - 1; i >= 0; i--)
            {
                if (cs[i].IsObsolete)
                {
                    cs.RemoveAt(i);
                }
            }
            return tag;
        }

        /// <summary>
        /// 聚合标签
        /// </summary>
        /// <param name="labelField"></param>
        public void AddLabel(string labelField)
        {
            AttributeLabelClass alc = new AttributeLabelClass
            {
                TextExpression = "[" + labelField + "]",
                Symbol = new TextSymbol
                {
                    Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 255),
                    Font = { FontFamily = "宋体" }
                }
            };
            //解决中文乱码
            Labeling.LabelClasses.Add(alc);
        }

        public void ClearLabel()
        {
            Labeling.LabelClasses.Clear();
        }

        private bool _isCluster;


        private GraphicCollection _originalGraphics;

        public void ClearGraphics()
        {
            Graphics.Clear();
            _originalGraphics = null;
        }

        /// <summary>
        /// 是否聚合
        /// </summary>
        public bool IsCluster
        {
            get
            {
                return _isCluster;
            }
            set
            {
                _isCluster = value;
                if (_isCluster) return;
                Graphics.Clear();
                if (_originalGraphics == null)
                {
                    return;
                }
                foreach (Graphic g in _originalGraphics)
                {
                    Graphics.Add(g);
                }
                _originalGraphics = null;
            }
        }

        /// <summary>
        /// 聚合像素距离
        /// </summary>
        public int ClusterDistance { get; set; }


    }
}

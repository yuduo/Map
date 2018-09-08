using Esri.ArcGISRuntime.ArcGISServices;
using Esri.ArcGISRuntime.Geometry;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// 图层对象
    /// </summary>
    public class LayerObject
    {
        private readonly JObject _obj;

        public LayerObject(JObject obj)
        {
            _obj = obj;
        }

        /// <summary>
        /// 服务标识
        /// </summary>
        public string Id => _obj?["id"].ToString() ?? "";

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name
        {
            get
            {
                if (_obj == null)
                {
                    return "";
                }
                return _obj["name"].ToString();
            }
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Layers
        {
            get
            {
                if (_obj == null)
                {
                    return "";
                }
                return _obj["layers"].ToString();
            }
        }

        /// <summary>
        /// 是否允许点查询
        /// </summary>
        public bool Identify
        {
            get
            {
                if (_obj?["identify"] == null)
                {
                    return false;
                }
                return _obj["identify"].ToString().Trim().ToLower() == "true";
            }
        }

        /// <summary>
        /// 令牌，用户需要授权的地图服务
        /// </summary>
        public string Token
        {
            get
            {
                if (_obj?["token"] == null)
                {
                    return null;
                }
                return ConfigUtil.GetToken(_obj["token"].ToString());
            }
        }

        /// <summary>
        /// 服务地址
        /// </summary>
        public Uri Uri
        {
            get
            {
                if (_obj == null)
                {
                    return null;
                }
                string url = _obj["url"].ToString().Trim();
                if (url == "")
                {
                    return null;
                }
                return new Uri(url);
            }
        }

        /// <summary>
        /// 服务信息
        /// </summary>
        public MapServiceInfo ServiceInfo { get; set; }

        /// <summary>
        /// 根据名称获取图层编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int? GetLayerId(string name)
        {
            foreach (LayerServiceInfo l in ServiceInfo.Layers)
            {
                if (l.Name == name)
                {
                    return l.ID;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 配置工具类
    /// </summary>
    internal class ConfigUtil
    {

        private static JObject _config;

        public static void Load(string path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs);
            var value = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            _config = JObject.Parse(value);
        }

        public static double MinScale
        {
            get
            {
                if (_config == null)
                {
                    return 0;
                }
                return _config["MinScale"].ToDouble();
            }
        }

        public static double MaxScale
        {
            get
            {
                if (_config == null)
                {
                    return 0;
                }
                return _config["MaxScale"].ToDouble();
            }
        }

        public static double InitScale
        {
            get
            {
                if (_config == null)
                {
                    return 0;
                }
                return _config["InitScale"].ToDouble();
            }
        }

        public static MapPoint InitCenter => _config == null ? new MapPoint(0, 0) : new MapPoint(_config["InitCenter"]["X"].ToDouble(), _config["InitCenter"]["Y"].ToDouble());

        public static LayerObject TiledLayer
        {
            get
            {
                var obj = _config?["TiledLayer"] as JObject;
                return obj == null ? null : new LayerObject(obj);
            }
        }
        
        public static string GetToken(string id)
        {
            var arr = _config["Tokens"] as JArray;
            if (arr == null || arr.Count < 1)
            {
                return null;
            }
            JObject tokenConfig = null;
            foreach (var jToken in arr)
            {
                var obj = (JObject) jToken;
                if (obj["id"].ToString() == id)
                {
                    tokenConfig = obj;
                }
            }
            if (tokenConfig == null)
            {
                return null;
            }
            var result="";
            var myEncoding = Encoding.UTF8;
            const string sMode = "POST";
            var sUrl = tokenConfig["url"].ToString();
            var sPostData = "request=gettoken&username="+ tokenConfig["user"]+ "&password="+ tokenConfig["password"] + "&expiration="+ tokenConfig["expiration"];
            const string sContentType = "application/x-www-form-urlencoded";

            try
            {
                // init
                var req = WebRequest.Create(sUrl) as HttpWebRequest;
                if (req != null)
                {
                    req.Method = sMode;
                    req.Accept = "*/*";
                    req.KeepAlive = false;
                    req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                    if (0 == string.CompareOrdinal("POST", sMode))
                    {
                        var bufPost = myEncoding.GetBytes(sPostData);
                        req.ContentType = sContentType;
                        req.ContentLength = bufPost.Length;
                        var newStream = req.GetRequestStream();
                        newStream.Write(bufPost, 0, bufPost.Length);
                        newStream.Close();
                    }

                    // Response
                    var res = req.GetResponse() as HttpWebResponse;
                    try
                    {
                        //OutLog("Response.ContentLength:\t{0}", res.ContentLength);
                        //OutLog("Response.ContentType:\t{0}", res.ContentType);
                        //OutLog("Response.CharacterSet:\t{0}", res.CharacterSet);
                        //OutLog("Response.ContentEncoding:\t{0}", res.ContentEncoding);
                        //OutLog("Response.IsFromCache:\t{0}", res.IsFromCache);
                        //OutLog("Response.IsMutuallyAuthenticated:\t{0}", res.IsMutuallyAuthenticated);
                        //OutLog("Response.LastModified:\t{0}", res.LastModified);
                        //OutLog("Response.Method:\t{0}", res.Method);
                        //OutLog("Response.ProtocolVersion:\t{0}", res.ProtocolVersion);
                        //OutLog("Response.ResponseUri:\t{0}", res.ResponseUri);
                        //OutLog("Response.Server:\t{0}", res.Server);
                        //OutLog("Response.StatusCode:\t{0}\t# {1}", res.StatusCode, (int)res.StatusCode);
                        //OutLog("Response.StatusDescription:\t{0}", res.StatusDescription);

                        //// header
                        //OutLog(".\t#Header:");  // 头.
                        //for (int i = 0; i < res.Headers.Count; ++i)
                        //{
                        //    OutLog("[{2}] {0}:\t{1}", res.Headers.Keys[i], res.Headers[i], i);
                        //}

                        // 找到合适的编码
                        //encoding = Encoding_FromBodyName(res.CharacterSet);	// 后来发现主体部分的字符集与Response.CharacterSet不同.
                        //if (null == encoding) encoding = myEncoding;
                        var encoding = Encoding.UTF8;
                        System.Diagnostics.Debug.WriteLine(encoding);

                        // body
                        //OutLog(".\t#Body:");    // 主体.
                        if (res != null)
                            using (var resStream = res.GetResponseStream())
                            {
                                if (resStream != null)
                                    using (var resStreamReader = new StreamReader(resStream, encoding))
                                    {
                                        result = resStreamReader.ReadToEnd();
                                    }
                            }
                        //OutLog(".\t#OK.");  // 成功.
                    }
                    finally
                    {
                        res?.Close();
                    }
                }
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        public static LayerObject ImageLayer
        {
            get
            {
                var obj = _config?["ImageLayer"] as JObject;
                return obj == null ? null : new LayerObject(obj);
            }
        }

        private static List<LayerObject> _dynamicLayer;
        public static List<LayerObject> DynamicLayer
        {
            get
            {
                if (_config == null)
                {
                    return null;
                }
                if (_dynamicLayer != null)
                {
                    return _dynamicLayer;
                }
                var arr = _config["DynamicLayer"] as JArray;
                if (arr == null || arr.Count < 1)
                {
                    return null;
                }
                _dynamicLayer = new List<LayerObject>();
                foreach (var jToken in arr)
                {
                    var obj = (JObject) jToken;
                    _dynamicLayer.Add(new LayerObject(obj));
                }
                return _dynamicLayer;
            }
        }

    }
}

using System;
using System.Xml;

namespace HongLi.MapControl.Util.TaskSupport
{
    public class TaskEventArgs : EventArgs
    {
        public string UserId { get; set; }
        public string TaskGuid { get; set; }

        public string DataGuid { get; set; }

        public XmlDocument Data { get; set; }

        public TaskEventArgs(string userId, string taskGuid,string dataGuid, string xmlData)
        {
            UserId = userId;
            TaskGuid = taskGuid;
            DataGuid = dataGuid;
            Data = new XmlDocument();

            if (xmlData.Trim() == "") return;
            try
            {
                Data.LoadXml(xmlData);
            }
            catch
            {
                Data = null;
            }
        }

        public double GetDouble(string path)
        {
            return GetDouble(Data, path);
        }

        public static double GetDouble(XmlDocument xd, string path)
        {
            var node = xd?.SelectSingleNode(path);
            if (node == null)
            {
                return 0;
            }
            if (node.Attributes != null && "SINGLE" != node.Attributes["Type"].Value)
            {
                return 0;
            }
            try
            {
                return double.Parse(node.InnerText);
            }
            catch
            {
                return 0;
            }
        }

        public int? GetLong(string path)
        {
            var node = Data?.SelectSingleNode(path);
            if (node == null)
            {
                return null;
            }
            if (node.Attributes != null && "LONG" != node.Attributes["Type"].Value)
            {
                return null;
            }
            try
            {
                return int.Parse(node.InnerText);
            }
            catch
            {
                return null;
            }
        }

        public string GetString(string path)
        {
            return GetString(Data, path);
        }

        public static string GetString(XmlDocument xd, string path)
        {
            var node = xd?.SelectSingleNode(path);
            if (node == null)
            {
                return "";
            }
            if (node.Attributes != null && "TEXT" != node.Attributes["Type"].Value)
            {
                return "";
            }
            try
            {
                return node.InnerText;
            }
            catch
            {
                return "";
            }
        }

        public string GetGuid(string path)
        {
            return GetGuid(Data, path);
        }

        public static string GetGuid(XmlDocument xd, string path)
        {
            var node = xd?.SelectSingleNode(path);
            if (node == null)
            {
                return "";
            }
            if (node.Attributes != null && "GUID" != node.Attributes["Type"].Value)
            {
                return "";
            }
            try
            {
                return node.InnerText;
            }
            catch
            {
                return "";
            }
        }

        public DateTime? GetTime(string path)
        {
            return GetTime(Data, path);
        }

        public static DateTime? GetTime(XmlDocument xd, string path)
        {
            var node = xd?.SelectSingleNode(path);
            if (node == null)
            {
                return null;
            }
            if (node.Attributes != null && "DATE" != node.Attributes["Type"].Value)
            {
                return null;
            }
            try
            {
                return  DateTime.Parse(node.InnerText);
            }
            catch
            {
                return null;
            }
        }

        public static XmlNode GetXmlNode(XmlDocument xd, string path)
        {
            return xd?.SelectSingleNode(path);
        }

        public XmlNode GetXmlNode(string path)
        {
           return GetXmlNode(Data, path);
        }

        public XmlNodeList GetXmlNodes(string path)
        {
            return Data?.SelectNodes(path);
        }

    }
}

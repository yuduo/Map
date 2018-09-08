using HongLi.MapControl.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace WpfMapControlBasedOnArcGIS.Core
{
    public class LogWriter : ILog
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public void Error(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public void Error(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public void Debug(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Debug("Debug", ex);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public void Debug(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Debug(msg);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public void Warn(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Warn("Debug", ex);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public void Warn(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Warn(msg);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public void Fatal(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Fatal("Fatal", ex);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public void Fatal(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Fatal(msg);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public void Info(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Info("Info", ex);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public void Info(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Info(msg);
        }
    }
}

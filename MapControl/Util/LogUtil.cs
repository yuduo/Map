using System;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// 日志工具类接口
    /// </summary>
    public interface ILog
    {
        void Error(Type t, Exception ex);

        void Error(Type t, string msg);

        void Debug(Type t, Exception ex);

        void Debug(Type t, string msg);

        void Warn(Type t, Exception ex);

        void Warn(Type t, string msg);

        void Fatal(Type t, Exception ex);

        void Fatal(Type t, string msg);

        void Info(Type t, Exception ex);

        void Info(Type t, string msg);
    }

    /// <summary>
    /// 日志工具类
    /// </summary>
    public class LogUtil
    {

        private static ILog _log;

        public static ILog Log
        {
            set
            {
                if (_log == null)
                {
                    _log = value;
                }
            }
            get
            {
                return _log;
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void Error(Type t, Exception ex)
        {
            _log?.Error(t, ex);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Error(Type t, string msg)
        {
            _log?.Error(t, msg);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void Debug(Type t, Exception ex)
        {
            _log?.Debug(t, ex);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Debug(Type t, string msg)
        {
            _log?.Debug(t, msg);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void Warn(Type t, Exception ex)
        {
            _log?.Warn(t, ex);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Warn(Type t, string msg)
        {
            _log?.Warn(t, msg);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void Fatal(Type t, Exception ex)
        {
            _log?.Fatal(t, ex);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Fatal(Type t, string msg)
        {
            _log?.Fatal(t, msg);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void Info(Type t, Exception ex)
        {
            _log?.Info(t, ex);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void Info(Type t, string msg)
        {
            _log?.Info(t, msg);
        }
    }
}

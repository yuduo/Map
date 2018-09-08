using System;

namespace HongLi.MapControl.Behavior
{
    /// <summary>
    /// 行为基类
    /// </summary>
    public class BaseBehavior
    {
        /// <summary>
        /// 行为编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 行为已经过期（过期的行为将被清除）
        /// </summary>
        public bool IsObsolete { get; set; }

        /// <summary>
        /// 回调方法，用于异步消息传递
        /// </summary>
        public Action<string> Callback { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public BaseBehavior(string id,Action<string> callback)
        {
            Id = id;
            Callback = callback;
        }
    }
}

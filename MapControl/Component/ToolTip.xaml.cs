namespace HongLi.MapControl.Component
{
    /// <summary>
    /// 提示消息（轨迹播放中应用）
    /// </summary>
    public partial class ToolTip
    {
        public ToolTip()
        {
            InitializeComponent();
        }

        public object Tip
        {
            get
            {
                return TipLabel.Content;
            }
            set
            {
                TipLabel.Content = value;
            }
        }
    }
}

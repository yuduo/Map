namespace HongLi.MapControl.Util.TaskSupport
{
    public class TransformDataEventArgs: TaskEventArgs
    {
        public string XmlTransform { get; set; }

        public TransformDataEventArgs(string userId, string taskGuid, string xmlTransform)
            :base(userId, taskGuid,"",xmlTransform)
        {
            XmlTransform = xmlTransform;            
        }
        
        public bool Valid
        {
            get
            {
                if (Data == null)
                {
                    return false;
                }
                if (TaskGuid != Map.TaskGuid)
                {
                    return false;
                }

                if (TaskGuid != DataTaskGuid)
                {
                    return false;
                }

                return true;
            }
        }

        public string DataType
        {
            get
            {
                try
                {
                    var xmlAttributeCollection = Data.ChildNodes[0].Attributes;
                    if (xmlAttributeCollection != null) return xmlAttributeCollection["DataType"].Value;
                }
                catch
                {
                    return "";
                }
                return "";
            }
        }

        private string DataTaskGuid
        {
            get
            {
                try
                {
                    var xmlAttributeCollection = Data.ChildNodes[0].Attributes;
                    if (xmlAttributeCollection != null) return xmlAttributeCollection["TaskGuid"].Value;
                }
                catch
                {
                    return "";
                }
                return "";
            }
        }
       
    }
}

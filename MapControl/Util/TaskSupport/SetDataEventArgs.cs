namespace HongLi.MapControl.Util.TaskSupport
{
    public class SetDataEventArgs : TaskEventArgs
    {
        public string DataType { get; set; }

        public SetDataEventArgs(string userId, string taskGuid, string dataGuid, string dataType, string xmlData)
            :base(userId,taskGuid,dataGuid,xmlData)
        {
            DataGuid = dataGuid;
            DataType = dataType;
            
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

                return DataDataType == DataType;
            }
        }

        private string DataDataType
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

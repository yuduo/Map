namespace HongLi.MapControl.Util.TaskSupport
{
    public class GetDataEventArgs: TaskEventArgs
    {
        public string DataType { get; set; }

        public GetDataEventArgs(string userId, string taskGuid, string dataGuid, string dataType)
            : base(userId, taskGuid,dataGuid, "")
        {
            DataGuid = dataGuid;
            DataType = dataType;
        }

        public bool Valid => TaskGuid == Map.TaskGuid;
    }
}

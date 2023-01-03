namespace ApiEngine.Options;

public class AppInfoOptions : IConfigurableOptions
{
    public bool GlobalAuthorize { get; set; }
    public LogClass Log { get; set; } = new();

    public class LogClass
    {
        public LogTypeEnum LogType { get; set; } = LogTypeEnum.File;
        public bool Request { get; set; }
        public bool Response { get; set; }
        public List<string> IgnoreMethods { get; set; } = new();
        public List<string> IgnorePaths { get; set; } = new();
        public LogDbSetClass LogDbSet { get; set; }

        public class LogDbSetClass
        {
            public string TableName { get; set; }
            public int KeepMonths { get; set; }
        }
    }
}

/// <summary>
///     日志类型
/// </summary>
public enum LogTypeEnum
{
    File,
    Seq,
    Db
}
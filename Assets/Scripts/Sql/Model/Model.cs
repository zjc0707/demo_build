public class Model
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FileUrlWindows { get; set; }
    public string FileUrlMac { get; set; }
    public int ModelTypeId { get; set; }
    public long Size { get; set; }
    public long DeployTime { get; set; }
}
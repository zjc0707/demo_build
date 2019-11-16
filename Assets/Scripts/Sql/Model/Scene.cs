public class Scene : BaseModel
{
    public string Name { get; set; }
    public byte[] Content { get; set; }
    public int UserId { get; set; }
    public long DeployTime { get; set; }
}
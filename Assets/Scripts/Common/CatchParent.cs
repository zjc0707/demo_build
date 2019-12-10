public class CatchParent : BaseUniqueObject<CatchParent>
{
    private void Awake()
    {
        this.transform.position = new UnityEngine.Vector3(this.transform.position.x, 0, this.transform.position.z);
    }
}
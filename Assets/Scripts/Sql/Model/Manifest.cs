using System.Collections.Generic;
public class Manifest
{
    public List<ModelType> ModelType { get; set; }
    public List<Model> Models { get; set; }
    public Manifest()
    {
        Models = new List<Model>();
        ModelType = new List<ModelType>();
    }
}
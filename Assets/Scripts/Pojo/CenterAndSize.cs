using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CenterAndSize
{
    public Vector3 Center { get; set; }
    public Vector3 Size { get; set; }

    public override string ToString()
    {
        return "Center:" + Center + "_Size:" + Size;
    }
}

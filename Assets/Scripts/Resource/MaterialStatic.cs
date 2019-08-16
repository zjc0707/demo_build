using System;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialStatic
{
    private static Material _TRANSLUCENT_BLUE;
    public static Material TRANSLUCENT_BLUE
    {
        get
        {
            if(_TRANSLUCENT_BLUE == null)
            {
                _TRANSLUCENT_BLUE = Resources.Load("Material/Translucent/Blue") as Material;
            }
            return _TRANSLUCENT_BLUE;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;


public static class MapHelper 
{
    public static int TransormX_ToMapX(float XPos)
    {
        return (Convert.ToInt16(XPos)) + Spawner.Instance.HalfHeightMap;
    }

    public static int TransormZ_ToMapY(float ZPos)
    {
        return (Convert.ToInt16(ZPos)) - Spawner.Instance.HalfWidthMap;
    }

    public static int MapFreeFieldCount()
    {
        int fields = 0;
        foreach (var item in Spawner.Instance.GenerateMap)
        {
            if(int.TryParse(item.ToString(), out int result))
            {
                fields++;
            }
        }
        return fields;
    }
}

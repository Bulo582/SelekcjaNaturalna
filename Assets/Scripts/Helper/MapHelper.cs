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

    public static int MapFreeFieldCount(ref List<MapFieldInfo> mapFieldInfos)
    {
        char[,] map = Spawner.Instance.originalMap;
        int fields = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (int.TryParse(map[i, j].ToString(), out int result))
                {
                    mapFieldInfos.Add(new MapFieldInfo(i,j,true));
                    fields++;
                }
                else
                {
                    mapFieldInfos.Add(new MapFieldInfo(i, j, false));
                }
            }
        }
        return fields;
    }

}


public struct MapFieldInfo
{
    public MapFieldInfo(int i, int j, bool isGround)
    {
        this.i = i;
        this.j = j;
        this.isGround = isGround;
    }

    public int i;
    public int j;
    public bool isGround;
}

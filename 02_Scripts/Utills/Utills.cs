using System;
using UnityEngine;

public static class Utills
{
    public static void DestroyAllChildren(this Transform parent)
    {
        // 자식 오브젝트 수만큼 반복하면서 각 자식을 삭제합니다.
        // 역순으로 반복하는 것은 중간에 자식이 삭제되면서 인덱스가 변경되는 것을 방지하기 위함입니다.
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            // 자식 오브젝트를 가져온 후 삭제합니다.
            Transform child = parent.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject); // 오브젝트를 파괴합니다.
        }
    }

    public static int GetFlagIndex(Enum enumValue)
    {
        int value = Convert.ToInt32(enumValue);
        if (value == 0 || (value & (value - 1)) != 0)
        {
            return -1;
        }

        return (int)Mathf.Log(value, 2);
    }
}

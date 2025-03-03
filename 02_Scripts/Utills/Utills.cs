using System;
using UnityEngine;

public static class Utills
{
    public static void DestroyAllChildren(this Transform parent)
    {
        // �ڽ� ������Ʈ ����ŭ �ݺ��ϸ鼭 �� �ڽ��� �����մϴ�.
        // �������� �ݺ��ϴ� ���� �߰��� �ڽ��� �����Ǹ鼭 �ε����� ����Ǵ� ���� �����ϱ� �����Դϴ�.
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            // �ڽ� ������Ʈ�� ������ �� �����մϴ�.
            Transform child = parent.GetChild(i);
            GameObject.DestroyImmediate(child.gameObject); // ������Ʈ�� �ı��մϴ�.
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

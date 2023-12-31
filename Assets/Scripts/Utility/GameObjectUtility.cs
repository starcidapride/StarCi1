﻿using UnityEngine;

public class GameObjectUtility
{
    public static Transform GetClosestSiblingGameObject(Transform transform, bool older = false)
    {
        var parent = transform.transform.parent;
        var index = transform.transform.GetSiblingIndex();
        var siblingIndex = older ? index + 1 : index - 1;

        if (siblingIndex < 0 || siblingIndex >= parent.childCount) return null;

        return parent.GetChild(siblingIndex);
    }
    public static void RemoveAllChildGameObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Object.Destroy(child.gameObject);
        }
    }
    public static Transform GetChildByPosition(Transform parent, ChildPosition childPosition)
    {
        if (parent.childCount == 0) return null;

        return childPosition == ChildPosition.Youngest ? parent.GetChild(parent.childCount - 1) : parent.GetChild(0);
    }

    public static Vector2 GetMousePos()
    {
        var mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public static Vector2 GetMousePositionRelativeToRectTransform(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, GetMousePos(), null, out Vector2 localPoint);
        return localPoint * rect.localScale;
    }

    public static bool IsPositionInsideRectTransformArea(Vector2 position, RectTransform area)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(area, position, null, out Vector2 localPosition))
        {
            return false;
        }

        var rect = area.rect;
        return rect.Contains(localPosition);
    }

    public static bool IsMousePositionInsideRectTransformArea(RectTransform area)
    {
        return IsPositionInsideRectTransformArea(GetMousePos(), area);
    }


    public static Transform GetParentGameObjectInHierarchy(Transform transform, int generation)
    {
        if (generation < 1) return null;
        Transform parent = transform.transform;
        for (int i = 0; i < generation; i++)
        {
            parent = parent.parent;
        }
        return parent;
    }

    public static void DestroyAllChildGameObjects(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i).gameObject;
            Object.Destroy(child);
        }
    }

    public static void SetInteractability(Transform transform, bool state = true)
    {
        var canvasGroup = transform.GetComponent<CanvasGroup>();

        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public static void SetVisibility(Transform transform, bool state = true)
    {
        var canvasGroup = transform.GetComponent<CanvasGroup>();

        canvasGroup.alpha = state ? 1 : 0;
    }
}
public enum ChildPosition
{
    Oldest,
    Youngest
}
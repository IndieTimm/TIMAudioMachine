using System.Collections.Generic;
using UnityEngine;

public static class IdentificatedObjectHelper
{
    public static string GetPath(GameObject gameObject)
    {
        var stack = new Stack<string>();
        var root = gameObject;

        for (int i = 0; i < 64; i++)
        {
            var identificatedObject = root?.GetComponent<IIdentificatedObject>();

            if (identificatedObject == null)
            {
                break;
            }

            stack.Push(identificatedObject.GetId());

            root = root.FindParentWithComponent<IIdentificatedObject>();
        }

        return string.Join("/", stack);
    }
}

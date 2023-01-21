using System.Collections.Generic;
using UnityEngine;

public static class GameSceneUtility
{
    public static GameObject Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player") ?? throw new System.Exception("Player wasn't been found");
            }

            return player;
        }
    }

    private static GameObject player;

    /// <summary>
    /// Find for an active active and inactive objects of type T in active scene.
    /// </summary>
    public static List<T> FindObjects<T>()
    {
        var targets = new List<T>();
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            targets.AddRange(gameObject.GetComponentsInChildren<T>(true));
        }

        return targets;
    }

    public static GameObject FindParentWithComponent<T>(this GameObject root)
    {
        if (root == null)
        {
            return null;
        }

        var parent = root.transform.parent;

        while (true)
        {
            if (parent == null)
            {
                return null;
            }

            var component = parent.GetComponent<T>();

            if (component != null)
            {
                return parent.gameObject;
            }

            parent = parent.parent;
        }
    }
}
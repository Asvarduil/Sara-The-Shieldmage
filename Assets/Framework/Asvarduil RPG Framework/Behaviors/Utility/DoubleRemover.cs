using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class DoubleRemover : DebuggableBehavior
{
    #region Variables / Properties

    #endregion Variables / Properties

    #region Hooks

    [ContextMenu("Remove Doubles")]
    public void RemoveDoubles()
    {
        List<GameObject> children = GetChildren();
        List<GameObject> duplicates = FindPositionalDuplicates(children);
        RemoveDuplicates(duplicates);
    }

    #endregion Hooks

    #region Methods

    private List<GameObject> GetChildren()
    {
        int childCount = transform.childCount;
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < childCount; i++)
        {
            Transform current = transform.GetChild(i);
            GameObject currentObject = current.gameObject;

            children.Add(currentObject);
        }

        DebugMessage("Found " + children.Count + " child objects.");

        return children;
    }

    private List<GameObject> FindPositionalDuplicates(List<GameObject> objectList)
    {
        List<GameObject> duplicates = new List<GameObject>();

        for (int i = 0; i < objectList.Count; i++)
        {
            GameObject currentObject = objectList[i];
            Vector3 position = currentObject.transform.position;

            for(int j = i + 1; j < objectList.Count; j++)
            {
                GameObject testObject = objectList[j];
                Vector3 testPosition = testObject.transform.position;

                if (Vector3.Distance(position, testPosition) < 0.01f)
                    duplicates.Add(testObject);
            }
        }

        DebugMessage("Found " + duplicates.Count + " duplicates to remove.");
        return duplicates;
    }

    private void RemoveDuplicates(List<GameObject> duplicates)
    {
        for(int i = 0; i < duplicates.Count; i++)
        {
            GameObject current = duplicates[i];
            GameObject.DestroyImmediate(current);
        }

        DebugMessage("Duplicates removed.  There are " + transform.childCount + " children remaining.");
    }

    #endregion Methods
}

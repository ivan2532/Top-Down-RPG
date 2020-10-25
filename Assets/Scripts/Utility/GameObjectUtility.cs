using System.Collections;
using UnityEngine;

public class GameObjectUtility
{
    public static IEnumerator DisableGameObjectDelayed(GameObject targetObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        targetObject.SetActive(false);
    }
}

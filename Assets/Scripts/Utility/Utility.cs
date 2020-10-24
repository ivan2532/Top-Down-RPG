using System.Collections;
using UnityEngine;

public class Utility
{
    public static IEnumerator DisableGameObjectDelayed(GameObject targetObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        targetObject.SetActive(false);
    }
}


using UnityEngine;

namespace ExtensionFunction
{
    public static class TransformExtension
    {
        public static void Clear(this Transform trm)
        {
            foreach(Transform child in trm)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}

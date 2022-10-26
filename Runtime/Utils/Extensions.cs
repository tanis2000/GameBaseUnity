using UnityEngine;

namespace GameBase.Utils
{
    public static class Extensions
    {
        public static Vector3 FlatY(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }
        
        public static Vector3 FlatZ(this Vector3 v)
        {
            return new Vector3(v.x, v.y, 0);
        }
    
        public static Vector3 AsVector3(this Vector2Int v)
        {
            return new Vector3(v.x, 0, v.y);
        }
        
        public static Vector3 WithZ(this Vector2 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ArchaeaMod
{
    internal class HelperUtil
    {
        public static Vector2 OrbitAngle(Vector2 origin, float angle, float radius)
        {
            float cos = (float)(radius * Math.Cos(angle));
            float sine = (float)(radius * 1.5f * Math.Sin(angle));
            return origin + new Vector2(cos, sine);
        }
    }
}

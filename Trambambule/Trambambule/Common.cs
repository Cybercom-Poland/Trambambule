using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trambambule
{
    public class Common
    {
        public enum EResult
        {
            Loose = 0,
            Win = 1,
            Draw = 2
        }

        public enum EPosition
        {
            Offence = 1,
            Defence = 0
        }

        public static Common.EResult GetResult(int g1, int g2)
        {
            if (g1 > g2) return Common.EResult.Win;
            if (g1 < g2) return Common.EResult.Loose;
            return Common.EResult.Draw;
        } 
    }
}
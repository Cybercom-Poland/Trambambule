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

        public static EResult GetResult(int goals1, int goals2)
        {
            if (goals1 > goals2) return Common.EResult.Win;
            if (goals1 < goals2) return Common.EResult.Loose;
            return Common.EResult.Draw;
        } 

        public static bool CompareNames(Player p, string prefix)
        {
            return CreateTextCompareString(p.FirstName + p.LastName)
                .Contains(CreateTextCompareString(prefix));
        }

        public static string CreateTextCompareString(string text)
        {
            text = text.ToLower()
                .Replace("ą", "a")
                .Replace("ę", "e")
                .Replace("ó", "o")
                .Replace("ł", "l")
                .Replace("ś", "s")
                .Replace("ć", "c")
                .Replace("ż", "z")
                .Replace("ź", "z")
                .Replace("ń", "n")
                .Replace(" ", "");
            return text;
        }
    }
}
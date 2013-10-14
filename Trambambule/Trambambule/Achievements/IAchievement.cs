using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trambambule.Achievements
{
    public interface IAchievement
    {
        string Label { get; }
        IEnumerable<Player> Calculate();
    }
}

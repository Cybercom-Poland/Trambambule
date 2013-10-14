using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trambambule.Achievements;

namespace Trambambule
{
    public partial class Player
    {
        private IList<IAchievement> _achievement;

        public IList<IAchievement> Achievements
        {
            get
            {
                if (_achievement == null)
                {
                    _achievement = new List<IAchievement>();
                }
                return _achievement;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaPlusPlus.Helpers
{
    class TimeHelper
    {
        public static String toHHMMSS(int time)
        {
            int hours = time / 3600;
            int minutes = (time - (hours * 3600)) / 60;
            int seconds = time - (hours * 3600) - (minutes * 60);

            string hoursStr = hours.ToString();
            string minutesStr = minutes.ToString();
            string secondsStr = seconds.ToString();

            if (hours < 10) hoursStr = "0" + hoursStr;
            if (minutes < 10) minutesStr = "0" + minutesStr;
            if (seconds < 10) secondsStr = "0" + secondsStr;


            return hoursStr + ":" + minutesStr + ":" + secondsStr;
        }
    }
}

/*
*   Author  :   JackerKun
*   Date    :   2023年4月10日 星期一 下午6:16:45
*   About   :
*/

using System.Collections.Generic;

namespace DevChartSpider
{
    public class MdlBaseInfo
    {
        public MdlBaseInfo()
        {
            Films = new List<MdlFilm>();
            Developers = new List<MdlDeveloper>();
        }
        public List<MdlFilm> Films{get;set;}
        public List<MdlDeveloper> Developers { get; set; }
    }
}
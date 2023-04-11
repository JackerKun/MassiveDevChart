/*
*   Author  :   JackerKun
*   Date    :   2023年4月11日 星期二 上午9:55:34
*   About   :
*/

using System.Collections.Generic;

namespace DevChartSpider
{
    public class Config
    {
        /// <summary>
        /// 基本信息配置文件
        /// </summary>
        public static string constFile = "config.json";

        /// <summary>
        /// 缓存目录
        /// </summary>
        public static string filmsFolder = "Films";

        /// <summary>
        /// 存储Csv地址
        /// </summary>
        public static string csvFile = "data.csv";

        /// <summary>
        /// 表头
        /// </summary>
        public static string[] csvHeader = new[]
            {"Film", "Developer", "Dilution", "ISO", "35MM", "120", "Sheet", "Temp", "Notes"};

        /// <summary>
        /// 主域名
        /// </summary>
        public static string urlBase = "http://www.digitaltruth.com/devchart.php";

        /// <summary>
        /// 
        /// </summary>
        public static string urlGetChart = $"{urlBase}?Developer=&mdc=Search&TempUnits=C&TimeUnits=T&Film=";

        /// <summary>
        /// 表格数据
        /// </summary>
        public static List<MdlChart> chartInfos = null;

        /// <summary>
        /// 基本信息
        /// </summary>
        public static MdlBaseInfo baseInfos = null;
    }
}
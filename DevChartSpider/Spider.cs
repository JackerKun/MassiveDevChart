/*
*   Author  :   JackerKun
*   Date    :   2023年4月10日 星期一 下午4:55:27
*   About   :
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using Jc.Net;
using Exception = System.Exception;

namespace DevChartSpider
{
    class HttpWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }
    
    internal class Spider
    {

        /// <summary>
        /// 
        /// </summary>
        private JcNet _net = new JcNet();

        /// <summary>
        /// 获取常量
        /// </summary>
        /// <returns></returns>
        public MdlBaseInfo GetEnums()
        {
            List<MdlDeveloper> devs = new List<MdlDeveloper>();
            List<MdlFilm> films = new List<MdlFilm>();
            try
            {
                var data = new HttpWebClient().DownloadString(Config.urlBase);
                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                var filmNodes= doc.DocumentNode.SelectNodes("//*[@id=\"Film\"]//option");
                foreach (var n in filmNodes)
                {
                    var film = new MdlFilm()
                    {
                        filmCode = n.Attributes["value"].Value,
                        filmName = n.InnerText
                    };
                    if (!string.IsNullOrWhiteSpace(film.filmCode))
                    {
                        films.Add(film);
                    }
                }
                var devNodes = doc.DocumentNode.SelectNodes("//*[@id=\"Developer\"]//option");
                foreach (var n in devNodes)
                {
                    var dev = new MdlDeveloper()
                    {
                        DevCode = n.Attributes["value"].Value,
                        DevName = n.InnerText
                    };
                    if (!string.IsNullOrWhiteSpace(dev.DevCode))
                    {
                        devs.Add(dev);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return new MdlBaseInfo() { Films = films,Developers = devs};
        }

        /// <summary>
        /// 根据胶卷名字获取
        /// </summary>
        /// <param name="filmCode"></param>
        /// <returns></returns>
        public  List<MdlChart> GetChartsByFilm(string filmCode)
        {
            List<MdlChart> charts = new List<MdlChart>();
            try
            {
                var data = new HttpWebClient().DownloadString(Config.urlGetChart+HttpUtility.UrlEncode(filmCode));
                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                HtmlNodeCollection tableNodes = doc.DocumentNode.SelectNodes("//*[@id=\"mdcmainbox\"]/table");////*[@id="mdcmainbox"]/table/tbody

                if (tableNodes == null)
                {
                    return charts;
                }
                foreach (HtmlNode table in tableNodes)
                {
                    HtmlNodeCollection tr = table.SelectNodes(".//tr");
                    if (tr==null)
                    {
                        continue;
                    }
                    foreach (HtmlNode t in tr)
                    {
                        HtmlNodeCollection td = t.SelectNodes(".//td");
                        if (td==null)
                        {
                            continue;
                        }

                        MdlChart chart = new MdlChart()
                        {
                            FilmCode = filmCode,
                        };
                        for (int i = 0; i < td.Count(); i++)
                        {
                            chart.FilmName= td[0].InnerText;
                            chart.DeveloperName = td[1].InnerText;
                            chart.Dilution = td[2].InnerText;
                            chart.ISO = td[3].InnerText;
                            chart.Film35= td[4].InnerText;
                            chart.Film120=td[5].InnerText;
                            chart.Sheet=td[6].InnerText;
                            chart.Temp=td[7].InnerText;
                            chart.NotesUrl=td[8].InnerHtml;
                        }
                        charts.Add(chart);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return charts;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string RequestWeb(string url)
        {
            string result = null;
            try
            {
                result = _net.HttpGet(url, 10000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return result;
        }
    }
}
/*
*   Author  :   JackerKun
*   Date    :   2023年4月11日 星期二 上午9:52:30
*   About   :
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace DevChartSpider
{
    public class DevChart
    {
        public DevChart()
        {
            LoadDevChart();
        }

        public string selectFilm;
        
        /// <summary>
        /// 拉取数据
        /// </summary>
        /// <returns></returns>
        public bool GrabDevChart()
        {
            bool result = false;
            try
            {
                Config.baseInfos = new MdlBaseInfo();
                Config.chartInfos = new List<MdlChart>();
                Spider spider = new Spider();
                Console.WriteLine("start load base info");
                if (!File.Exists((Config.constFile)))
                {
                    Config.baseInfos = spider.GetEnums();
                    File.WriteAllText(Config.constFile, JsonConvert.SerializeObject(Config.baseInfos));
                }
                else
                {
                    Config.baseInfos = JsonConvert.DeserializeObject<MdlBaseInfo>(File.ReadAllText(Config.constFile));
                }
                Console.WriteLine("base info load success");

                Console.WriteLine("start download films");
                foreach (var v in Config.baseInfos.Films)
                {
                    Console.WriteLine("download:" + v.filmName);
                    string filmDevFile = Path.Combine(Config.filmsFolder, v.filmName.Replace("/", "_") + ".json");
                    if (File.Exists(filmDevFile))
                    {
                        string fileInfo = File.ReadAllText(filmDevFile);
                        if (string.IsNullOrWhiteSpace(fileInfo))
                        {
                            continue;
                        }
                        List<MdlChart> tmp = JsonConvert.DeserializeObject<List<MdlChart>>(fileInfo);
                        Config.chartInfos.AddRange(tmp);
                        continue;
                    }
                    var filmDev = spider.GetChartsByFilm(v.filmCode);
                    if (filmDev.Any())
                    {
                        Config.chartInfos.AddRange(filmDev);
                        File.WriteAllText(filmDevFile, JsonConvert.SerializeObject(filmDev));
                    }
                }

                Console.WriteLine("download success");
                Console.WriteLine("insert  csv");
                string csvInfo = string.Join(",", Config.csvHeader) + "\r\n";
                foreach (var c in Config.chartInfos)
                {
                    csvInfo +=
                        $"{c.FilmName},{c.DeveloperName},{c.Dilution},{c.ISO},{c.Film35},{c.Film120},{c.Sheet},{c.Temp},{c.NotesUrl}\r\n";
                }
                File.WriteAllText(Config.csvFile, csvInfo);
                Console.WriteLine("insert csv success");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 加载已经缓存的数据
        /// </summary>
        /// <returns></returns>
        public List<MdlChart> LoadDevChart()
        {
            Config.baseInfos = new MdlBaseInfo();
            Config.chartInfos = new List<MdlChart>();
            List<MdlChart> result = new List<MdlChart>();
            try
            {
                Console.WriteLine("start load base info");
                if (File.Exists(Config.constFile))
                {
                    Config.baseInfos  = JsonConvert.DeserializeObject<MdlBaseInfo>(File.ReadAllText(Config.constFile));
                }
                else
                {
                    Console.WriteLine($"load base info error:{Config.constFile} not found");
                    return null;
                }
                
                foreach (var v in Config.baseInfos.Films)
                {
                    string filmDevFile = Path.Combine(Config.filmsFolder, v.filmName.Replace("/","_")+".json");
                    if (File.Exists(filmDevFile))
                    {
                        Console.WriteLine("load:"+v.filmName);
                        string fileInfo = File.ReadAllText(filmDevFile);
                        if (string.IsNullOrWhiteSpace(fileInfo))
                        {
                            continue;
                        }
                        List<MdlChart> tmp= JsonConvert.DeserializeObject<List<MdlChart>>(fileInfo);
                        result.AddRange(tmp);
                        continue;
                    }
                }
                Config.chartInfos = result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 获取基础信息
        /// </summary>
        /// <returns></returns>
        public (List<string>  Films,List<string> Devs) GetBaseInfo()
        {
            List<string> films = null;
            List<string> devs = null;
            try
            {
                if (Config.chartInfos != null)
                {
                    films = Config.chartInfos.OrderBy(p => p.FilmName).Select(p => p.FilmName).Distinct().ToList();
                    devs = Config.chartInfos.OrderBy(p => p.DeveloperName).Select(p => p.DeveloperName).Distinct().ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return (films, devs);
        }
        
        /// <summary>
        /// 根据胶卷查询
        /// </summary>
        /// <param name="filmName"></param>
        /// <returns></returns>
        public List<MdlChart> SelectByFilm(string filmName)
        {
            List<MdlChart> result=null;
            try
            {
                if (Config.chartInfos == null || Config.chartInfos.Count <= 0)
                {
                    return null;
                }
                return Config.chartInfos.Where(p => p.FilmName == filmName).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        /// <summary>
        /// 根据药水查询
        /// </summary>
        /// <param name="devName"></param>
        /// <returns></returns>
        public List<MdlChart> SelectByDev(string devName)
        {
            List<MdlChart> result=null;
            try
            {
                if (Config.chartInfos == null || Config.chartInfos.Count <= 0)
                {
                    return null;
                }
                return Config.chartInfos.Where(p => p.DeveloperName == devName).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
        }

        
        /// <summary>
        /// 根据胶卷和药水查询
        /// </summary>
        /// <param name="filmName"></param>
        /// <param name="devName"></param>
        /// <returns></returns>
        public List<MdlChart> SelectByFilmAndDev(string filmName,string devName)
        {
            List<MdlChart> result=null;
            try
            {
                if (Config.chartInfos == null || Config.chartInfos.Count <= 0)
                {
                    return null;
                }
                return Config.chartInfos.Where(p => p.FilmName == filmName&&p.DeveloperName==devName).ToList();
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
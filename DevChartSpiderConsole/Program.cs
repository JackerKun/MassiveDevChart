// See https://aka.ms/new-console-template for more information

using DevChartSpider;

//使用数据
DevChart devChart = new DevChart();

//网络抓取数据
var GrabData = devChart.GrabDevChart();

//加载缓存数据
var result= devChart.LoadDevChart();

//获取基本信息：胶卷和药水列表
var result2 = devChart.GetBaseInfo();

//获取显影时间表格信息
var result3 = devChart.SelectByFilm("Orwo UN54+");

Console.ReadKey();
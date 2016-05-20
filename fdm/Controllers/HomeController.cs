using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using fdm.Models;

namespace fdm.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ViewResult Index()
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 17 ? "Dzień dobry" : "Dobry wieczór";
            return View();
        }

        [HttpGet]
        public ViewResult Fdm1D()
        {
            FdmForOneDimension fdm1d = new FdmForOneDimension();
            ViewBag.ViewedWithChart = false;
            return View("fdm1D", fdm1d);
        }

        [HttpPost]
        public ViewResult Fdm1D(FdmForOneDimension fdm1d)
        {
            if (ModelState.IsValid)
            {
                double[][] result = fdm1d.OneDimensionInTime(true);
                string[] labels = new string[fdm1d.n];
                List<double>[] dataList = new List<double>[result.Length + 1];
                
                dataList[0] = new List<double>();
                dataList[0].Add(fdm1d.boundaryValueLeft);
                for (int i = 1; i < fdm1d.n - 1; i++)               
                    dataList[0].Add(fdm1d.initialValue);
                dataList[0].Add(fdm1d.boundaryValueRight);

                for (int i = 0; i < fdm1d.iteration; i++)
                {
                    dataList[i + 1] = new List<double>();
                    for (int j = 0; j < fdm1d.n; j++)
                    {
                        labels[j] = (j + 1).ToString();
                        dataList[i + 1].Add(Math.Round(result[i][j], 4));
                    }
                }

                double[] chartValues = { fdm1d.boundaryValueLeft, fdm1d.initialValue, fdm1d.boundaryValueRight };

                ViewBag.ScaleMaxValue = chartValues.Max();
                ViewBag.ScaleStartValue = (int)chartValues.Min();
                ViewBag.Labels = labels;
                ViewBag.ChartData = dataList;
                ViewBag.ViewedWithChart = true;
                
                return View("fdm1D", fdm1d);
            }
            else
            {
                ViewBag.ViewedWithChart = false;
                return View();
            }
        }

        [HttpGet]
        public ViewResult Fdm2D()
        {
            FdmForTwoDimension fdm2d = new FdmForTwoDimension();
            ViewBag.ViewedWithChart = false;
            return View("fdm2D", fdm2d);
        }

        [HttpPost]
        public ViewResult Fdm2D(FdmForTwoDimension fdm2d)
        {
            if (ModelState.IsValid)
            {
                double[][] result = fdm2d.TwoDimensionInSpace(false);

                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(result);
                
                
                
                //string[] labels = new string[fdm2d.n];
                List<double>[] dataList = new List<double>[fdm2d.n + 1];

                //dataList[0] = new List<double>();
                //dataList[0].Add(fdm1d.boundaryValueLeft);
                //for (int i = 1; i < fdm1d.n - 1; i++)
                //    dataList[0].Add(fdm1d.initialValue);
                //dataList[0].Add(fdm1d.boundaryValueRight);

                for (int i = 0; i <= fdm2d.n; i++)
                {
                    dataList[i] = new List<double>();
                    for (int j = 0; j <= fdm2d.n; j++)
                    {
                        //labels[j] = (j + 1).ToString();
                        dataList[i].Add(result[j][i]);
                    }
                }

                //double[] chartValues = { fdm1d.boundaryValueLeft, fdm1d.initialValue, fdm1d.boundaryValueRight };

                //ViewBag.ScaleMaxValue = chartValues.Max();
                //ViewBag.ScaleStartValue = (int)chartValues.Min();
                //ViewBag.Labels = labels;
                ViewBag.data = json;
                ViewBag.ChartData = dataList;
                ViewBag.ViewedWithChart = true;

                return View("fdm2D", fdm2d);
            }
            else
            {
                ViewBag.ViewedWithChart = false;
                return View();
            }
        }
    }
}

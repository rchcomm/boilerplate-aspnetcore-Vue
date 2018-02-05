using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using Vue_by_Telerik.Models;
using Kendo.Mvc.Extensions;

namespace Vue_by_Telerik.Controllers
{
    public class GridController : Controller
    {
		public ActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
		{
            var result = Enumerable.Range(0, 50).Select(i => new OrderViewModel
            {
                OrderID = i,
                Freight = i * 10,
                OrderDate = new DateTime(2016, 9, 15).AddDays(i % 7),
				ShipName = "ShipName " + i,
				ShipCity = "ShipCity " + i
			});

			var dsResult = result.ToDataSourceResult(request);
			return Json(dsResult);
		}
	}
}

﻿using System.Web;
using System.Web.Mvc;

namespace V_Vuelos_Main_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

using System.Collections.Generic;
using T2Access.Web.SPA.VueJs.Models;

namespace T2Access.Web.SPA.VueJs.Providers
{
    public interface IWeatherProvider
    {
        List<WeatherForecast> GetForecasts();
    }
}

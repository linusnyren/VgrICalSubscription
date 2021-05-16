using System;
namespace HeromaVgrIcalSubscription.Options
{
    public class SeleniumOptions
    {
        public string SeleniumDir { get; set; }
        public string TargetUrl { get; set; }
        public string Driver { get; set; }
        public int ServicePort { get; set; }
        public int TimeOut { get; set; }

        public SeleniumOptions()
        {
        }
    }
}

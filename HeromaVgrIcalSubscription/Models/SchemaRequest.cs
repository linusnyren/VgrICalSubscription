using System;
namespace HeromaVgrIcalSubscription.Models
{
    public class SchemaRequest
    {
        public SchemaRequest()
        {
        }

        public string UserName { get; internal set; }
        public string Password { get; internal set; }
        public int Months { get; internal set; }
    }
}

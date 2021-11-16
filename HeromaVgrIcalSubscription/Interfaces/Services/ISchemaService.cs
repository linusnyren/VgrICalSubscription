using System;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Models;
using RestSharp;

namespace HeromaVgrIcalSubscription.Interfaces.Services
{
    public interface ISchemaService
    {
        Task<string> GetCalendarAsync(SchemaRequest req);
    }
}

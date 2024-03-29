﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using HeromaVgrIcalSubscription.Interfaces.Services;
using HeromaVgrIcalSubscription.Models;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace HeromaVgrIcalSubscription.Services
{
    public class SchemaService : ISchemaService
    {
        private readonly ISeleniumTokenService seleniumTokenService;
        private readonly ICalendarService calendarService;
        private readonly ILogger<SchemaService> log;

        public SchemaService(ISeleniumTokenService seleniumTokenService, ICalendarService calendarService, ILogger<SchemaService> log)
        {
            this.seleniumTokenService = seleniumTokenService;
            this.calendarService = calendarService;
            this.log = log;
        }

        public async Task<string> GetCalendarAsync(SchemaRequest req)
        {
            var cookies = seleniumTokenService.GetCookiesAsync(req.UserName, req.Password);
            var res = await calendarService.GetIcalAsync(cookies, req.Months);
            var response = AddReminders(res.Content, req);
            return response;
        }

        private string AddReminders(string icsString, SchemaRequest req)
        {
            icsString = icsString.Replace("END:VTIMEZONE", "END: VTIMEZONE"); //Weird bug in Ical.Net package
            var calendar = Ical.Net.Calendar.Load(icsString);
            var summaryString = $"{req.UserName} schema";
            var quarterAlarm = new Alarm()
            {
                Summary = summaryString,
                Trigger = new Trigger(TimeSpan.FromMinutes(-15)),
                Action = AlarmAction.Display

            };
            var hourAlarm = new Alarm()
            {
                Summary = summaryString,
                Trigger = new Trigger(TimeSpan.FromHours(-1)),
                Action = AlarmAction.Display

            };
            calendar.ProductId = "En tjänst skapad av Linus Nyrén";
            var currentDate = DateTime.Now.ToUniversalTime().AddHours(1);
            foreach (var ev in calendar.Events)
            {
                ev.Summary = summaryString;
                ev.Alarms.Add(quarterAlarm);
                ev.Alarms.Add(hourAlarm);
                ev.GeographicLocation = new GeographicLocation(57.6824618, 11.9614532);
                ev.Location = "Sahlgrenska Universitetssjukhuset 413 45 Göteborg, Sverige";
                ev.Description = $"En tjänst skapad av Linus Nyrén, \nSenaste uppdateringen från Heroma: {currentDate.ToString("HH:mm dd/MM")}";
                ev.Url = new Uri("https://github.com/linusnyren/VgrICalSubscription");
            }
            return new CalendarSerializer().SerializeToString(calendar);
        }
    }
}

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avanade.AzureWorkshop.WebApp.Services
{
    public class TelemetryService
    {
        private readonly TelemetryClient _telemetryClient;

        public TelemetryService()
        {
            _telemetryClient = new TelemetryClient();
        }

        public void Log(string message, string correlationId)
        {
            _telemetryClient.TrackTrace($"{message}, {nameof(correlationId)}: {correlationId}");
        }
    }
}
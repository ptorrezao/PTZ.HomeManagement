using Microsoft.AspNetCore.Http;
using Sentry;
using Sentry.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Utils
{
    public class CustomSentryEventExceptionProcessor : SentryEventExceptionProcessor<Exception>
    {
        protected override void ProcessException(
            Exception exception,
            SentryEvent sentryEvent)
        {
            sentryEvent.AddBreadcrumb("Processor running on special exception.");
        }
    }
}

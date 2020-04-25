using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

//define FunctionsStartup attribute for assembly.
[assembly: FunctionsStartup(typeof(AFDependencyInjection.Startup))]

namespace AFDependencyInjection
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Services are registered here
            builder.Services.AddScoped<IAlertService, EmailAlertService>();
            builder.Services.AddScoped<IAlertService, SmsAlertService>();
        }
    }


}

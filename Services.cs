using Core;
using FileLogging;
using Karambolo.Extensions.Logging.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using TextBlockLogging;

namespace ExceptLog
{
    public static class ServicesRoot
    {
        public static void Register(IConfiguration Configuration, IServiceCollection services, GlobalSettings opt)
        {
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"));
                if (opt.Logging.Box.Info) builder.AddTextBlock<InfoTextBlockLoggerProvider, TextBlockOptions, InfoTextBlockLoggerFormatter>();
                if (opt.Logging.Box.Trace) builder.AddTextBlock<TraceTextBlockLoggerProvider, TextBlockOptions, TraceTextBlockLoggerFormatter>();
                if (opt.Logging.Box.Error) builder.AddTextBlock<ExcTextBlockLoggerProvider, TextBlockOptions, ExcTextBlockLoggerFormatter>();
                if (opt.Logging.File.Error) builder.AddFile<FileLoggerProvider>(configure: o => o.RootPath = AppContext.BaseDirectory);
                if (opt.Logging.File.Trace) builder.AddFile<TraceFileLoggerProvider>(configure: o => o.RootPath = AppContext.BaseDirectory);
                if (opt.Logging.File.Info) builder.AddFile<InfoFileLoggerProvider>(configure: o => o.RootPath = AppContext.BaseDirectory);
                builder.SetMinimumLevel(LogLevel.Debug);
            });
            services.AddSingleton<ILogTextBlockService, LogTextBlockService>();
        }
    }
}

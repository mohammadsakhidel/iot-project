using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TrackAdmin.Extensions;
using TrackAdmin.Views;

namespace TrackAdmin
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e) {

            #region Configure Services:
            IServiceCollection services = new ServiceCollection();
            services.AddConfiguration();
            services.AddWindows();
            
            ServiceProvider = services.BuildServiceProvider();
            #endregion

            // Start Application:
            ServiceProvider.GetRequiredService<MainWindow>()
                .Show();

        }
    }
}

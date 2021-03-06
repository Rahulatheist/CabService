﻿using MyCabBooker.Services;
using MyCabBooker.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCabBooker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MapAPIService.Initialize(Constants.GoogleMapsApiKey);
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

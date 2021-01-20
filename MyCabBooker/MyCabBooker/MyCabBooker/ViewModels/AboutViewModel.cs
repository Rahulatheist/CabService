using MyCabBooker.Models;
using MyCabBooker.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MyCabBooker.ViewModels
{
    public enum PageStatusEnum
    {
        Default, Searching, ShowingRoute
    }
    public class AboutViewModel : BaseViewModel
    {
        IMapAPIService mapAPIService = new MapAPIService();

        
        public PageStatusEnum PageStatusEnum { get; set; }

        public ICommand DrawRouteCommand { get; set; }
        public ICommand GetPlacesCommand { get; set; }
        public ICommand GetUserLocationCommand { get; set; }
        public ICommand ChangePageStatusCommand { get; set; }
        public ICommand GetLocationNameCommand { get; set; }
        public ICommand CenterMapCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; set; }
        public ICommand LoadRouteCommand { get; set; }
        public ICommand CleanPolylineCommand { get; set; }
        public ICommand ChooseLocationCommand { get; set; }

        public ObservableCollection<AutoCompletePlaceForGoogleMap> Places { get; set; }
        public ObservableCollection<AutoCompletePlaceForGoogleMap> RecentPlaces { get; set; }
        public AutoCompletePlaceForGoogleMap RecentPlace1 { get; set; }
        public AutoCompletePlaceForGoogleMap RecentPlace2 { get; set; }
        public ObservableCollection<PriceOption> PriceOptions { get; set; }
        public PriceOption PriceOptionSelected { get; set; }

        public string PickupLocation { get; set; }

        Location OriginCoordinates { get; set; }
        Location DestinationCoordinates { get; set; }

        string _destinationLocation;
        public string DestinationLocation
        {
            get
            {
                return _destinationLocation;
            }
            set
            {
                _destinationLocation = value;
                if (!string.IsNullOrEmpty(_destinationLocation))
                {
                    GetPlacesCommand.Execute(_destinationLocation);
                }
            }
        }

        AutoCompletePlaceForGoogleMap _placeSelected;
        public AutoCompletePlaceForGoogleMap PlaceSelected
        {
            get
            {
                return _placeSelected;
            }
            set
            {
                _placeSelected = value;
                if (_placeSelected != null)
                    GetPlaceDetailCommand.Execute(_placeSelected);
            }
        }
        public AboutViewModel()
        {
            Title = "Search your Location";
            LoadRouteCommand = new Command(async () => await LoadRoute());
            GetPlaceDetailCommand = new Command<AutoCompletePlaceForGoogleMap>(async (param) => await GetPlacesDetail(param));
            GetPlacesCommand = new Command<string>(async (param) => await GetPlacesByName(param));
            GetUserLocationCommand = new Command(async () => await GetActualUserLocation());
            GetLocationNameCommand = new Command<Position>(async (param) => await GetLocationName(param));
            ChangePageStatusCommand = new Command<PageStatusEnum>((param) =>
            {
                PageStatusEnum = param;

                if (PageStatusEnum == PageStatusEnum.Default)
                {
                    CleanPolylineCommand.Execute(null);
                    GetUserLocationCommand.Execute(null);
                    DestinationLocation = string.Empty;
                }
                else if (PageStatusEnum == PageStatusEnum.Searching)
                {
                    Places = new ObservableCollection<AutoCompletePlaceForGoogleMap>(RecentPlaces);
                }
            });

            ChooseLocationCommand = new Command<Position>((param) =>
            {
                if (PageStatusEnum == PageStatusEnum.Searching)
                {
                    GetLocationNameCommand.Execute(param);
                }
            });

            GetUserLocationCommand.Execute(null);
        }


        async Task GetActualUserLocation()
        {
            try
            {
                await Task.Yield();
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(5000));
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    OriginCoordinates = location;
                    CenterMapCommand.Execute(location);
                    GetLocationNameCommand.Execute(new Position(location.Latitude, location.Longitude));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to get actual location", "Ok");
            }
        }

        //Get place 
        public async Task GetLocationName(Position position)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
                PickupLocation = placemarks?.FirstOrDefault()?.FeatureName;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public async Task GetPlacesByName(string placeText)
        {
            var places = await mapAPIService.GetNewPlaces(placeText);
            var placeResult = places.AutoCompletePlaces;
            if (placeResult != null && placeResult.Count > 0)
            {
                Places = new ObservableCollection<AutoCompletePlaceForGoogleMap>(placeResult);
            }
        }

        public async Task GetPlacesDetail(AutoCompletePlaceForGoogleMap placeA)
        {
            var place = await mapAPIService.GetMyPlaceDetails(placeA.PlaceId);
            if (place != null)
            {
                DestinationCoordinates = new Location(place.Latitude, place.Longitude);
                LoadRouteCommand.Execute(null);
                RecentPlaces.Add(placeA);
            }
        }
        
        public async Task LoadRoute()
        {
            if (OriginCoordinates == null)
                return;

            ChangePageStatusCommand.Execute(PageStatusEnum.ShowingRoute);

            var googleDirection = await mapAPIService.GetMyDirections($"{OriginCoordinates.Latitude}", $"{OriginCoordinates.Longitude}", $"{DestinationCoordinates.Latitude}", $"{DestinationCoordinates.Longitude}");
            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
            {


                var positions = (Enumerable.ToList(googleDirection.Routes.First().OverviewPolyline.Points));
                DrawRouteCommand.Execute(positions);
            }
            else
            {
                ChangePageStatusCommand.Execute(PageStatusEnum.Default);
                await Application.Current.MainPage.DisplayAlert(":(", "No route found", "Ok");

            }
        }
        public ICommand OpenWebCommand { get; }
    }

}
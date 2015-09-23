
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace GPSMaps
{
	[Activity (Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback//,Android.Gms.Maps.GoogleMap.IInfoWindowAdapter 
    {


		private GoogleMap map;
        Button btnmap, btnsat, btnhybrid, btnterrain;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.Map);

			//MapFragment mapFrag = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.map);
            btnmap = FindViewById<Button>(Resource.Id.buttonNormal);
            btnsat = FindViewById<Button>(Resource.Id.buttonSatellite);
            btnhybrid = FindViewById<Button>(Resource.Id.buttonHybrid);
            btnterrain = FindViewById<Button>(Resource.Id.buttonTerrain);

            btnmap.Click += btnmap_Click;
            btnhybrid.Click += btnhybrid_Click;
            btnsat.Click += btnsat_Click;
            btnterrain.Click += btnterrain_Click;
            setupmap();
			
			}

        void btnterrain_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeTerrain;
        }

        void btnsat_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeSatellite;
        }

        void btnhybrid_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeHybrid;
        }

        void btnmap_Click(object sender, EventArgs e)
        {
            map.MapType = GoogleMap.MapTypeNormal; 
        }


        private void setupmap()
        {
            if (map == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
           
            }
        

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            adddatatomap();
        }

        public void adddatatomap()
        { 
         
            LatLng loc0=new LatLng(-37.773250,175.250250);
            MarkerOptions opt0 = new MarkerOptions() //no semicolon
            .SetPosition(loc0)
            .SetRotation(45)
            
            .SetAlpha(.5f)
            .Draggable(true)
            .SetTitle("In the waikato")
            .SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.Icon))
            .SetSnippet("The presence of this actually creates an infowindow"); // which is why the other marker does not have one
            map.AddMarker(opt0);

            MarkerOptions opt1 = new MarkerOptions();
                double lat = Convert.ToDouble(Intent.GetStringExtra("Latitude"));
                double lng = Convert.ToDouble(Intent.GetStringExtra("Longitude"));
                string address = Intent.GetStringExtra("Address");

                LatLng location = new LatLng(lat, lng);

                opt1.SetPosition(location);
                opt1.SetTitle(address);

                map.AddMarker(opt1);

                // Positioning the camera to show the marker 

                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(15);
                builder.Bearing(90);
                builder.Tilt(65);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                map.MoveCamera(cameraUpdate);
            //marker window clicked event
           map.InfoWindowClick += map_InfoWindowClick;
            //marker dragged event
                map.MarkerDragEnd += map_MarkerDragEnd;

               // map.SetInfoWindowAdapter(this);
             
            //  map.SetOnInfoWindowClickListener(this); //- implements the interface IOnInfoWindowclickListener
        }

        void map_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            Toast.MakeText(this,"ended at "+ e.Marker.Position.Latitude.ToString() + e.Marker.Position.Longitude.ToString(), ToastLength.Long).Show();
        }
      
        //""STANDARD"" INFO WINDOW CLICK EVENT
        void map_InfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
      {
           Toast.MakeText(this, e.Marker.Position.Latitude.ToString() + e.Marker.Position.Longitude.ToString(), ToastLength.Long).Show();
        }

        public static void dothis() 
        {
            Console.WriteLine("Ive donew it");
        }


        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker) // there is no component click events here
        {
            View view = LayoutInflater.Inflate(Resource.Layout.Info_window,null,false);
            view.FindViewById<TextView>(Resource.Id.textViewName).Text = "Testfield";
            view.FindViewById<TextView>(Resource.Id.textViewAddress).Text = "Testfield rd auckland";
            view.FindViewById<TextView>(Resource.Id.textViewHours).Text = "9am-12pm";
            return view;
            //next implement a oninfowindowclick 
        }

//void GoogleMap.IOnInfoWindowClickListener.OnInfoWindowClick(Marker marker)
//{
//    throw new NotImplementedException();
//}
    }

}


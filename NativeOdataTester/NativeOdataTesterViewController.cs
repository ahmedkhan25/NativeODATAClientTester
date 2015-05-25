 using System;
 using System.Drawing;
using Simple.OData.Client;
using Simple.OData.Client.V3 ;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using ModernHttpClient;
using System.Net.Http;
using  System.Net;

using Foundation;
using UIKit;

/// <summary>
/// Native ODATA test view controller.
/// The Purpose of this single view ios app is to test the performance of using the simple ODATA 
/// client with a regular HTTP Client vs the ModernHTTPClient
/// see for more info: https://github.com/object/Simple.OData.Client/issues/113#issuecomment-105170145
/// and see modern native client here (using Paul Bett's ModernHttpClient)
//  https://components.xamarin.com/view/modernhttpclient
/// 
/// </summary>

namespace NativeOdataTester
{
	public partial class NativeOdataTesterViewController : UIViewController
	{

		public List<string> PackageNames { get; set; }
		public IEnumerable <Package> PackageList { get; set; }
		int count;
		TimeSpan diff { get; set; }
		double LastClientTime  { get; set; }
  
		public const string ServiceURL = "http://packages.nuget.org/v1/FeedService.svc/";

		//simple nuget package finder -looks for nuget packages 
		//see for example https://vagifabilov.wordpress.com/category/simple-odata-client/

		public NativeOdataTesterViewController (IntPtr handle) : base (handle)
		{
			
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.

			CountSlider.MinValue = 1;
			CountSlider.MaxValue = 10;
			count = 100;
			LastClientTime = -1;

			CountSlider.ValueChanged += (object sender, EventArgs e) => {

				LastClientTime = -1;

				if (CountSlider.Value < 4) count = 100;
				else if (CountSlider.Value < 6) count = 300;
				else if (CountSlider.Value <= 10) count = 800;



				DataLabel.Text = "Retrieved NuGetPackage Count: " + count.ToString();
				DataLabel.SetNeedsDisplay();

			};

			ODataButton.TouchUpInside += (object sender, EventArgs e) => {
				if (PackageNames != null) PackageNames.Clear();
				TypeLabel.Text = "Type: STANDARD";
				loadODATAUsingStandardClient();
			};

			NativeClientButton.TouchUpInside += (object sender, EventArgs e) => {
				if (PackageNames != null)PackageNames.Clear();
				TypeLabel.Text = "Type: MODERN HTTP";
				loadODATAUsingNativeClient();
			};
		}

		/// <summary>
		/// using regular HTTP CLIENT HERE
		/// </summary>
		//see http://www.odata.org/documentation/odata-version-2-0/uri-conventions/
		private async void loadODATAUsingStandardClient()
		{
 
			PackageNames = new List<string> ();
			DateTime start = DateTime.Now;
			 diff = new TimeSpan ();

		 

		// Create an ODATA Client with the normal httpclient (not using native client handler in this method)
			ODataClient client = new ODataClient(ServiceURL);

			var annotations = new ODataFeedAnnotations ();
			var a = await client
				.For<Package> ()
				//.Filter (x => x.Authors == "microsoft")
				.Top (count)
				//.OrderByDescending (x => x.DownloadCount)
				.FindEntriesAsync (annotations);

			foreach (Package package in a)
			{
				//Console.WriteLine(package["Title"]);

				PackageNames.Add(package.Title + " v: " + package.Version);
			}

			while (annotations.NextPageLink != null)
			{
				var b =	  await client
					.For<Package>()
					//.Filter (x => x.Authors == "microsoft")
					//.OrderByDescending (x => x.DownloadCount)
					.FindEntriesAsync(annotations.NextPageLink, annotations);

				foreach (Package package in b)
				{
					//Console.WriteLine(package["Title"]);

					PackageNames.Add(package.Title + " v: " + package.Version);
				}


			}


					diff = DateTime.Now- start;
				 
					UIAlertView alert =  new UIAlertView("Alert", "Got " + PackageNames.Count.ToString() + " Entries in "
				+ diff.TotalMilliseconds +" MilliSeconds",null,"Ok",null);

			alert.Show ();

			LastClientTime = diff.TotalMilliseconds;

			populateDataTable ();


						
		}

		/// USING MODERN HTTP CLIENT HERE
		/// should be faster since its using a ios networking based native library
		/// 
		private async void loadODATAUsingNativeClient()
		{

			PackageNames = new List<string> ();
			DateTime start = DateTime.Now;
			 diff = new TimeSpan ();

			//create native client here (using Paul Bett's ModernHttpClient)
			//https://components.xamarin.com/view/modernhttpclient

			var NativeClient = new System.Net.Http.HttpClient (new NativeMessageHandler());
		 
			ODataClientSettings settings = new ODataClientSettings ();

			NativeMessageHandler handler = new NativeMessageHandler ();

			settings.OnCreateMessageHandler = () => handler;
 

			settings.BaseUri = new Uri (ServiceURL);


			ODataClient client = new ODataClient(settings);

			var annotations = new ODataFeedAnnotations ();
			var a = await client
				.For<Package> ()
				//.Filter (x => x.Authors == "microsoft")
				 .Top (count)
				//.OrderByDescending (x => x.DownloadCount)
				.FindEntriesAsync (annotations);

			foreach (Package package in a)
			{
				//Console.WriteLine(package["Title"]);

				PackageNames.Add(package.Title + " v: " + package.Version);
			}

			while (annotations.NextPageLink != null)
			{
				var b =	  await client
					.For<Package>()
					//.Filter (x => x.Authors == "microsoft")
					//.OrderByDescending (x => x.DownloadCount)
					.FindEntriesAsync(annotations.NextPageLink, annotations);

				foreach (Package package in b)
				{
					//Console.WriteLine(package["Title"]);

					PackageNames.Add(package.Title + " v: " + package.Version);
				}

				 
			}
	 


			diff = DateTime.Now- start;

			UIAlertView alert =  new UIAlertView("Alert", "Got " + PackageNames.Count.ToString() + " Entries in "
				+ diff.TotalMilliseconds +" MilliSeconds",null,"Ok",null);

			alert.Show ();

			populateDataTable ();



		}

		public void populateDataTable()
		{
			 
			DataTable.Source = new TableSource(PackageNames.ToArray());
			DataTable.ReloadData ();

			DataLabel.Text = "Nuget Package Count: " + PackageNames.Count ();

			TimeLabel.Text = "Time Elapsed (Miliseconds): " + diff.TotalMilliseconds;

			TimeLabel.SetNeedsDisplay ();

			//compare times between default and modernhttpclient:
			if (LastClientTime != -1) {
				double time = LastClientTime - diff.TotalMilliseconds;
				DiffLabel.Text = time.ToString ();
				DiffLabel.SetNeedsLayout ();
			} else
				DiffLabel.Text = "N/A";
		    	DiffLabel.SetNeedsLayout ();
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}

	//Data Table source

	public class TableSource : UITableViewSource {
		string[] tableItems;
		string cellIdentifier = "TableCell";
		public TableSource (string[] items)
		{
			tableItems = items;
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return tableItems.Length;
		}
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			cell.TextLabel.Text = tableItems[indexPath.Row];
			return cell;
		}
	}

	//http://stackoverflow.com/questions/15251159/whats-the-best-way-to-add-one-item-to-an-ienumerablet
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Append<T>(
			this IEnumerable<T> source, params T[] tail)
		{
			return source.Concat(tail);
		}
	}
}


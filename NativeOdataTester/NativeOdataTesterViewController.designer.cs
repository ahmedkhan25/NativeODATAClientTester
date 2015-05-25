// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NativeOdataTester
{
	[Register ("NativeOdataTesterViewController")]
	partial class NativeOdataTesterViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider CountSlider { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DataLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView DataTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel DiffLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton NativeClientButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ODataButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TimeLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TitleLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TypeLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CountSlider != null) {
				CountSlider.Dispose ();
				CountSlider = null;
			}
			if (DataLabel != null) {
				DataLabel.Dispose ();
				DataLabel = null;
			}
			if (DataTable != null) {
				DataTable.Dispose ();
				DataTable = null;
			}
			if (DiffLabel != null) {
				DiffLabel.Dispose ();
				DiffLabel = null;
			}
			if (NativeClientButton != null) {
				NativeClientButton.Dispose ();
				NativeClientButton = null;
			}
			if (ODataButton != null) {
				ODataButton.Dispose ();
				ODataButton = null;
			}
			if (TimeLabel != null) {
				TimeLabel.Dispose ();
				TimeLabel = null;
			}
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
			if (TypeLabel != null) {
				TypeLabel.Dispose ();
				TypeLabel = null;
			}
		}
	}
}

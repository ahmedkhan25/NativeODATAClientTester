# NativeODATAClientTester

  Native ODATA test view controller.
  The Purpose of this single view ios app is to test the performance of using the simple ODATA 
 client with a regular HTTP Client vs the ModernHTTPClient
 see for more info: https://github.com/object/Simple.OData.Client/issues/113#issuecomment-105170145
 and see modern native client here (using Paul Bett's ModernHttpClient)
 https://components.xamarin.com/view/modernhttpclient
 
 In general the results appear to be faster using the ModernHTTPClient code as seen here:
 (for 800 entries a gain of 4 Seconds)
 
 ![screencap](http://i.imgur.com/C2a71S7.png)

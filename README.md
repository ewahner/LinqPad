### LinqPad Scripts

LinqPad scripts used for more than querying a database.

#####DbCompareWithWinMerge
- Requires the installation of [WinMerge](http://winmerge.org/)

######Also Requires the MS-SQL SDK with the following DLLs
* Microsoft.SqlServer.ConnectionInfo.dll
* Microsoft.SqlServer.Management.Sdk.Sfc.dll
* Microsoft.SqlServer.Smo.dll

Typically found in Program Files\Microsoft SQL Server\110\SDK\Assemblies

Add the following Namespaces:

    Microsoft.SqlServer.Management.Common
    Microsoft.SqlServer.Management.Smo
    System.Collections
    System.Collections.Specialized

In the credSource connection ctor add the FQDN to your Source and 
what you wish to compare to in the credTest.

#####TimeSheet
Ever have to create a timesheet to provide the hours that you worked to your employer.
This script will scan the EventLog looking for Logon/Logoff events and calculate the
amount of time worked and then produce an Excel Spreadsheet, where it drops the information.

If you don't have a c:\DL directory it will create one for you.  If you don't like that location
change it.  

The premise of this application is for users that work remotely and have to VPN and RDP to a 
machine.  If you want this to work for non-RDP session you might have to look for different
event ids.

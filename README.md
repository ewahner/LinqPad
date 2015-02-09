### LinqPad Scripts

LinqPad scripts used for more than querying a database.

DbCompareWithWinMerge
- Requires the installation of [WinMerge](http://winmerge.org/)

######Also Requires the MS-SQL SDK with the following DLLs
Microsoft.SqlServer.ConnectionInfo.dll
Microsoft.SqlServer.Management.Sdk.Sfc.dll
Microsoft.SqlServer.Smo.dll

Typically found in Program Files\Microsoft SQL Server\110\SDK\Assemblies

Add the following Namespaces:
Microsoft.SqlServer.Management.Common
Microsoft.SqlServer.Management.Smo
System.Collections
System.Collections.Specialized

In the credSource connection ctor add the FQDN to your Source and 
what you wish to compare to in the credTest.

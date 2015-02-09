void Main()
{
	// 4624 An Account was successfully logged on
	// 4634 An account was logged off
	// 4778 Reconnected session
	// 4800 Workstation locked
	// 4801 Workstation unlocked
	// 4779 Disconnected session
	var logs = new List<TimeClock>();
	var log = QueryActiveLog();
	for (EventRecord ev = log.ReadEvent(); null != ev; ev = log.ReadEvent())
	{
		if ( ev.Id == 4778 || ev.Id == 4779)
		{
			logs.Add(new TimeClock
			{
				date = Convert.ToDateTime(ev.TimeCreated),
				logon = ev.Id == 4778,
				logoff = ev.Id == 4779
			});
		}
	}
	var hrs = new List<HoursWorked>();
	DateTime? begdate = null;
	foreach(var lg in logs)
	{
		if (lg.logon == true)
		{
			begdate = lg.date;
		}
		else
		{
			if (begdate != null)
			{
				hrs.Add(new HoursWorked() { begtime = Convert.ToDateTime(begdate), endtime = lg.date, TimeWorked = (TimeSpan)(lg.date - begdate) });
				begdate = null;
			}
		}
	}
	hrs.ForEach(x =>
	{
		x.Worked = string.Format("{0}hrs {1}mins", x.TimeWorked.Hours, x.TimeWorked.Minutes);
		x.day = x.begtime.DayOfWeek;
	});
	
	var lastMon = Convert.ToDateTime(DateTime.Now.AddDays(-9).ToString("yyyy-MM-dd"));

	hrs = hrs.Where (h => h.begtime >= lastMon).ToList();
	TimeSpan time = TimeSpan.FromTicks(0);
	hrs.ForEach(x => time += x.TimeWorked);
	var minutes = time.TotalHours - Math.Floor(time.TotalHours);
	Console.WriteLine("Total Time Worked for Week w/o Lunch: {0} hrs {1} minutes", Math.Floor(time.TotalHours), Convert.ToInt32(minutes * 60));
	Console.WriteLine("Total Time Worked for Week w/ Lunch : {0} hrs {1} minutes", Math.Floor(time.TotalHours - 5), Convert.ToInt32(minutes * 60));
	var strm = new MemoryStream();
		
	using (var excel = new ExcelPackage(strm))
	{
		var book = excel.Workbook;
		var ws = book.Worksheets.Add("Sheet1");
		
		int i = 2;
		
		ws.Cells[1, 1].Value = "Start Time";
		ws.Cells[1, 2].Value = "End Time";
		ws.Cells[1, 3].Value = "Time Worked";
		ws.Cells[1, 4].Value = "Day";
		ws.Cells[1, 5].Value = "Worked";
		
		ws.Column(1).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss AM/PM";
		ws.Column(1).Width = 24;
		ws.Column(2).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss AM/PM";
		ws.Column(2).Width = 24;
		ws.Column(3).Style.Numberformat.Format = "hh:mm:ss";
		ws.Column(3).Width = 14;
		ws.Column(4).Width = 12;
		ws.Column(5).Width = 12;
		
		hrs.ForEach(x => 
		{
			ws.Cells[i, 1].Value = x.begtime;
			ws.Cells[i, 2].Value = x.endtime;
			ws.Cells[i, 3].Value = x.TimeWorked;
			ws.Cells[i, 4].Value = x.day;
			ws.Cells[i, 5].Value = x.Worked;
			i++;
		});
		
		if (!Directory.Exists(@"C:\DL"))
			Directory.CreateDirectory(@"C:\DL");
			
		var path = @"c:\DL\HoursWorked.xlsx";
		File.Delete(path);
		excel.SaveAs(new FileInfo(path));
	}
	
	hrs.Dump();
}

public class HoursWorked 
{
	public DateTime begtime { get; set; }
	public DateTime endtime { get; set; }
	public TimeSpan TimeWorked { get; set; }
	public DayOfWeek day { get; set; }
	public string Worked { get; set; }
}

public class TimeClock
{
	public DateTime date { get; set; }
	public bool logon { get; set; }
	public bool logoff { get; set; }
}

public EventLogReader QueryActiveLog()
{
	string queryString =
		"<QueryList>" +
		"  <Query Id=\"0\" Path=\"Security\">" +
		"    <Select Path=\"Security\">" +
		"        *[System[(Level &lt;= 3)]]" +
		"    </Select>" +
		"  </Query>" +
		"</QueryList>"; 

	EventLogQuery eventsQuery = new EventLogQuery("Application", PathType.LogName, queryString);
	EventLogReader logReader = new EventLogReader(eventsQuery);
	return logReader;
}

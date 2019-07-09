using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TextTasks.Models;

using MySql.Data.MySqlClient;
using dto = TextTasks.Models;

using Microsoft.AspNetCore.Authorization;


namespace TextTasks.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private MySqlDatabase MySqlDatabase { get; set; }
		public HomeController(MySqlDatabase mySqlDatabase)
		{
			this.MySqlDatabase = mySqlDatabase;
		}

		public async Task<IActionResult> Index()
		{
			return View(await this.GetTasks());
		}
		private async Task<List<dto.Task>> GetTasks()
		{
			var ret = new List<dto.Task>();

			var cmd = this.MySqlDatabase.Connection.CreateCommand() as MySqlCommand;
			cmd.CommandText = @"SELECT TaskId, Text, Completed FROM Tasks WHERE Archived IS NULL";

			using (var reader = await cmd.ExecuteReaderAsync())
				while (await reader.ReadAsync())
				{
					var t = new dto.Task()
					{
						TaskId = reader.GetFieldValue<int>(0),
						Text = reader.GetFieldValue<string>(1)
					};
					if (!reader.IsDBNull(2))
						t.Completed = reader.GetFieldValue<DateTime>(2);

					ret.Add(t);
				}
			return ret;
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

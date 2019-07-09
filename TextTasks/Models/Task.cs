using System;

namespace TextTasks.Models
{
	public class Task
	{
		public int TaskId { get; set; }
		public string Text { get; set; }
		public DateTime? Completed { get; set; }
	}
}

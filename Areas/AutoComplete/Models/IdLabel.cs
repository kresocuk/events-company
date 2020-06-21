using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestWebApp.Areas.AutoComplete.Models
{
	/// <summary>
	/// Nesto za autocomplete
	/// </summary>
	public class IdLabel
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("label")]
		public string Label { get; set; }

		public IdLabel() { }

		public IdLabel(int id, string label)
		{
			Id = id;
			Label = label;
		}

	}
}

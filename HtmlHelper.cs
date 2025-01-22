using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HtmlSerializer2
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper _helper = new HtmlHelper();
        public static HtmlHelper helper => _helper;

        public List<string> HtmlTags { get; set; }
        public List<string> HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            var context = File.ReadAllText("JSON Files/HtmlTags.json");
            HtmlTags=JsonSerializer.Deserialize<List<string>>(context);
            context = File.ReadAllText("JSON Files/HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<List<string>>(context);
        }

    }
}

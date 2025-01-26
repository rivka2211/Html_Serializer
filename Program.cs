using System.Text.RegularExpressions;

namespace HtmlSerializer2
{
    internal class Program
    {
        static void Print(HtmlElement root)
        {
            Console.WriteLine(root);
            foreach (var item in root.Children)
            {
                Print(item);
            }
        }
        static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
       
        public static async Task Main(string[] args)
        {
            string url = "https://docs.google.com/forms/d/e/1FAIpQLSd6hahe0kxVLFsqo8JWLfRhUdFo55ZAPODcFg3RI2YdnQlJbw/formResponse";
            string html = await Load(url);
            string cleanHtml = new Regex("\\n").Replace(html, "");
            string[] htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 1).ToArray();
            string[] tags = htmlLines;

            Console.WriteLine("before init");
            //init 
            HtmlElement root = new HtmlElement(tags[0]);
            HtmlElement current = root;
            for (int i = 1; i < tags.Length && tags[i] != "/html"; i++)
            {
                int index = tags[i].IndexOf(' ');
                string name = index == -1 ? tags[i] : tags[i].Substring(0, index);
                if (tags[i][0] == '/')
                {
                    current = current.Parent;
                }
                else if (HtmlHelper.helper.HtmlTags.Contains(name))
                {
                    HtmlElement tmp = new HtmlElement(tags[i]);
                    tmp.Parent = current;
                    current.Children.Add(tmp);
                    current = tmp;
                }
                else if (HtmlHelper.helper.HtmlVoidTags.Any(s => s.Contains(name)))
                {
                    HtmlElement tmp = new HtmlElement(tags[i]);
                    tmp.Parent = current;
                    current.Children.Add(tmp);
                }
                else
                {
                    current.InnerHtml = tags[i];
                }
            }
            Console.WriteLine("after init");

            Print(root);
            Console.WriteLine("dom finish!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Selector s1 = new Selector("meta");
            var res = root.GetBySelector(s1);
            Console.WriteLine($"there is {res.Count} tags for this selector" );
            res.ToList().ForEach(e => Console.WriteLine(e));
            Console.WriteLine("finish");
            Console.ReadLine();
        }

    }
}

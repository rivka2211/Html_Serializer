using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer2
{
    internal class CreateHtml
    {
        private static string[] tags;

        private static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://localhost:5173/");
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        private static async Task InitializeAsync(string url)
        {
            string html = await Load(url);
            string cleanHtml = new Regex("\\n").Replace(html, "");
            string[] htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 1).ToArray();
            tags = htmlLines;
        }

        //public static string GetRoot(string url)
        //{
        //    InitializeAsync(url);
        //    return allLines[0];
        //}

        public static async Task<HtmlElement> Init(string url)
        {
           await InitializeAsync(url);
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
            return root;
        }

        /*public static void Init(HtmlElement parent, int current)
        {
            if (new Regex("\\s").Replace(allLines[current], "").Length > 0)
            {
                if (allLines[current].Contains('/'))
                {
                    //allHtml.Add(parent);
                    return;
                }
                HtmlElement html1 = new HtmlElement(allLines[current]);
                string tagName = allLines[current].Split(' ')[0];
                if (HtmlHelper.helper.HtmlTags.Contains(tagName))
                {
                    parent.AddChild(html1);
                    Init(html1, current + 1);
                }
                else
                {
                    if (HtmlHelper.helper.HtmlVoidTags.Contains(tagName))
                        parent.AddChild(html1);
                    else
                        parent.InnerHtml = allLines[current];
                }
                Init(parent, current + 1);
            }
            else
              Init(parent,current++);
        }*/
        public static async Task<string> GetRootAsync(string url)
        {
            await InitializeAsync(url);
            return tags[0];
        }
     
    }
}

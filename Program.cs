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

        public static async Task Main(string[] args)
        {
            string url = "http://localhost:5173/";
            CreateHtml html = new CreateHtml();
            //HtmlElement root = new HtmlElement(await CreateHtml.GetRootAsync(url));
            //root.Name = "html";
            Console.WriteLine("before init");
            HtmlElement dom = await CreateHtml.Init(url);
            Console.WriteLine("after init");
            Print(dom);
            Console.WriteLine("dom finish!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Selector s1 = new Selector("div#root");
            var res = dom.GetBySelector(s1);
            Console.WriteLine("there is {0} tags for this selector",res.Count);
            res.ToList().ForEach(e => Console.WriteLine(e));
            Console.WriteLine("finish");
            Console.ReadLine();
        }

    }
}

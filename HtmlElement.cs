using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string myElement)
        {
            Id = "";
            Name = myElement.Split(' ').First();
            //var myElement = "<div id=\"my id\" class=\"my class1 my class2\" width=\"100%\"></div>";
            var myAttribute = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(myElement);
            var classes = "";
            Attributes = new List<string>();
            foreach (Match match in myAttribute)
            {
                if (match.Groups[1].Value == "id")
                    Id = match.Groups[1].Value;
                else if (match.Groups[1].Value == "class")
                    classes = match.Groups[2].Value;
                else
                    Attributes.Add(match.Groups[2].Value);

                //Console.WriteLine($"Attribute: {match.Groups[1].Value}, Value: {match.Groups[2].Value}");
            }
            Classes = classes.Split(" ").ToList();
            this.Children = new List<HtmlElement>();

        }
        public void AddChild(HtmlElement child)
        {
            Children.Add(child);
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> elements = new Queue<HtmlElement>();
            elements.Enqueue(this);
            HtmlElement current;
            while (elements.Count > 0)
            {
                current = elements.Dequeue();
                yield return current;
                foreach (var child in current.Children)
                {
                    elements.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement currentFather = this.Parent;
            while (currentFather != null)
            {
                yield return currentFather;
                currentFather = currentFather.Parent;
            }
        }

        public HashSet<HtmlElement> GetBySelector(Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            GetBySelector(selector, this, result);
            return result;
        }
        private void GetBySelector(Selector selector, HtmlElement element, HashSet<HtmlElement> result)
        {
            var good = new List<HtmlElement>(element.Descendants().ToList().FindAll(e => selector.Fit(e)));

            if (selector.Child == null)
            {
                good.ForEach(g => result.Add(g));
                return;
            }
            foreach (var item in good)
            {
                GetBySelector(selector.Child, item,result);
            }
        }

        public override string? ToString()
        {
            return $"id:{Id},name:{Name},inner HTML:{InnerHtml},classes:{Classes.Count},parent:{Parent?.Name}\n";
        }
    }
}


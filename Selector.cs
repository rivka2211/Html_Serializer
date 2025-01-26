using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer2
{
    internal class Selector
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector(string query)
        {
            Classes = new List<string>();

            if (string.IsNullOrEmpty(query))
            {
                return;
            }
            string[] parts = query.Split(new[] { '#', '.' });
            int idIndex = query.IndexOf('#');
            if (idIndex != -1)
            {
                Id = parts[1];
            }
            int classIndex = query.IndexOf('.');
            if (classIndex != -1)
            {
                for (int i = (idIndex != -1 ? 2 : 1); i < parts.Length; i++)
                {
                    Classes.Add(parts[i]);
                }
            }

            int firstSpecialCharIndex = query.IndexOfAny(new[] { '#', '.' });
            if (firstSpecialCharIndex == -1)
                Name = query;
            else
                Name = query.Substring(0, firstSpecialCharIndex);
        }

      /*  public static Selector GetSelector(string query)
        {
            string[] parts = query.Split(' ');
            Selector rootSelector = null;
            Selector currentSelector = null;

            for (int i = 0; i < parts.Length; i++)
            {
                Selector newSelector = new Selector(parts[i]);
                if (rootSelector == null)
                {
                    rootSelector = newSelector;
                }
                else if (currentSelector != null)
                {
                    currentSelector.Child = newSelector;
                    newSelector.Parent = currentSelector;
                }
                currentSelector = newSelector;
            }
            return rootSelector;
        }*/

        public bool Fit(HtmlElement element)
        {
            if (element == null)
                return false;

            if (this.Id != null && this.Id != element.Id)
                return false;

            if (this.Name != null && this.Name != element.Name)
                return false;

            if (this.Classes.Count > 0 && !this.Classes.All(c => element.Classes.Contains(c)))
                return false;

            return true;
        }
    }
}



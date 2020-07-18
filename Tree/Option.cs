using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
    public class Option
    {            
        public string ShortName { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class OptionsParser
    {

        private List<Option> _options = new List<Option>();

        public void AddExpectedOptionName(string name, string shortName)
        {
            _options.Add(new Option()
            {
                Name = name,
                ShortName = shortName,
                Value = null
            });
        }

        public Option GetOption (string shortName, string name = "")
        {
            return null;
        }

        public void Parse(string[] args)
        {
            foreach (Option option in _options)
            {
                string name = "--" + option.Name;
                string shortName = "-" + option.ShortName;
                string arg = args.FirstOrDefault(o => o.IndexOf(name) > 0 || o.IndexOf(shortName) > 0);
            }
        }
    }
}

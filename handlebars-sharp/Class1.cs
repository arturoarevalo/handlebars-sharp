namespace Handlebars
{
    using System.Collections.Specialized;
    using System.Dynamic;
    using System;
    using Handlebars.Core;

    public class Class1
    {
        class H
        {
            public string Name { get; set; }
        }

        public static void Main (string[] args)
        {
            var data = "{{<hotel}}Hotel {{Name}} in {{../Name}}\r\n{{/hotel}}{{>hoteltemplate lista=hotels}}";
            var data2 = "a{{#with hotels.3}}{{Name}}{{/with}}b";

            var compiler = new Compiler ();

            var template = compiler.Compile (data);
            var template2 = compiler.Compile ("{{<hotel2}}New superhotel ({{@index}} - {{index}}) {{Name}} in {{../Name2}}\r\n{{/hotel2}}{{#each lista}}{{>hotel2}}{{/each}}");

            var test = new
                       {
                           Name2 = "Paris",
                           BadName = "<Arturo & Arévalo>",
                           hotels2 = new string[] {},
                           hotels = new[]
                                    {
                                        new  { index = "aaa", Name = "Hotel1" },
                                        new  { index = "bbb", Name = "Hotel2" },
                                        new  { index = "ccc", Name = "Hotel3" },
                                        new  { index = "ddd", Name = "Hotel4" }
                                    }
                       };

            var result = template.Render (test, s =>
                                                {
                                                    if (s == "hoteltemplate") return template2;
                                                    return null;
                                                });

            Console.WriteLine(result);

            Console.ReadKey ();
        }
    }

}

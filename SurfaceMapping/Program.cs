using System;
using System.Diagnostics;
using System.Linq;

namespace SurfaceMapping
{
    class MainClass
    {
        private class Dto { public int Id { get; set; } public string Name { get; set; } public DateTime Date { get; set; }};

        public static void Main(string[] args)
        {
            var list = new Dto[20];
            for(var i = 0; i< 20; i++)
            {
                list[i] = new Dto
                {
                    Id = i + 1,
                    Name = $"Name {i + 1 }",
                    Date = DateTime.Now
                };
            }



            var sw = new Stopwatch();
            sw.Start();
            var source = list.Select(x => new { x.Id, x.Name, x.Date }).ToArray();
            var destination = new Dto[source.Length];
            var properties = source
                .GetType()
                .GetElementType()
                .GetProperties()
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)).ToArray();

            for(var i = 0; i < source.Length; i++)
            {
                var dto = Activator.CreateInstance<Dto>();

                foreach(var property in properties)
                {
                    var destinationProperty = typeof(Dto).GetProperty(property.Name);

                    if(destinationProperty != null)
                    {
                        var value = property.GetValue(source.ElementAt(i));
                        destinationProperty.SetValue(dto, value);
                    }
                }

                destination[i] = dto;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}

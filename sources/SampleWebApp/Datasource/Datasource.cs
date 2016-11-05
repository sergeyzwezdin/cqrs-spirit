using System.Collections.Generic;

namespace SampleWebApp.Datasource
{
    public class Datasource
    {
        public List<Item> Items = new List<Item>()
                                        {
                                            new Item { Id = 1, Value = "Value 1" },
                                            new Item { Id = 2, Value = "Value 2" },
                                            new Item { Id = 3, Value = "Value 3" },
                                            new Item { Id = 4, Value = "Value 4" },
                                            new Item { Id = 5, Value = "Value 5" },
                                            new Item { Id = 6, Value = "Value 6" },
                                        };
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Xunit.Abstractions;

namespace AdventOfCode2019.Tests
{
    public abstract class TestBase
    {
        protected ITestOutputHelper Output { get; }
        protected JsonSerializerSettings JsonSettings { get; set; }

        protected TestBase(ITestOutputHelper output)
        {
            Output = output;
            JsonSettings = new JsonSerializerSettings();
            JsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            JsonSettings.Converters.Add(new StringEnumConverter());
            JsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        protected void PrintObj(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, JsonSettings);
            Output.WriteLine(json);
        }
    }
}

using System.CommandLine;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var parameterOne = new Option<string?>(["-o", "--one"], () => null, "Parameter one");
parameterOne.AddValidator(r =>
{
    var value = r.GetValueOrDefault<string?>();
    if (value != null && value != "one")
    {
        r.ErrorMessage = "Parameter one must be 'one'";
    }
});

var parameterTwo = new Option<string?>(["-t", "--two"], () => null, "Parameter two");

var rootCommand = new RootCommand
{
    parameterOne,
    parameterTwo
};

rootCommand.SetHandler(RunAsync, parameterOne, parameterTwo);
await rootCommand.InvokeAsync(args);

async Task RunAsync(string? one, string? two)
{
    Console.WriteLine($"Parameter one: {one}");
    Console.WriteLine($"Parameter two: {two}");
    Console.WriteLine($"Host: {configuration["HOST"]}");

    await Task.Delay(1000);

    Console.WriteLine("Done");
}

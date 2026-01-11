using DeliveryCost;
using Kiki.Courier.Domain;
using Kiki.Courier.ServiceLayer.IContract;
using Microsoft.Extensions.DependencyInjection;
using System;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var collection = new ServiceCollection();
        RegisterDependency.Register(collection);
        IServiceProvider serviceProvider = collection.BuildServiceProvider();
        var packageService = serviceProvider.GetService<IPackageService>();
        int basicCost = 0, numberOfPackages = 0, vehicleCount = 0, maxSpeed = 0, maxWeight = 0;
        List<Package> packages = GetInputData(out basicCost, out vehicleCount, out maxSpeed, out maxWeight);
        await packageService.CalculatePackageDeliveryCostAsync(basicCost, packages);
        await packageService.CalculatePackageDeliveryTimeAsync(packages, vehicleCount, maxSpeed, maxWeight);
        
        ShowOutput(packages);
        if (serviceProvider is IDisposable)
        {
            ((IDisposable)serviceProvider).Dispose();
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void ShowOutput(List<Package> packages)
    {
        foreach (var package in packages)
        {
            Console.WriteLine($"{package.Id} {package.Discount} {package.TotalCost} {package.EstimatedDeliveryTime:F2}");
        }
    }

    private static List<Package> GetInputData(out int basicCost, out int vehicleCount, 
        out int maxSpeed, out int maxWeight)
    {
        Console.WriteLine("Welcome to Kiki Courier Service");
        Console.WriteLine("Please enter the basic delivery cost and number of packages:");
        var basicCostInput = Console.ReadLine();
        basicCost = int.Parse(basicCostInput.Split(' ')[0] ?? "0");
        int numberOfPackages = int.Parse(basicCostInput.Split(' ')[1] ?? "0");
        Console.WriteLine("Please enter the package details (PackageID WeightInKg DistanceInKm CouponCode) one per line:");
        List<Package> packages = new List<Package>();
        for (int i = 0; i < numberOfPackages; i++)
        {
            var packageInput = Console.ReadLine();
            // Process each package input as needed
            Package package = new Package();
            package.Id = packageInput.Split(' ')[0];
            package.WeightInKg = int.Parse(packageInput.Split(' ')[1] ?? "0");
            package.DistanceInKm = int.Parse(packageInput.Split(' ')[2] ?? "0");
            package.CouponCode = packageInput.Split(' ')[3];
            packages.Add(package);
        }

        Console.WriteLine("Please enter the number of vehicles max speed and max weight :");
        var vehicleInput = Console.ReadLine();
        vehicleCount = int.Parse(vehicleInput.Split(' ')[0] ?? "0");
        maxSpeed = int.Parse(vehicleInput.Split(' ')[1] ?? "0");
        maxWeight = int.Parse(vehicleInput.Split(' ')[2] ?? "0");
        return packages;
    }
}
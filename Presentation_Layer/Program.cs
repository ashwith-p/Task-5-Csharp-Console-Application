using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory;
using EmployeeDirectory.Interfaces;
using EmployeeDirectory.Providers;

class Employee_Project
{
    public static void Main()
    {
        var services=new ServiceCollection();
        services.AddSingleton<Domain.Interfaces.IEmployeeProvider,Domain.Providers.EmployeeProviders>();
        services.AddSingleton<Domain.Interfaces.IRoleProvider, Domain.Providers.RoleProviders>();
        services.AddSingleton<IEmployeeProvider, EmployeeProvider>();
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<Data.Interfaces.IDatabaseOperations, Data.DatabaseOperations>();
        services.AddSingleton<MainMenu>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        MainMenu menu = serviceProvider.GetRequiredService<MainMenu>();
        menu.Init();
        
    }
}

       


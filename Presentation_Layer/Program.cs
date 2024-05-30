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
        services.AddSingleton<Domain.Interfaces.IRoleProvider,Domain.Providers.RoleProviders>();
        services.AddSingleton<IEmployeeProvider, EmployeeProvider>();
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<Data.Interfaces.IRoleDetailProvider,Data.Provider.RoleDetailsProvider>();
        services.AddSingleton<Data.Interfaces.IRoleDataProvider, Data.Provider.RoleDataProvider>();
        services.AddSingleton<Data.Interfaces.IProjectProvider, Data.Provider.ProjectProvider>();
        services.AddSingleton<Data.Interfaces.ILocationProvider, Data.Provider.LocationProvider>();
        services.AddSingleton<Data.Interfaces.IDepartmentProvider, Data.Provider.DepartmentProvider>();
        services.AddSingleton<Data.Interfaces.IEmployeeDataProvider, Data.Provider.EmployeeDataProvider>();
        services.AddSingleton<Data.Interfaces.IDatabaseOperations, Data.DatabaseOperations>();
        services.AddDbContext<AshwithEmployeeDirectoryContext>();
        services.AddSingleton<MainMenu>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        MainMenu menu = serviceProvider.GetRequiredService<MainMenu>();
        menu.Init();
        
    }
}

       


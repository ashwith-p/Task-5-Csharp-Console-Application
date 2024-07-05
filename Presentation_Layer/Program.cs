using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory;
using EmployeeDirectory.Interfaces;
using EmployeeDirectory.Providers;

class Employee_Project
{
    public static void Main()
    {
        var services=new ServiceCollection();
        services.AddSingleton<Domain.Interfaces.IEmployeeProvider,Domain.Providers.EmployeeProvider>();
        services.AddSingleton<Domain.Interfaces.IRoleProvider,Domain.Providers.RoleProvider>();
        services.AddSingleton<IEmployeeProvider, EmployeeProvider>();
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<Data.Interfaces.IRoleDetailRepository,Data.Provider.RoleDetailsRepository>();
        services.AddSingleton<Data.Interfaces.IRoleRepository, Data.Provider.RoleRepository>();
        services.AddSingleton<Data.Interfaces.IProjectRepository, Data.Provider.ProjectRepository>();
        services.AddSingleton<Data.Interfaces.ILocationRepository, Data.Provider.LocationRepository>();
        services.AddSingleton<Data.Interfaces.IDepartmentRepository, Data.Provider.DepartmentRepository>();
        services.AddSingleton<Data.Interfaces.IEmployeeRepository, Data.Provider.EmployeeRepository>();
        services.AddSingleton<Data.Interfaces.IDatabaseOperations, Data.DatabaseOperations>();
        services.AddDbContext<AshwithEmployeeDirectoryContext>();
        services.AddSingleton<MainMenu>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        MainMenu menu = serviceProvider.GetRequiredService<MainMenu>();
        menu.Init();
        
    }
}

       


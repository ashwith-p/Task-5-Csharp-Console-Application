using Microsoft.Extensions.DependencyInjection;
using EmployeeDirectory;
using EmployeeDirectory.Interfaces;
using EmployeeDirectory.Providers;

class Employee_Project
{
    public static void Main()
    {
        var services=new ServiceCollection();
        services.AddSingleton<BLL.Interfaces.IEmployeeProvider,BLL.Providers.EmployeeProviders>();
        services.AddSingleton<BLL.Interfaces.IRoleProvider, BLL.Providers.RoleProviders>();
        services.AddSingleton<IEmployeeProvider, EmployeeProvider>();
        services.AddSingleton<IRoleProvider, RoleProvider>();
        services.AddSingleton<DLL.Interfaces.IDatabaseOperations, DLL.DatabaseOperations>();
        services.AddSingleton<MainMenu>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        MainMenu menu = serviceProvider.GetRequiredService<MainMenu>();
        menu.Init();
        
    }
}

       


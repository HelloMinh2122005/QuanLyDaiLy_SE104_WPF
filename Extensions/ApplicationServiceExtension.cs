using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Repositories;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.Views;

namespace QuanLyDaiLy.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Register database
        services.AddSingleton<DatabaseConfig>();

        // Register repositories and services
        services.AddScoped<IDaiLyService, DaiLyRepository>();
        services.AddScoped<ILoaiDaiLyService, LoaiDaiLyRepository>();
        services.AddScoped<IQuanService, QuanRepository>();
        services.AddScoped<IPhieuThuService, PhieuThuRepository>();
        services.AddScoped<IPhieuXuatService, PhieuXuatRepository>();
        services.AddScoped<IMatHangService, MatHangRepository>();
        services.AddScoped<IDonViTinhService, DonViTinhRepository>();
        services.AddScoped<IThamSoService, ThamSoRepository>();
        services.AddScoped<IChiTietPhieuXuatService, ChiTietPhieuXuatRepository>();

        // Register helpers
        services.AddSingleton<ComboBoxItemConverter>();

        // Register ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<HoSoDaiLyViewModel>();
        services.AddTransient<Func<int, ChinhSuaDaiLyViewModel>>(sp => dailyId =>
            new ChinhSuaDaiLyViewModel(
                sp.GetRequiredService<IDaiLyService>(),
                sp.GetRequiredService<IQuanService>(),
                sp.GetRequiredService<ILoaiDaiLyService>(),
                dailyId
            )
        );
        services.AddTransient<TraCuuDaiLyViewModel>();

        // Register Views
        services.AddTransient<MainWindow>();
        services.AddTransient<HoSoDaiLyWinDow>();
        services.AddTransient<ChinhSuaDaiLyWindow>();
        services.AddTransient<TraCuuDaiLyWindow>();
                    
        return services;
    }   
}
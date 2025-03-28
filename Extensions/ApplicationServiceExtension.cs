using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Repositories;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.ViewModels.QuanViewModels;
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

        // Register navigation service
        services.AddSingleton<INavigationService, NavigationService>();

        // Register Page Views
        services.AddTransient<Views.DashboardViews.DashboardPage>();
        services.AddTransient<Views.LoaiDaiLyViews.LoaiDaiLyPage>();
        services.AddTransient<Views.QuanViews.QuanPage>();
        services.AddTransient<Views.QuanViews.ThemQuanWindow>();
        services.AddTransient<Views.QuanViews.CapNhatQuanWindow>();

        services.AddTransient<Views.MatHangViews.MatHangPage>();
        services.AddTransient<Views.PhieuThuViews.PhieuThuPage>();
        services.AddTransient<Views.PhieuXuatViews.PhieuXuatPage>();
        services.AddTransient<Views.DonViTinhViews.DonViTinhPage>();
        services.AddTransient<Views.ThamSoViews.ThamSoPage>();

        // Register Page ViewModels
        services.AddTransient<ViewModels.DashboardViewModels.DashboardPageViewModel>();
        services.AddTransient<ViewModels.LoaiDaiLyViewModels.LoaiDaiLyPageViewModel>();
        services.AddTransient<QuanPageViewModel>();
        services.AddTransient<ThemQuanViewModel>();
        services.AddTransient<Func<int, ChinhSuaQuanViewModel>>(sp => quanId =>
            new ChinhSuaQuanViewModel(
                sp.GetRequiredService<IQuanService>(),
                quanId
            )
        );
        services.AddTransient<ViewModels.MatHangViewModels.MatHangPageViewModel>();
        services.AddTransient<ViewModels.PhieuThuViewModels.PhieuThuPageViewModel>();
        services.AddTransient<ViewModels.PhieuXuatViewModels.PhieuXuatPageViewModel>();
        services.AddTransient<ViewModels.DonViTinhViewModels.DonViTinhPageViewModel>();
        services.AddTransient<ViewModels.ThamSoViewModels.ThamSoPageViewModel>();

        return services;
    }   
}
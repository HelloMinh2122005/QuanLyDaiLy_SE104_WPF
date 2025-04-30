using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Configs;
using QuanLyDaiLy.Helpers;
using QuanLyDaiLy.Repositories;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels;
using QuanLyDaiLy.ViewModels.BaoCaoViewModels;
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
                dailyId,
                sp.GetRequiredService<IThamSoService>()
            )
        );
        services.AddSingleton<Func<string, int, BaoCaoDoanhSoViewModel>>(sp => (month, year) =>
        {
            var daiLyService = sp.GetRequiredService<IDaiLyService>();
            var phieuXuatService = sp.GetRequiredService<IPhieuXuatService>();

            var vm = new BaoCaoDoanhSoViewModel(month, year, daiLyService, phieuXuatService);
            return vm;
        });


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
        services.AddTransient<Views.LoaiDaiLyViews.ThemLoaiDaiLyWindow>();
        services.AddTransient<Views.LoaiDaiLyViews.CapNhatLoaiDaiLyWindow>();
        services.AddTransient<Views.LoaiDaiLyViews.TraCuuLoaiDaiLyWindow>();

        services.AddTransient<Views.QuanViews.QuanPage>();
        services.AddTransient<Views.QuanViews.ThemQuanWindow>();
        services.AddTransient<Views.QuanViews.CapNhatQuanWindow>();
        services.AddTransient<Views.QuanViews.TraCuuQuanWindow>();

        services.AddTransient<Views.MatHangViews.MatHangPage>();
        services.AddTransient<Views.MatHangViews.ThemMatHangWindow>();
        services.AddTransient<Views.MatHangViews.CapNhatMatHangWindow>();
        services.AddTransient<Views.MatHangViews.TraCuuMatHangWindow>();

        services.AddTransient<Views.PhieuThuViews.PhieuThuPage>();
        services.AddTransient<Views.PhieuThuViews.ThemPhieuThuWindow>();
        services.AddTransient<Views.PhieuThuViews.TraCuuPhieuThuTienWindow>();

        services.AddTransient<Views.PhieuXuatViews.PhieuXuatPage>();
        services.AddTransient<Views.PhieuXuatViews.ThemPhieuXuatWindow>();
        services.AddTransient<Views.PhieuXuatViews.CapNhatPhieuXuatWindow>();
        services.AddTransient<Views.PhieuXuatViews.TraCuuPhieuXuatWindow>();

        services.AddTransient<Views.DonViTinhViews.DonViTinhPage>();
        services.AddTransient<Views.DonViTinhViews.ThemDonViTinhWindow>();
        services.AddTransient<Views.DonViTinhViews.CapNhatDonViTinhWindow>();

        services.AddTransient<Views.ThamSoViews.ThamSoPage>();

        services.AddTransient<Views.BaoCaoViews.BaoCaoChiTietPage>();
        services.AddTransient<Views.BaoCaoViews.BaoCaoCongNoWindow>();
        services.AddTransient<Views.BaoCaoViews.BaoCaoDoanhSoWindow>();

        // Register Page ViewModels
        services.AddTransient<ViewModels.DashboardViewModels.DashboardPageViewModel>();

        services.AddTransient<ViewModels.LoaiDaiLyViewModels.LoaiDaiLyPageViewModel>();
        services.AddTransient<ViewModels.LoaiDaiLyViewModels.ThemLoaiDaiLyViewModel>();
        services.AddTransient<Func<int, ViewModels.LoaiDaiLyViewModels.CapNhatLoaiDaiLyViewModel>>(sp => loaiDaiLyId =>
            new ViewModels.LoaiDaiLyViewModels.CapNhatLoaiDaiLyViewModel(
                sp.GetRequiredService<ILoaiDaiLyService>(),
                loaiDaiLyId
            )
        );
        services.AddTransient<ViewModels.LoaiDaiLyViewModels.TraCuuLoaiDaiLyWindowViewModel>();

        services.AddTransient<ViewModels.QuanViewModels.QuanPageViewModel>();
        services.AddTransient<ViewModels.QuanViewModels.ThemQuanViewModel>();
        services.AddTransient<Func<int, ViewModels.QuanViewModels.ChinhSuaQuanViewModel>>(sp => quanId =>
            new ViewModels.QuanViewModels.ChinhSuaQuanViewModel(
                sp.GetRequiredService<IQuanService>(),
                quanId
            )
        );
        services.AddTransient<ViewModels.QuanViewModels.TraCuuQuanWindowViewModel>();

        services.AddTransient<ViewModels.MatHangViewModels.MatHangPageViewModel>();
        services.AddTransient<ViewModels.MatHangViewModels.ThemMatHangWindowViewModel>();
        services.AddTransient<Func<int, ViewModels.MatHangViewModels.CapNhatMatHangWindowViewModel>>(sp => matHangId =>
            new ViewModels.MatHangViewModels.CapNhatMatHangWindowViewModel(
                sp.GetRequiredService<IMatHangService>(),
                sp.GetRequiredService<IDonViTinhService>(),
                matHangId
            )
        );
        services.AddTransient<ViewModels.MatHangViewModels.TraCuuMatHangWindowViewModel>();

        services.AddTransient<ViewModels.PhieuThuViewModels.PhieuThuPageViewModel>();
        services.AddTransient<ViewModels.PhieuThuViewModels.ThemPhieuThuWindowViewModel>();
        services.AddTransient<ViewModels.PhieuThuViewModels.TraCuuPhieuThuWindowViewModel>();


        // PHIẾU XUẤT CỦA THÀNH
        services.AddTransient<ViewModels.PhieuXuatViewModels.PhieuXuatPageViewModel>();
        services.AddTransient<ViewModels.PhieuXuatViewModels.ThemPhieuXuatWindowViewModel>();
        services.AddTransient<Func<int, ViewModels.PhieuXuatViewModels.CapNhatPhieuXuatWindowViewModel>>(px => phieuXuatId =>
            new ViewModels.PhieuXuatViewModels.CapNhatPhieuXuatWindowViewModel(
                px.GetRequiredService<IPhieuXuatService>(),
                px.GetRequiredService<IChiTietPhieuXuatService>(),
                px.GetRequiredService<IDaiLyService>(),
                px.GetRequiredService<IMatHangService>(),
                px.GetRequiredService<ILoaiDaiLyService>(),
                phieuXuatId
            )
        );
        services.AddTransient<ViewModels.PhieuXuatViewModels.TraCuuPhieuXuatWindowViewModel>();

        services.AddTransient<ViewModels.DonViTinhViewModels.DonViTinhPageViewModel>();
        services.AddTransient<ViewModels.DonViTinhViewModels.ThemDonViTinhPageViewModel>();
        services.AddTransient<Func<int, ViewModels.DonViTinhViewModels.CapNhatDonViTinhPageViewModel>>(dvt => dvtID =>
            new ViewModels.DonViTinhViewModels.CapNhatDonViTinhPageViewModel(
                dvt.GetRequiredService<IDonViTinhService>(),
                dvtID
            )
        );

        services.AddTransient<ViewModels.ThamSoViewModels.ThamSoPageViewModel>();

        services.AddTransient<ViewModels.BaoCaoViewModels.BaoCaoChiTietViewModel>();
        services.AddTransient<ViewModels.BaoCaoViewModels.BaoCaoCongNoViewModel>();
        services.AddTransient<ViewModels.BaoCaoViewModels.BaoCaoDoanhSoViewModel>();

        return services;
    }
}
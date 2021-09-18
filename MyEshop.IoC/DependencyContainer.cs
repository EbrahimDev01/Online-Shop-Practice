using Microsoft.Extensions.DependencyInjection;
using MyEshop.Application.Interfaces;
using MyEshop.Application.Services;
using MyEshop.Data.Repositories;
using MyEshop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            RegisterServiceApplication(services);
            RegisterServiceData(services);
        }

        private static void RegisterServiceApplication(IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        private static void RegisterServiceData(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
        }
    }
}

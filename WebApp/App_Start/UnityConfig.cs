using System;
using System.Configuration;
using System.Data;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using DataLayer;
using DataLayer.Repository;
using DataLayer.Repository.Interfaces;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using WebApp.Controllers;

namespace WebApp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            container.RegisterType<IDbContext, SqlServer>(
                    new PerRequestLifetimeManager(),
                    new InjectionConstructor(ConfigurationManager.ConnectionStrings["default"].ConnectionString)
                );

            // Register Data Access Layer
            container.RegisterType<IAppUserRepo, AppUserRepo>(new PerRequestLifetimeManager());
            container.RegisterType<ITrainingRepo, TrainingRepo>(new PerRequestLifetimeManager());
            container.RegisterType<IDepartmentRepo, DepartmentRepo>(new PerRequestLifetimeManager());
            container.RegisterType<IPrerequisiteRepo, PrerequisiteRepo>(new PerRequestLifetimeManager());
            container.RegisterType<ITrainingContentRepo, TrainingContentRepo>(new PerRequestLifetimeManager());
            container.RegisterType<ITrainingContentAttachmentRepo, TrainingContentAttachmentRepo>(new PerRequestLifetimeManager());
            container.RegisterType<ITrainingPrerequisiteRepo, TrainingPrerequisiteRepo>(new PerRequestLifetimeManager());
            container.RegisterType<IEnrollmentPrerequisiteAttachmentRepo, EnrollmentPrerequisiteAttachmentRepo>(new PerRequestLifetimeManager());
            container.RegisterType<IUserTrainingEnrollmentRepo, UserTrainingEnrollmentRepo>(new PerRequestLifetimeManager());
            container.RegisterType<IUserManagerRepo, UserManagerRepo>(new PerRequestLifetimeManager());
            container.RegisterType<IUserRoleRepo, UserRoleRepo>(new PerRequestLifetimeManager());


            // Register Service Layer
            container.RegisterType<IUserService, UserService>(new PerRequestLifetimeManager());
            container.RegisterType<ITrainingService, TrainingService>(new PerRequestLifetimeManager());
            container.RegisterType<IDepartmentService, DepartmentService>(new PerRequestLifetimeManager());
            container.RegisterType<IPrerequisiteService, PrerequisiteService>(new PerRequestLifetimeManager());
            container.RegisterType<IUserTrainingEnrollmentService, UserTrainingEnrollmentService>(new PerRequestLifetimeManager());
            container.RegisterType<IUserRoleService, UserRoleService>(new PerRequestLifetimeManager());
            
            
            container.RegisterType<IStorageService, AmazonS3Service>(new PerRequestLifetimeManager());
            container.RegisterType<INotificationService, NtfyService>(new PerRequestLifetimeManager());

        }
    }
}
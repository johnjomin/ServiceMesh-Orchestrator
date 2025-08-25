using ServiceMeshOrchestrator.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceMeshOrchestrator.Models;
using ServiceMeshOrchestrator.Models.BodyModels;
using ServiceMeshOrchestrator.Models.Modeller;

namespace ServiceMeshOrchestrator.Services
{
    public interface IManagementService
    {
        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetUserByUserId(string userId);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<List<Machine>> GetMachineByMachineId(string machineId);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<List<Application>> GetApplicationByApplicationId(string applicationId);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<CouchDbCreateDocumentResult> PostServiceUser(UserBody body);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<CouchDbCreateDocumentResult> PostServiceMachine(MachineBody machine);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<CouchDbCreateDocumentResult> PostServiceApplication(ApplicationBody body);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteServiceUser(string userId);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteServiceMachine(string machineId);

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteServiceApplication(string applicationId);






















        /// <summary>
        /// Get pages and group.
        /// </summary>
        /// <returns></returns>
        //Task<ServiceManagementResponse> DeleteKeywordfromBricks(string id, string name, string pageId);

    }
}

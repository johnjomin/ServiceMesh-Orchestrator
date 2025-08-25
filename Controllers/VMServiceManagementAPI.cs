using ServiceMeshOrchestrator.Models;
using ServiceMeshOrchestrator.Models.BodyModels;
using ServiceMeshOrchestrator.Models.Modeller;
using ServiceMeshOrchestrator.Models.Response;
using ServiceMeshOrchestrator.Services;

namespace ServiceMeshOrchestrator.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ServiceMeshOrchestrator.Services;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Globalization;
    using Microsoft.AspNetCore.Authorization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ServiceMeshOrchestrator.Models;
    using ServiceMeshOrchestrator.Models.Response;
    using ServiceMeshOrchestrator.Models.Modeller;
    using ServiceMeshOrchestrator.Models.BodyModels;

    public class VMServiceManagementAPI : Controller
    {
        private IManagementService ManagementService = new ManagementService();

        /// <summary>
        /// Retrieve user information from userId
        /// </summary>
        /// <response code="200">The page has been retrieved successfully.</response>
        /// <param name="userId"></param>
#if !DEBUG
        [Authorize]
#endif
        [HttpGet]
        [Route("/user/{userId}")]
        [SwaggerOperation("GetServiceUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "The user has been retrieved successfully.")]
        [SwaggerResponseHeader(statusCode: 200, name: "ETag", type: "string", description: "Entity tag.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceManagementResponse>> GetServiceUser(string userId)
        {
            List<User> getUser;
            try
            {
                getUser = await ManagementService.GetUserByUserId(userId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unable to retrieve users from \nUser ID: {userId}. \n\nException: {e.Message}");
            }

            if (getUser.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Unable to retrieve users from \nUser ID: {userId}. \n\nDue to User not Found");
            }

            return Ok(getUser);
        }

        /// <summary>
        /// Retrieve machine information from machineId
        /// </summary>
        /// <response code="200">The page has been retrieved successfully.</response>
        /// <param name="machineId"></param>
        [HttpGet]
        [Route("/machine/{machineId}")]
        [SwaggerOperation("GetServiceMachine")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "The machine has been retrieved successfully.")]
        [SwaggerResponseHeader(statusCode: 200, name: "ETag", type: "string", description: "Entity tag.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> GetServiceMachine(string machineId)
        {
            /*
                GET /machine/<uuid>
                Returns 200 - OK and Machine
                Returns 404 - NotFound
            */

            List<Machine> getMachine;

            try
            {
                getMachine = await ManagementService.GetMachineByMachineId(machineId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unable to retrieve machines from \nMachine ID: {machineId}. \n\nException: {e.Message}");
            }

            if (getMachine.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Unable to retrieve machines from \nMachine ID: {machineId}. \n\nDue to Machine not Found");
            }

            return Ok(getMachine);
        }

        /// <summary>
        /// Retrieve application information from applicationId
        /// </summary>
        /// <response code="200">The page has been retrieved successfully.</response>
        /// <param name="applicationId"></param>
        [HttpGet]
        [Route("/application/{applicationId}")]
        [SwaggerOperation("GetServiceApplication")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "The application has been retrieved successfully.")]
        [SwaggerResponseHeader(statusCode: 200, name: "ETag", type: "string", description: "Entity tag.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> GetServiceApplication(string applicationId)
        {
            /*
                GET /application/<uuid>
                Returns 200 - OK and Application
                Returns 404 - NotFound
            */

            List<Application> getApplication;

            try
            {
                getApplication = await ManagementService.GetApplicationByApplicationId(applicationId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unable to retrieve application from \nApplication ID: {applicationId}. \n\nException: {e.Message}");
            }

            if (getApplication.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Unable to retrieve machines from \nApplication ID: {applicationId}. \n\nDue to Application not Found");
            }

            return Ok(getApplication);
        }

        /// <summary>
        /// Create user information
        /// </summary>
        /// <response code="201">The page has been retrieved successfully.</response>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/user")]
        [SwaggerOperation("CreateServiceUser")]
        [SwaggerResponse(statusCode: 201, type: typeof(User), description: "The user has been created successfully.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<CouchDbCreateDocumentResult>> CreateServiceUser([FromBody] UserBody body)
        {
            CouchDbCreateDocumentResult postUser;

            try
            {
                if (string.IsNullOrEmpty(body.name) || string.IsNullOrEmpty(body.email) || string.IsNullOrEmpty(body.password))
                {
                    return StatusCode(StatusCodes.Status409Conflict, $"Unable to create user. \n\nDue to missing parameters");
                }

                postUser = await ManagementService.PostServiceUser(body);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server no issue, from \nUser: {body}. \n\nException: {e.Message}");
            }

            return Ok(postUser.IsSuccessful);
        }

        /// <summary>
        /// Create machine information
        /// </summary>
        /// <response code="201">The page has been retrieved successfully.</response>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/machine")]
        [SwaggerOperation("CreateServiceMachine")]
        [SwaggerResponse(statusCode: 201, type: typeof(User), description: "The machine has been created successfully.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> CreateServiceMachine([FromBody] MachineBody body)
        {
            CouchDbCreateDocumentResult postMachine;

            try
            {
                if ((body.owners == null || body.owners.Count == 0) || (body.applications == null || body.applications.Count == 0) || string.IsNullOrEmpty(body.address) || body.port == 0)
                {
                    return StatusCode(StatusCodes.Status409Conflict, $"Unable to create machine. \n\nDue to missing parameters");
                }

                postMachine = await ManagementService.PostServiceMachine(body);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server no issue, from \nMachine: {body}. \n\nException: {e.Message}");
            }

            return Ok(postMachine.IsSuccessful);
        }

        /// <summary>
        /// Create application information
        /// </summary>
        /// <response code="201">The page has been retrieved successfully.</response>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/application")]
        [SwaggerOperation("CreateServiceApplication")]
        [SwaggerResponse(statusCode: 201, type: typeof(User), description: "The application has been created successfully.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> CreateServiceApplication([FromBody] ApplicationBody body)
        {
            CouchDbCreateDocumentResult postApplication;

            try
            {
                if (string.IsNullOrEmpty(body.container) || string.IsNullOrEmpty(body.status))
                {
                    return StatusCode(StatusCodes.Status409Conflict, $"Unable to create application. \n\nDue to missing parameters");
                }

                postApplication = await ManagementService.PostServiceApplication(body);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server no issue, from \nApplication: {body}. \n\nException: {e.Message}");
            }

            return Ok(postApplication.IsSuccessful);
        }

        /// <summary>
        /// Delete user information
        /// </summary>
        /// <response code="200">The page has been retrieved successfully.</response>
        /// <param name="userId"></param>
        [HttpDelete]
        [Route("/user/{userId}")]
        [SwaggerOperation("DeleteServiceUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "The user has been deleted successfully.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> DeleteServiceUser(string userId)
        {
            /*
                DELETE /user
                Returns 200 - OK
                Returns 401 - Unauthorized
            */

            bool deleteUser;

            try
            {
                deleteUser = await ManagementService.DeleteServiceUser(userId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Unable to create user from \nUser Id: {userId}. \n\nException: {e.Message}");
            }

            return Ok(deleteUser);
        }

        /// <summary>
        /// Delete machine information
        /// </summary>
        /// <response code="200">The page has been retrieved successfully.</response>
        /// <param name="machineId"></param>
        [HttpDelete]
        [Route("/machine/{machineId}")]
        [SwaggerOperation("DeleteServiceMachine")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "The user has been deleted successfully.")]
        [SwaggerResponseHeader(statusCode: 200, name: "ETag", type: "string", description: "Entity tag.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> DeleteServiceMachine(string machineId)
        {
            /*
                DELETE /machine
                Returns 200 - OK
                Returns 401 - Unauthorized
            */

            bool deleteMachine;

            try
            {
                deleteMachine = await ManagementService.DeleteServiceMachine(machineId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Unable to create user from \nUser Id: {machineId}. \n\nException: {e.Message}");
            }

            return Ok(deleteMachine);
        }

        /// <summary>
        /// Delete application information
        /// </summary>
        /// <response code="200">The page has been retrieved successfully.</response>
        /// <param name="applicationId"></param>
        [HttpDelete]
        [Route("/application/{applicationId}")]
        [SwaggerOperation("DeleteServiceApplication")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "The user has been deleted successfully.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
#if !DEBUG
        [Authorize]
#endif
        public async Task<ActionResult<ServiceManagementResponse>> DeleteServiceApplication(string applicationId)
        {
            /*
                DELETE /application
                Returns 200 - OK
                Returns 401 - Unauthorized
            */

            bool deleteApplication;

            try
            {
                deleteApplication = await ManagementService.DeleteServiceApplication(applicationId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, $"Unable to create user from \nApplication Id: {applicationId}. \n\nException: {e.Message}");
            }

            return Ok(deleteApplication);
        }
    }
}
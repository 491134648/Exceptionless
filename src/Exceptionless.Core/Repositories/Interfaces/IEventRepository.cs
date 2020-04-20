﻿using System;
using System.Threading.Tasks;
using Exceptionless.Core.Extensions;
using Exceptionless.Core.Models;
using Exceptionless.Core.Repositories.Queries;
using Foundatio.Repositories;
using Foundatio.Repositories.Models;

namespace Exceptionless.Core.Repositories {
    public interface IEventRepository : IRepositoryOwnedByOrganizationAndProject<PersistentEvent> {
        Task<FindResults<PersistentEvent>> GetByReferenceIdAsync(string projectId, string referenceId);
        Task<FindResults<PersistentEvent>> GetByFilterAsync(AppFilter systemFilter, string userFilter, string sort, string field, DateTime utcStart, DateTime utcEnd, CommandOptionsDescriptor<PersistentEvent> options = null);
        Task<PreviousAndNextEventIdResult> GetPreviousAndNextEventIdsAsync(PersistentEvent ev, AppFilter systemFilter = null, string userFilter = null, DateTime? utcStart = null, DateTime? utcEnd = null);

        Task<FindResults<PersistentEvent>> GetOpenSessionsAsync(DateTime createdBeforeUtc, CommandOptionsDescriptor<PersistentEvent> options = null);
        Task<bool> UpdateSessionStartLastActivityAsync(string id, DateTime lastActivityUtc, bool isSessionEnd = false, bool hasError = false, bool sendNotifications = true);
        Task<CountResult> GetCountByProjectIdAsync(string projectId, bool includeDeleted = false);
        Task<long> RemoveAllAsync(string[] organizationIds, string[] projectIds, string[] stackIds, string[] eventIds, string clientIpAddress, DateTime? utcStart, DateTime? utcEnd, CommandOptionsDescriptor<PersistentEvent> options = null);
    }

    public static class EventRepositoryExtensions {
        public static async Task<PreviousAndNextEventIdResult> GetPreviousAndNextEventIdsAsync(this IEventRepository repository, string id, AppFilter systemFilter = null, string userFilter = null, DateTime? utcStart = null, DateTime? utcEnd = null) {
            var ev = await repository.GetByIdAsync(id, o => o.Cache()).AnyContext();
            return await repository.GetPreviousAndNextEventIdsAsync(ev, systemFilter, userFilter, utcStart, utcEnd).AnyContext();
        }
    }
}
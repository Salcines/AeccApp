﻿using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public class HospitalRequestService : IHospitalRequestService
    {
        private readonly IHttpRequestProvider _requestProvider;
        private readonly IIdentityService _identityService;

        public HospitalRequestService(IIdentityService identityService, IHttpRequestProvider requestProvider)
        {
            _identityService = identityService;
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<RequestType>> GetRequestTypesAsync(CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/RequestTypes",
                Query = $"requestSource={RequestSourceEnum.Hospital.ToString()}"
            };
            var identityToken = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<IEnumerable<RequestType>>(uribuilder.ToString(), cancelToken, identityToken);
        }

        public async Task<IEnumerable<Hospital>> GetHospitalsAsync(string province, CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = "api/Hospitals",
                Query = $"province={province}"
            };
            var identityToken = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<IEnumerable<Hospital>>(uribuilder.ToString(), cancelToken, identityToken);
        }

        public async Task<Hospital> GetHospitalDetail(int hospitalId, CancellationToken cancelToken)
        {
            UriBuilder uribuilder = new UriBuilder(GlobalSetting.Instance.ApiEndpoint)
            {
                Path = $"api/Hospitals/{hospitalId}"
            };
            var identityToken = await _identityService.GetTokenAsync();
            return await _requestProvider.GetAsync<Hospital>(uribuilder.ToString(), cancelToken, identityToken);
        }
    }
}

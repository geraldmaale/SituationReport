using GreatIdeas.Blazor.Extensions;
using GreatIdeas.Extensions;
using MapsterMapper;
using Microsoft.JSInterop;
using Serilog;
using SR.Data;
using SR.Data.Repositories.Contracts;
using SR.Shared;
using SR.Shared.DTOs.Crews;
using SR.Shared.DTOs.Revenues;
using SR.Shared.Params;

namespace SR.Components.DataServices;

public class ReportsDataService : IReportsDataService
{
	private readonly IRevenueRepository _revenueCollectionRepository;
	private readonly ICrewProcessingRepository _crewProcessingRepository;
	private readonly IMapper _mapper = new Mapper();
	private readonly IJSRuntime _jsRuntime;
	private readonly ExportFileHelper _exportFileHelper;

	public ReportsDataService(IRevenueRepository revenueCollectionRepository,
		IJSRuntime jsRuntime, ExportFileHelper exportFileHelper, ICrewProcessingRepository crewProcessingRepository)
	{
		_revenueCollectionRepository = revenueCollectionRepository;
		_jsRuntime = jsRuntime;
		_exportFileHelper = exportFileHelper;
		_crewProcessingRepository = crewProcessingRepository;
	}

	public async Task<ApiResult> ExportRevenues(PagingParameters pagingParams, CancellationToken cancellationToken)
	{
		try
		{
			var results = await _revenueCollectionRepository.GetRevenueCollectionDetails(pagingParams);

			var exporter = _exportFileHelper.GenerateExcel<RevenueCollectionDetailExportDto>(
				  results, pagingParams.Title, pagingParams.ExportType, false);

			//var result = await _revenueCollectionRepository.Export(pagingParams);
			Log.Information($"Revenue Collections exported successfully");

			await _jsRuntime.SaveAs(pagingParams.FileName + ".xlsx", exporter.ToArray());

			return new ApiResult()
			{
				IsSuccessful = true,
				Message = "Revenue Collection exported successfully",
			};
		}
		catch (Exception e)
		{
			Log.Fatal(e, e.Message);
			return new ApiResult()
			{
				Message = StatusLabels.ExportError
			};
		}
	}
	
	public async Task<ApiResult> ExportEmbarkations(PagingParameters pagingParams)
	{
		try
		{
			var results = await _crewProcessingRepository.GetCrewProcessingDetails(pagingParams);

			var exporter = _exportFileHelper.GenerateExcel<EmbarkationExportDto>(
				results.Embarkations.ToList(), pagingParams.Title, pagingParams.ExportType, true);

			Log.Information($"Embarkations exported successfully");

			await _jsRuntime.SaveAs(pagingParams.FileName + ".xlsx", exporter.ToArray());

			return new ApiResult()
			{
				IsSuccessful = true,
				Message = "Embarkations exported successfully",
			};
		}
		catch (Exception)
		{
			return new ApiResult() {Message = StatusLabels.ExportError};
		}
	}
}
﻿using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace SR.Web.Helpers;

public abstract class ListServiceComponentBase<TBase, TDto> : ServiceComponentBase<TBase> where TBase : class where TDto : class
{
    protected IEnumerable<TDto> DataSource { get; set; } = new List<TDto>();

    [Parameter]
    public IReadOnlyList<TDto> Data { get; set; }

    protected readonly int[] PageSizeOption = { 10, 20, 50 };

    public RadzenDataGrid<TDto> TableGrid;

    protected IList<TDto> SelectedItems = null;
    
    protected int ActivePageIndex { get; set; } = 1;
    
    public TDto SelectedItem { get; set; }
    
    protected virtual void OnRowSelected(TDto dto)
    {
        SelectedItem = dto;
        IsDisabled = false;
        VisibleProperty = true;
    }

    protected virtual void OnRowDeSelected(TDto dto)
    {
        SelectedItem = null;
        IsDisabled = true;
        VisibleProperty = false;
    }

    protected virtual Task OnLoadData()
    {
        return null;
    }

    protected virtual Task OnNewDataAsync()
    {
        return null;
    }

    protected virtual void OnNewData() { }

    protected virtual Task OnEditDataAsync(TDto dto)
    {
        return null;
    }
    
    protected virtual Task OnEditDataAsync()
    {
        return null;
    }

    protected virtual void OnDetailData(TDto dto) { }

    protected virtual Task OnDeleteDataAsync(TDto dto)
    {
        return null;
    }
}

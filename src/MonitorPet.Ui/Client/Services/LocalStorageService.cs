using Microsoft.JSInterop;

namespace MonitorPet.Ui.Client.Services;

public class LocalStorageService
{
    private IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string?> GetItem(string key)
        => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    

    public async Task SetItem(string key, string value)
        => await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    

    public async Task RemoveItem(string key)
        => await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    
}

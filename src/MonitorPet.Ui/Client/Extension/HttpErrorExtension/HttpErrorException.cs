using MonitorPet.Ui.Client.Model;
using MonitorPet.Ui.Shared.Model.Exceptions;
using System.Net.Http.Json;
using System.Net;

namespace MonitorPet.Ui.Client.Extension.HttpErrorExtension
{
    internal static class HttpErrorExtension
    {
        /// <summary>
        /// Try get errors from content HTTP
        /// </summary>
        /// <remarks>
        ///     <para>If <see cref="ResultErrors.ContainsError"/> is true, the current content was dispose.</para>
        /// </remarks>
        /// <returns></returns>
        public static async Task<ResultErrors> TryGetErrors(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
                return new ResultErrors(false, Enumerable.Empty<ErrorViewModel>());

            List<ErrorViewModel> errors = new();
            try
            {
                errors.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<ErrorViewModel[]>()
                    ?? Enumerable.Empty<ErrorViewModel>());
            }
            catch { }

            if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden ||
                httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ResultErrors(true, new[] { new ErrorViewModel { Message = "Operação não autorizada." } });

            if (!errors.Any())
                return new ResultErrors(true, new[] { new ErrorViewModel { Message = "Erro durante a operação." } });

            return new ResultErrors(true, errors);
        }
    }
}

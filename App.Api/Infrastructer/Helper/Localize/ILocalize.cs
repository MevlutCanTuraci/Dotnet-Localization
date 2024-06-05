#region Imports
using App.Api.Localize;
using Microsoft.Extensions.Localization;
#endregion


namespace App.Api.Infrastructer.Helper.Localize
{
    public interface ILocalize : IStringLocalizer<Resources>
    {
        public string this[string name] { get; } // Sadece value değerini döndürmek için
        public string this[string name, params object[] arguments] { get; } // Parametrelerle birlikte value değerini döndürmek için
    }
}
#region Imports
using App.Mvc.Localize;
using Microsoft.Extensions.Localization;
#endregion


namespace App.Mvc.Infrastructer.Helper.Localize
{
    public class LocalizeHelper : ILocalize
    {
        private readonly IStringLocalizer<Resources> _localizer;

        public LocalizeHelper(IStringLocalizer<Resources> localizer)
        {
            _localizer = localizer;
        }

        public string this[string name] => _localizer[name].Value;

        public string this[string name, params object[] arguments] => _localizer[name, arguments];


        LocalizedString IStringLocalizer.this[string name] => throw new NotImplementedException();

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => throw new NotImplementedException();

        [Obsolete("Herhangi bir aktifliği yoktur.")]
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => throw new NotImplementedException();

    }
}
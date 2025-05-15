using System.Globalization;
using System.Text;

namespace QuanLyDaiLy.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Loại bỏ dấu (dấu tiếng Việt) khỏi chuỗi đầu vào.
        /// </summary>
        /// <param name="text">Chuỗi có dấu</param>
        /// <returns>Chuỗi đã loại bỏ dấu</returns>
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

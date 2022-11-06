using System.ComponentModel.DataAnnotations;

namespace DocStorage.Util.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<ValidationResult> Validate(this object obj)
        {
            var result = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, context, result, true);
            return result;
        }

        public static void ArgsNotNull(this object obj, string parameterName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}

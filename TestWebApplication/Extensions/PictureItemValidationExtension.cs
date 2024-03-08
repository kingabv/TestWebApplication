using System.ComponentModel.DataAnnotations;
using TestWebApplication.Models;

namespace TestWebApplication.Extensions
{
    internal static class PictureItemValidationExtension
    {
        /// <summary>
        ///	Validates a model decorated with System.ComponentModel.DataAnnotations attributes
        /// </summary>
        /// <returns>Validation errors</returns>
        internal static IEnumerable<ValidationResult> ValidateControllerModel<T>(this T? model) where T : class
        {
            if (model == null)
                return new List<ValidationResult> { new ValidationResult($"Cannot validate {nameof(model)}; argument is null") };

            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(model, context, validationResults, true);

            return validationResults;
        }

        /// <summary>
        /// Validates the ValidatePictureItemModel model
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Validation errors</returns>
        internal static IEnumerable<ValidationResult> ValidatePictureItemModel(this PictureItem model)
        {
            var validationResults = new List<ValidationResult>();
            validationResults.AddRange(model.ValidateControllerModel());

            {
                if (model.Id == 0)
                    validationResults.Add(new ValidationResult("The picture Item cannot have 0 as identifier"));
            }

            return validationResults;
        }
    }
}

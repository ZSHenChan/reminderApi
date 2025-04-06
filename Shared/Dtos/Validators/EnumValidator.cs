// using System.ComponentModel.DataAnnotations;

// namespace personal_ai.Dtos.Validators
// {
//   public static class EnumValidator
//   {
//     public static ValidationResult? ValidateEnum<TEnum>(object value, ValidationContext context)
//       where TEnum : struct, Enum
//     {
//       if (value == null || Enum.TryParse(typeof(TEnum), value.ToString(), true, out _))
//       {
//         return ValidationResult.Success;
//       }

//       return new ValidationResult(
//         $"Invalid value '{value}' for {context.DisplayName}. Allowed values are: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}."
//       );
//     }
//   }
// }


// * Correct Code here
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Validators
{
  public static class EnumValidator
  {
    public static ValidationResult? ValidateEnum(object value, ValidationContext context)
    {
      // Get the enum type from the validation context
      var enumType = context.ObjectType.GetProperty(context.MemberName)?.PropertyType;

      if (enumType == null || !enumType.IsEnum)
      {
        return new ValidationResult($"The field {context.DisplayName} is not a valid enum.");
      }

      if (value == null || Enum.IsDefined(enumType, value))
      {
        return ValidationResult.Success;
      }

      return new ValidationResult(
        $"Invalid value '{value}' for {context.DisplayName}. Allowed values are: {string.Join(", ", Enum.GetNames(enumType))}."
      );
    }
  }
}

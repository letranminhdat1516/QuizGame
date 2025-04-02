using System;
using System.ComponentModel.DataAnnotations;

namespace QuizGame.Service.BusinessModel
{
    public class GameModel
    {
        public int GameId { get; set; }

        [Required(ErrorMessage = "Game name is required.")]
        [StringLength(100, ErrorMessage = "Game name must be at most 100 characters.")]
        public string GameName { get; set; }

        public int? HostId { get; set; }

        [Required(ErrorMessage = "Game PIN is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Game PIN must be a 6-digit number.")]
        public string GamePin { get; set; }

        [Required(ErrorMessage = "Game status is required.")]
        public string Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.DateTime)]
        [EndTimeAfterStartTime(nameof(StartTime))]
        public DateTime? EndTime { get; set; }

        [Required(ErrorMessage = "Quiz is required.")]
        public int? QuizId { get; set; }
    }

    /// <summary>
    /// Custom validation attribute to ensure EndTime is after StartTime.
    /// </summary>
    public class EndTimeAfterStartTime : ValidationAttribute
    {
        private readonly string _startTimeProperty;

        public EndTimeAfterStartTime(string startTimeProperty)
        {
            _startTimeProperty = startTimeProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startTimeProperty = validationContext.ObjectType.GetProperty(_startTimeProperty);
            if (startTimeProperty == null)
            {
                return new ValidationResult($"Unknown property: {_startTimeProperty}");
            }

            var startTimeValue = startTimeProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
            var endTimeValue = value as DateTime?;

            if (startTimeValue.HasValue && endTimeValue.HasValue && endTimeValue <= startTimeValue)
            {
                return new ValidationResult("End time must be after start time.");
            }

            return ValidationResult.Success;
        }
    }
}

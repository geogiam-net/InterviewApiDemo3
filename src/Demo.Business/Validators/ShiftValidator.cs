using Demo.Business.Extensions;
using Demo.Business.Models;

namespace Demo.Business.Validators;

public static class ShiftValidator
{
    public static string[] ValidateDates(Shift shift)
    {
        var errors = new List<string>();

        if(shift.StartTime >= shift.EndTime)
        {
            errors.Add("Start time must be before end time.");
        }

        if (shift.StartTime.Equals(default(DateTime)))
        {
            errors.Add(default(DateTime).ToString("o") + " is not a valid start time.");
        }

        if (shift.EndTime.Equals(default(DateTime)))
        {
            errors.Add(default(DateTime).ToString("o") + " is not a valid end time.");
        }

        return errors.ToArray();
    }

    public static string[] ValidateShiftAssignment(Shift proposedShift, IEnumerable<Shift> currentShifts)
    {
        var errors = new List<string>();

        var hasTimeRangeOverlap = currentShifts.Any(p =>
            proposedShift.StartTime.IsInsideTimeRange(p.StartTime, p.EndTime)
            || proposedShift.EndTime.IsInsideTimeRange(p.StartTime, p.EndTime)
            || p.StartTime.IsInsideTimeRange(proposedShift.StartTime, proposedShift.EndTime)
            || p.EndTime.IsInsideTimeRange(proposedShift.StartTime, proposedShift.EndTime)
        );

        if (hasTimeRangeOverlap)
        {
            errors.Add("Proposed shift overlaps with an existing shift.");
        }

        return errors.ToArray();
    }
}
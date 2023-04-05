using System.Globalization;

namespace SR.Shared
{
    public static class StatusLabels
    {
        public static string GetSuccess(string entityName)
        {
            return $"{entityName} records retrieved successfully.";
        }
        
        public static string ExportSuccess(string entityName)
        {
            return $"{entityName} exported successfully.";
        }

        public static string ExportError => "Error exporting data.";
        
        public static string NotFound(string entityName)
        {
            return $"{entityName} cannot be found.";
        }

        public static string LoadFailed(string entityName)
        {
            return $"Sorry, we could not retrieve {entityName} records.";
        }

        public static string Exists(string entityName)
        {
            return $"{entityName} already exists.";
        }

        public static string InsertSuccess(string entityName)
        {
            return $"{entityName} was saved successfully.";
        }

        public static string InsertRangeSuccess(string entityName)
        {
            return $"{entityName}(s) were saved successfully.";
        }

        public static string UpdateSuccess(string entityName)
        {
            return $"{entityName} was updated successfully.";
        }

        public static string UpdateRangeSuccess(string entityName)
        {
            return $"{entityName}(s) were updated successfully.";
        }

        public static string DeleteSuccess(string entityName)
        {
            return $"{entityName} was deleted successfully.";
        }

        public static string DeleteRangeSuccess(string entityName)
        {
            return $"{entityName}(s) were deleted successfully.";
        }

        public static string ArgumentNull(string entityName)
        {
            return $"{entityName} cannot be null.";
        }

        public const string UnprocessableError = "Record could not be processed, please try again";
        public const string UnprocessableUpdateError = "Record(s) could not be updated, please try again.";
        public const string UnprocessableInsertError = "Record(s) could not be saved, please try again.";
        public const string UnprocessableUploadError = "File could not be uploaded, please try again.";
        public const string TaskCancelled = "The running task was cancelled";
        public const string ValidationErrors = "There were validation errors.";

        public const string UnprocessableDeleteError = "Record could not be deleted, it may have related records.";
        public const string DownloadSuccess = "File downloaded successfully";
        public const string DownloadFailure = "File failed to download, please try again.";

        public const string ServerError = "Error occurred in server, please try again";

        public const string ServerErrorRecords = "Records could not be retrieved from server, please try again";
        public const string AuthorizationMessage = "You do not have access rights to the resource.";
    }

    public static class DateLabels
    {
        public const string MediumDateFormat = "dd MMMM yyyy";
        public const string LongDateFormat = "D";
        public const string ShortDateFormat = "dd MMM, yyyy";
        public const string ShortDateTimeFormat = "ddd, dd MMM-yyyy, hh:mm tt";
    }

    public static class AppCultureInfo
    {
        public static string GhCurrency(this decimal amount)
        {
            return amount.ToString("C2", CultureInfo.CreateSpecificCulture("en-GH"));
        }
    }
}
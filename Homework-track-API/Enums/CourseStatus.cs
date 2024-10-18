using System.Text.Json.Serialization;

namespace Homework_track_API.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CourseStatus
{
    Active,
    Archived,
    Deleted
}
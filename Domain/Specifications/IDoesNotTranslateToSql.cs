namespace Domain.Specifications
{
    /// <summary>
    /// Specification should not be used for querying from database.
    /// Should be used for already materialized objects' validation.
    /// </summary>
    public interface IDoesNotTranslateToSql
    {
        
    }
}
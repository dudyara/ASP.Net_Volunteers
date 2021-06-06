namespace Volunteers.Services.Dto
{
    /// <summary>
    /// RequestCreateComment
    /// </summary>
    public class RequestCreateComment
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        public string Comment { get; set; }
    }
}

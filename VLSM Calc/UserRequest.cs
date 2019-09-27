namespace VLSM_Calc
{
    public class UserRequest
    {
        public int RequestedHosts { get; set; }

        /// <summary>
        /// Create a request
        /// </summary>
        /// <param name="requestedHosts"></param>
        public UserRequest(int requestedHosts)
        {
            RequestedHosts = requestedHosts;
        }

        public override string ToString()
        {
            return $"{RequestedHosts} hosts";
        }
    }
}

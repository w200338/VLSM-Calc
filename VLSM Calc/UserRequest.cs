using System;

namespace VLSM_Calc
{
    public class UserRequest : IComparable<UserRequest>
    {
        /// <summary>
        /// Amount of hosts which were requested
        /// </summary>
        public int RequestedHosts { get; }

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
            // return singular 'host' if there's only requested
            if (RequestedHosts == 1)
            {
                return "1 host";
            }

            return $"{RequestedHosts} hosts";
        }

        /// <inheritdoc />
        public int CompareTo(UserRequest other)
        {
            return other.RequestedHosts.CompareTo(RequestedHosts);
        }
    }
}

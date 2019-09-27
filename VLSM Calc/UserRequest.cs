using System;

namespace VLSM_Calc
{
    public class UserRequest : IComparable<UserRequest>
    {
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
            return $"{RequestedHosts} hosts";
        }

        /// <inheritdoc />
        public int CompareTo(UserRequest other)
        {
            return other.RequestedHosts.CompareTo(RequestedHosts);
        }
    }
}

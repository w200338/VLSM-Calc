using System;

namespace VLSM_Calc
{
    public class UserRequest : IComparable<UserRequest>
    {
        /// <summary>
        /// User given name of the request/subnet
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Amount of hosts which were requested
        /// </summary>
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
            // show name if it isn't null or empty
            if (!string.IsNullOrEmpty(Name))
            {
                return $"{Name} ({AmountOfHostsString()})";
            }

            // just show the amount of hosts
            return AmountOfHostsString();
        }

        /// <summary>
        /// String representation of the amount of hosts in the request
        /// </summary>
        /// <returns></returns>
        private string AmountOfHostsString()
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

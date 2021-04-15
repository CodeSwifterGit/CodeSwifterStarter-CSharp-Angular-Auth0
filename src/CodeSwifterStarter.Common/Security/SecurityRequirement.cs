namespace CodeSwifterStarter.Common.Security
{
    public class SecurityRequirement
    {
        public const string ReadNonSensitiveData = "ReadNonSensitiveData";
        public const string WriteNonSensitiveData = "WriteNonSensitiveData";
        public const string ReadUsers = "ReadUsers";
        public const string WriteUsers = "WriteUsers";

        public string Name { get; set; }
        public string Description { get; set; }

        public SecurityRequirement()
        {
        }

        public SecurityRequirement(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
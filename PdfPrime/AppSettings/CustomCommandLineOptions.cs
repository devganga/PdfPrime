using CommandLine;

namespace PdfPrime.AppSettings
{
    public class CustomCommandLineOptions
    {
        //[Option('e', "endyear", Required = false, HelpText = "End of Year Range.")]
        //public int EndYear { get; set; }

        [Option('e', "environment", Required = false, HelpText = "Application Environment")]
        public string? Environment { get; set; }

        [Option('p', "password", Required = false, HelpText = "Credential Password")]
        public string? Password { get; set; }

        [Option('t', "programtag", Required = false, HelpText = "Program Tag")]
        public string? ProgramTag { get; set; }

        [Option('s', "startyear", Required = false, HelpText = "Start of Year Range.")]
        public int StartYear { get; set; }

        [Option('u', "username", Required = false, HelpText = "Credential Username")]
        public string? UserName { get; set; }

        [Option('i', "input", Required = false, HelpText = "Input Path")]
        public string? Input { get; set; }

        [Option('o', "output", Required = false, HelpText = "output Path")]
        public string? Output { get; set; }

        [Option('v', "databaseserver", Required = false, HelpText = "Database Server")]
        public string? DatabaseServer { get; set; }

        [Option('d', "databasename", Required = false, HelpText = "Database Name")]
        public string? DatabaseName { get; set; }

        [Option('l', "databaselogin", Required = false, HelpText = "Database Login")]
        public string? DatabaseLogin { get; set; }

        [Option('w', "databasepw", Required = false, HelpText = "Database Pw")]
        public string? DatabasePw { get; set; }

        [Option('f', "savehtmltos3", Required = false, HelpText = "Save HTML To S3")]
        public string? SaveHTMLFileToS3 { get; set; }

        [Option('g', "savepdftos3", Required = false, HelpText = "Save pdf To S3")]
        public string? SavePDFFileToS3 { get; set; }

        [Option('b', "bucketname", Required = false, HelpText = "S3 Bucket")]
        public string? BucketName { get; set; }

        [Option('a', "accesskey", Required = false, HelpText = "S3 access key")]
        public string? AccessKey { get; set; }

        [Option('k', "secretkey", Required = false, HelpText = "S3 secret key")]
        public string? SecretKey { get; set; }

        [Option('c', "cleanuptempfiles", Required = false, HelpText = "Cleanup Temp Files")]
        public string? CleanupTempFiles { get; set; }
    }
}

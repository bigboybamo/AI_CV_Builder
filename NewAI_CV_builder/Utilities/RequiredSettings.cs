namespace NewAI_CV_builder.Utilities
{
    /// <summary>
    /// Checks the environment variables a workflow depends on before that workflow starts,
    /// so a missing .env entry surfaces as a named setting instead of an ArgumentNullException
    /// or an opaque 401 body from the AI provider.
    /// </summary>
    public static class RequiredSettings
    {
        /// <summary>
        /// Returns the names that are unset or blank, in the order given. An empty list means
        /// every setting is present and <see cref="Get"/> is safe to call for all of them.
        /// </summary>
        public static List<string> FindMissing(params string[] names)
        {
            var missing = new List<string>();

            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(name)))
                    missing.Add(name);
            }

            return missing;
        }

        /// <summary>
        /// Builds the user-facing message for the names returned by <see cref="FindMissing"/>.
        /// </summary>
        public static string DescribeMissing(IEnumerable<string> names)
        {
            var list = names.ToList();
            var heading = list.Count == 1
                ? "This action needs a setting that is not configured:"
                : "This action needs settings that are not configured:";

            return heading +
                   "\n\n" + string.Join("\n", list.Select(name => "  • " + name)) +
                   "\n\nAdd " + (list.Count == 1 ? "it" : "them") +
                   " to the .env file in the application folder and restart the app.";
        }

        /// <summary>
        /// Returns a setting's value, guaranteed non-null. Call it only after
        /// <see cref="FindMissing"/> has cleared the same name — the throw is a backstop for a
        /// caller that forgot to, not the path the UI is meant to take.
        /// </summary>
        public static string Get(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"{name} is not set.");

            return value;
        }
    }
}

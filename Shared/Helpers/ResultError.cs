using System.Collections.Immutable;

namespace Shared.Helpers
{
    public record ResultError
    {
        /// <summary>
        /// Error messages
        /// </summary>
        public ImmutableSortedDictionary<string, ImmutableSortedSet<string>> Messages { get; private init; }
            = ImmutableSortedDictionary<string, ImmutableSortedSet<string>>.Empty;

        public IEnumerable<string> AllMessageValues => Messages.SelectMany(m => m.Value);

        public ResultError Merge(ResultError resultError, string pathPrefix = "")
        {
            if (!resultError.HasMessages) return this;

            return new ResultError
            {
                Messages = Messages.Union(resultError
                        .Messages
                        .Select(
                            e => new KeyValuePair<string, ImmutableSortedSet<string>>(pathPrefix + e.Key, e.Value)))
                    .GroupBy(d => d.Key)
                    .ToImmutableSortedDictionary(
                        g => g.Key,
                        g => g.SelectMany(e => e.Value).ToImmutableSortedSet())
            };
        }

        public static ResultError WithError(string key, string message)
        {
            return new ResultError
            {
                Messages = new Dictionary<string, ImmutableSortedSet<string>>
            {
                { key, new[] { message }.ToImmutableSortedSet() }
            }.ToImmutableSortedDictionary()
            };
        }

        public static ResultError WithError(string errorMessage)
        {
            return WithError(string.Empty, errorMessage);
        }

        public static ResultError WithErrors(IEnumerable<string> errorMessages)
        {
            var resultError = new ResultError();
            foreach (var error in errorMessages)
                resultError = resultError.AppendError(string.Empty, error);
            return resultError;
        }

        public static ResultError WithErrors(IEnumerable<(string key, string[] messages)> errors)
        {
            var baseError = new ResultError();

            foreach (var error in errors)
            {
                foreach (var message in error.messages)
                {
                    baseError = baseError.AppendError(error.key, message);
                }
            }

            return baseError;
        }

        public ResultError AppendError(string key, string message)
        {
            return new ResultError
            {
                Messages = Messages.SetItem(key,
                    (Messages.GetValueOrDefault(key) ?? ImmutableSortedSet<string>.Empty).Add(message))
            };
        }

        public ResultError AppendError(string message)
        {
            return AppendError(string.Empty, message);
        }

        public bool HasMessages => Messages.Any();

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Messages.Select(m => $"{m.Key}: {string.Join(", ", Messages[m.Key])}"));
        }
    }
}

using System.Diagnostics.CodeAnalysis;

namespace Shared.Helpers
{
    public class Result : Result<Result, Unit>
    {
        public static Result<T> Success<T>(T result)
        {
            return Result<T>.Success(result);
        }

        public TResult Match<TResult>(Func<TResult> onSuccess, Func<ResultError, TResult> onFailure)
        {
            if (onSuccess == null)
            {
                throw new ArgumentNullException(nameof(onSuccess));
            }

            if (onFailure == null)
            {
                throw new ArgumentNullException(nameof(onFailure));
            }

            return IsSuccess ? onSuccess() : onFailure(Error);
        }

        public static implicit operator Result<Unit>(Result @this)
            => @this.IsSuccess ? Result<Unit>.Success() : Result<Unit>.Failure(@this.Error);
    }

    public class Result<T> : Result<Result<T>, T>
    {
        public static implicit operator Result(Result<T> @this)
            => @this.IsSuccess ? Result.Success() : Result.Failure(@this.Error);
    }

    public abstract class Result<TSelf, T> : Result<TSelf, T, ResultError>
        where TSelf : Result<TSelf, T>, new()
    {
        public static TSelf Failure(string message)
        {
            return new TSelf() { Error = ResultError.WithError(message) };
        }

        public static TSelf Failure(string key, string message)
        {
            return new TSelf() { Error = ResultError.WithError(key, message) };
        }

        public Result<T2> OnSuccess<T2>(Func<T, T2> success)
        {
            if (success == null)
            {
                throw new ArgumentNullException(nameof(success));
            }

            return IsSuccess ? Result<T2>.Success(success(Value)) : Result<T2>.Failure(Error);
        }

        public TSelf Merge(TSelf other)
        {
            if (IsSuccess && other.IsSuccess)
            {
                return new TSelf();
            }

            if (IsFailure && other.IsSuccess)
            {
                return (TSelf)this;
            }

            if (IsSuccess && other.IsFailure)
            {
                return other;
            }

            if (IsFailure && other.IsFailure)
            {
                return new TSelf { Error = Error.Merge(other.Error) };
            }

            throw new InvalidOperationException("Impossible state.");
        }

        public override string ToString()
        {
            return IsSuccess
                ? "Success. No errors."
                : $"Failure. Errors: {string.Join(",", Error.Messages.SelectMany(x => x.Value))}";
        }
    }

    public abstract class Result<TSelf, T, TError>
        where TSelf : Result<TSelf, T, TError>, new()
    {
        public T? Value { get; private init; }
        public TError? Error { get; protected init; }

        [MemberNotNullWhen(true, nameof(Value))]
        [MemberNotNullWhen(false, nameof(Error))]
        public bool IsSuccess => Error == null;

        [MemberNotNullWhen(true, nameof(Error))]
        [MemberNotNullWhen(false, nameof(Value))]
        public bool IsFailure => Error != null;

        public static TSelf Failure(TError error)
        {
            return new TSelf() { Error = error };
        }

        public static TSelf Failure<TSelfOther, TOther>(Result<TSelfOther, TOther, TError> other)
            where TSelfOther : Result<TSelfOther, TOther, TError>, new()
        {
            if (other.IsSuccess) throw new ArgumentException("Other result is not a failure.", nameof(other));

            return Failure(other.Error);
        }

        public static TSelf Success(T result)
        {
            return new TSelf { Value = result };
        }

        public static TSelf Success()
        {
            if (typeof(T) != typeof(Unit))
                throw new InvalidOperationException("Cannot create Result without Value.");

            return new TSelf();
        }

        public TResult Match<TResult>(Func<T, TResult> success, Func<TError, TResult> error)
        {
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return IsSuccess ? success(Value) : error(Error);
        }

        public async Task<TResult> Match<TResult>(Func<T, Task<TResult>> success, Func<TError, Task<TResult>> error)
        {
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return IsSuccess ? await success(Value) : await error(Error);
        }

        public async Task<TResult> Match<TResult>(Func<T, TResult> success, Func<TError, Task<TResult>> error)
        {
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return IsSuccess ? success(Value) : await error(Error);
        }

        public async Task<TResult> Match<TResult>(Func<T, Task<TResult>> success, Func<TError, TResult> error)
        {
            if (success == null)
                throw new ArgumentNullException(nameof(success));
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return IsSuccess ? await success(Value) : error(Error);
        }
    }

}